namespace Gone.Service

open Gone.Syntax

open Mono.Cecil
open Mono.Cecil.Cil
open Mono.Cecil.Rocks

//module FrankFuncs =
    //let ni () = raise (new System.NotImplementedException ())
    //let wtf s = Printf.kprintf (fun x -> raise (new System.Exception (x))) s

type Env =
    { //Types : Map<string, TypeReference>
      //Imports : Map<string, Env>
      //Functions : Map<string, MethodReference>

      Module : ModuleDefinition

      VoidType : TypeReference
      ObjectType : TypeReference
      StringType : TypeReference
      ConsoleType : TypeReference

      PackageName : string }
    member this.Push() = this

module Intermediate =

    type StackResult =
        | NoResult
        | HasResult

    type CompiledPackage =
        { Name : string
          PackageType : TypeDefinition
          GlobalFunctions : GlobalFunction list }

    and GlobalFunction =
        { Name : string
          Method : MethodDefinition
          CompileBody : unit -> unit
          PackageName : string }

    let rec buildItermediate (env : Env) (files : SourceFile list) : CompiledPackage =

        let packageName = files.[0].Package
        let ns = "Packages"
        let tattrs =
            TypeAttributes.Public ||| TypeAttributes.Abstract ||| TypeAttributes.Sealed
            ||| TypeAttributes.BeforeFieldInit
        let packageType = new TypeDefinition(ns, packageName, tattrs)
        packageType.BaseType <- env.ObjectType
        env.Module.Types.Add(packageType)

        { Name = files.[0].Package
          PackageType = packageType
          GlobalFunctions = files |> List.collect (findFuncs env packageType) }

    and clrTypeForGoType (env : Env) (typ : Type) : TypeReference = failwithf "I never heard of %A" typ

    and clrTypeForFunctionResult (env : Env) (result : FunctionResult option) : TypeReference =
        match result with
        | Some(ResultType t) -> clrTypeForGoType env t
        | None -> env.VoidType
        | _ -> failwithf "Can't find result type for %A" result

    and findFuncs (env : Env) (packageType : TypeDefinition) (file : SourceFile) : GlobalFunction list =
        file.Declarations
        |> List.choose (function
            | FunctionDecl data ->
                let s = data.Signature
                let resultType = clrTypeForFunctionResult env s.Result

                let parameters =
                    s.Parameters
                    |> List.collect (fun p ->
                        let ptype = clrTypeForGoType env p.ParameterType
                        p.Identifiers |> List.map (fun pname -> new ParameterDefinition(ptype, Name = pname)))

                let mattrs = MethodAttributes.Public ||| MethodAttributes.Static ||| MethodAttributes.HideBySig
                let method = new MethodDefinition(data.Name, mattrs, resultType)
                for p in parameters do
                    method.Parameters.Add p
                let compileThisMethod() = compileMethod env data method
                packageType.Methods.Add(method)
                Some
                    { Name = data.Name
                      Method = method
                      CompileBody = compileThisMethod
                      PackageName = env.PackageName }
            | _ -> None)

    and compileMethod (topEnv : Env) (data : FunctionDeclData) (method : MethodDefinition) : unit =

        let body = new MethodBody(method)
        let il = body.GetILProcessor()

        let emit instruction = il.Append(instruction)
        let pop() = emit (il.Create(OpCodes.Pop))

        let rec compileBlock (env : Env) (block : BlockData) =
            for s in block.Statements do
                compileStmt env s

        and compileStmt (env : Env) (stmt : Statement) =
            match stmt with
            | Block b -> compileBlock (env.Push()) b
            | ExpressionStmt e ->
                match pushExpression env e with
                | NoResult -> ()
                | HasResult -> pop()

        and pushExpression (env : Env) (expr : Expression) : StackResult =
            match expr with
            | CallExpr call ->
                for a in call.Arguments do
                    pushExpression env a |> ignore
                let methodRef = pushFunction env call.Function
                emit (il.Create(OpCodes.Call, methodRef))
                match methodRef.ReturnType.FullName with
                | "System.Void" -> NoResult
                | _ -> HasResult
            | StringLit str ->
                emit (il.Create(OpCodes.Ldstr, str))
                HasResult
            | _ -> failwithf "I don't know how to push expression %A" expr

        and pushFunction (env : Env) (fexpr : Expression) : MethodReference =
            match fexpr with
            | SelectorExpr { Parent = VariableExpr { Name = "fmt" }; Name = "Println" } ->
                let imr = new MethodReference("WriteLine", env.VoidType, env.ConsoleType)
                imr.Parameters.Add(new ParameterDefinition(env.StringType))
                let mr = env.Module.ImportReference imr
                mr
            | SelectorExpr { Parent = VariableExpr { Name = varName }; Name = funName } ->
                failwithf "I don't know how to find %A %A" varName funName
            | _ -> failwithf "IDK %A" fexpr


        match data.Body with
        | None -> failwithf "Can't compile function without a body %A" data
        | Some fbody ->
            compileBlock topEnv fbody
            emit (il.Create(OpCodes.Ret))

        body.OptimizeMacros()
        method.Body <- body

    let designateEntryPoint (env : Env) (compiledPackage : CompiledPackage) =
        let entryFunction =
            compiledPackage.GlobalFunctions
            |> Seq.find (fun f ->
                f.Name = "main" && f.PackageName = "main")
        //entryFunction.Method.Me
        //entryFunction.Method.Name <- "GoMain"
        env.Module.EntryPoint <- entryFunction.Method


type Compiler() =

    let compileFiles (files : SourceFile list) =

        let userDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile)
        let netstdPath =
            System.IO.Path.Combine
                (userDir, ".nuget", "packages", "netstandard.library", "2.0.0", "build", "netstandard2.0", "ref",
                 "netstandard.dll")
        let netstd = AssemblyDefinition.ReadAssembly(netstdPath)

        let packageName = files.[0].Package

        let asmName = AssemblyNameDefinition(packageName, new System.Version(1, 0))
        let asm = AssemblyDefinition.CreateAssembly(asmName, packageName, ModuleKind.Dll)

        let m = asm.MainModule

        let lookupNetStdType (fullName : string) : TypeReference =
            netstd.MainModule.Types
            |> Seq.find (fun f -> f.FullName = fullName)
            |> m.ImportReference

        let initialEnv =
            { 
              Module = m
              VoidType = lookupNetStdType "System.Void"
              ObjectType = lookupNetStdType "System.Object"
              StringType = lookupNetStdType "System.String"
              ConsoleType = lookupNetStdType "System.Console"
              PackageName = packageName }

        //
        // Pass 1, find types and methods
        //
        let compiledPackage = Intermediate.buildItermediate initialEnv files

        //
        // Pass 1a, designate the entry point
        //
        Intermediate.designateEntryPoint initialEnv compiledPackage


        // Pass 2, compile methods
        compiledPackage.GlobalFunctions |> Seq.iter (fun x -> x.CompileBody())

        asm



    member this.Compile(code : string) : AssemblyDefinition =

        let parser = Gone.Parser()

        let sourceFile = parser.Parse code

        let r = compileFiles [ sourceFile ]
        r
