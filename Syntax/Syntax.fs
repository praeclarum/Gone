module Gone.Syntax


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


type Expression = string
and ExpressionList = Expression list



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







