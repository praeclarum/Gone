namespace Gone.Syntax


type Location =
    {
        Document : string
        Line:int
        Column:int
    }
type Range = Location * Location

type Identifier = string
and IdentifierList = Identifier list

type PackageName = Identifier

type QualifiedIdent =
    {
        PackageName : PackageName
        Identifier : Identifier
    }

type Tag = string





type Statement =
    | Block of BlockData
    | ExpressionStmt of Expression

and BlockData =
    {
        Statements : Statement[]
    }
    static member Empty = { Statements = [||] }

and Expression =
    | CallExpr of CallData
    | SelectorExpr of SelectorData
    | StringLit of string
    | VariableExpr of VariableData

and VariableData =
    {
        Name : string
        Package : PackageName option
    }

and CallData =
    {
        Function : Expression
        Arguments : ExpressionList
    }

and SelectorData =
    {
        Parent : Expression
        Name : string
    }

and ExpressionList = Expression[]



type TypeName =
    | IdentifiedType of Identifier
    | QualifiedType of QualifiedIdent

type Type =
    | NamedType of TypeName
    | ArrayType of Length : Expression * ElementType
    | StructType of StructTypeData
    | PointerType of Type
    | FunctionType
    | InterfaceType
    | SliceType
    | MapType
    | ChannelType

and ElementType = Type

and StructTypeData =
    { 
        Fields : FieldDecl list
    }
and FieldDecl =
    {
        Tag : Tag
        Body : FieldBody
    }
and FieldBody =
    | IdentifiedField of Identifier list * Type
    | EmbeddedField of TypeName

and FunctionSignature =
    {
        Parameters : Parameter[]
        Result : FunctionResult option
    }
and FunctionResult =
    | ResultParameters of Parameters
    | ResultType of Type

and Parameters = Parameter[]
and Parameter =
    {
        Identifiers : Identifier[]
        ParameterType : Type
    }

type FunctionDeclData =
    {
        Name : FunctionName
        Signature : FunctionSignature
        Body : FunctionBody option
    }
and FunctionName = Identifier
and FunctionBody = BlockData

type TopLevelDecl = 
    | Declaration
    | FunctionDecl of FunctionDeclData
    | MethodDecl

and Declaration =
    | ConstDecl
    | TypeDecl
    | VarDecl

type SourceFile =
    {
        Package : PackageClause
        Imports : ImportDecl[]
        Declarations : TopLevelDecl[]
    }

and PackageClause = PackageName


and ImportDecl = ImportSpec[]
and ImportSpec = 
    {
        PackagePath : PackageName[]
        ImportPath : ImportPath
    }
and ImportPath = string








