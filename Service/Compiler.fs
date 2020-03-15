namespace Gone.Service

open Gone.Syntax
open Gone.Parser


open Mono.Cecil
open Mono.Cecil.Cil

type Env =
    {
        //Types : Map<string, TypeReference>
        //Imports : Map<string, Env>
        //Functions : Map<string, MethodReference>

        VoidType : TypeReference
        Module : ModuleDefinition
    }
    member this.Push () = this

module Intermediate =

    type CompiledPackage =
        {
            Name : string
            PackageType : TypeDefinition
            GlobalFunctions : GlobalFunction[]
        }

    and GlobalFunction =
        {
            Name : string
            Method : MethodDefinition
            CompileBody : unit -> unit
        }

    let rec buildItermediate (env : Env) (files : SourceFile[]) : CompiledPackage =

        let packageName = files.[0].Package
        let ns = "Packages"
        let tattrs =
            TypeAttributes.Public
            ||| TypeAttributes.Abstract
            ||| TypeAttributes.Sealed
            ||| TypeAttributes.BeforeFieldInit
        let packageType = new TypeDefinition (ns, packageName, tattrs)
        env.Module.Types.Add (packageType)

        {
            Name = files.[0].Package
            PackageType = packageType
            GlobalFunctions =
                files
                |> Array.collect (findFuncs env packageType)
        }

    and clrTypeForGoType (env : Env) (typ : Type) : TypeReference =
        failwithf "I never heard of %A" typ

    and clrTypeForFunctionResult (env : Env) (result : FunctionResult option) : TypeReference =
        match result with
        | Some (ResultType t) -> clrTypeForGoType env t
        | None -> env.VoidType
        | _ -> failwithf "Can't find result type for %A" result

    and findFuncs (env : Env) (packageType : TypeDefinition) (file : SourceFile) : GlobalFunction[] =
        file.Declarations
        |> Array.choose (function
            | FunctionDecl data ->
                let s = data.Signature
                let resultType = clrTypeForFunctionResult env s.Result
                let parameters =
                    s.Parameters
                    |> Array.collect (fun p ->
                        let ptype = clrTypeForGoType env p.ParameterType
                        p.Identifiers
                        |> Array.map (fun pname ->
                            new ParameterDefinition(ptype, Name = pname)))
                let mattrs = MethodAttributes.Public
                             ||| MethodAttributes.Static
                             ||| MethodAttributes.HideBySig
                let method = new MethodDefinition (data.Name, mattrs, resultType)
                for p in parameters do method.Parameters.Add p
                let compileThisMethod () = compileMethod env data method
                packageType.Methods.Add (method)
                Some {
                    Name = data.Name
                    Method = method
                    CompileBody = compileThisMethod
                }
            | _ -> None)

    and compileMethod (topEnv : Env) (data : FunctionDeclData) (method : MethodDefinition) : unit =

        let body = new MethodBody (method)
        let il = body.GetILProcessor ()

        let emit instruction =
            il.Append (instruction)
        let pop () = emit (il.Create (OpCodes.Pop))

        let rec compileBlock (env : Env) (block : BlockData) =
            for s in block.Statements do
                compileStmt env s

        and compileStmt (env : Env) (stmt : Statement) =
            match stmt with
            | Block b -> compileBlock (env.Push()) b
            | ExpressionStmt e ->
                compileExpression env e
                pop ()

        and compileExpression (env : Env) (expr : Expression) =
            ()
            failwithf "I don't know how to compile %A" expr

        match data.Body with
        | None -> failwithf "Can't compile function without a body %A" data
        | Some fbody -> compileBlock topEnv fbody



type Compiler () =

    let compileFiles (files : SourceFile[]) =

        let netstdPath = "/Users/fak/.nuget/packages/netstandard.library/2.0.0/build/netstandard2.0/ref/netstandard.dll"
        let netstd = AssemblyDefinition.ReadAssembly(netstdPath)

        let packageName = files.[0].Package

        let asmName = AssemblyNameDefinition (packageName, new System.Version (1, 0))
        let asm = AssemblyDefinition.CreateAssembly (asmName, packageName, ModuleKind.Dll)

        let m = asm.MainModule

        let lookupNetStdType (fullName : string) : TypeReference =
            netstd.MainModule.Types
            |> Seq.find(fun f -> f.FullName = fullName)
            |> m.ImportReference

        let initialEnv =
            {
                Module = m
                VoidType = lookupNetStdType "System.Void"
            }
        let intermediate = Intermediate.buildItermediate initialEnv files

        intermediate.GlobalFunctions
        |> Seq.iter (fun x -> x.CompileBody ())

        asm



    member this.Compile (code : string) : AssemblyDefinition =

        let parser = GoParser ()

        let sourceFile = parser.Parse code

        let r = compileFiles [| sourceFile |]
        r

