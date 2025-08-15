namespace Ugen.SourceGenerator;

/// <summary>
/// ソースジェネレーターで使用する共通の定数定義
/// </summary>
internal static class GeneratorConstants
{
    // Attribute names
    internal const string UgenBehaviourAttributeName = "UgenBehaviourAttribute";
    internal const string UgenNodeAttributeName = "UgenNodeAttribute";
    internal const string UgenInputAttributeName = "UgenInputAttribute";
    internal const string UgenOutputAttributeName = "UgenOutputAttribute";

    // Namespaces
    internal const string UgenAttributesNamespace = "Ugen.Attributes";
    internal const string UgenBehavioursNamespace = "Ugen.Behaviours";
    internal const string UgenGraphNamespace = "Ugen.Graph";
    internal const string UgenSerializationNamespace = "Ugen.Serialization";

    // Type names
    internal const string UgenInputTypeName = "UgenInput";
    internal const string UgenOutputTypeName = "UgenOutput";
    internal const string PortTypeName = "PortData";
    internal const string UgenNodeDataTypeName = "UgenNodeData";
    internal const string UgenBehaviourNodeDataTypeName = "UgenBehaviourNodeData";

    // Suffix
    internal const string NodeDataClassSuffix = "NodeData";
    internal const string DataClassSuffix = "Data";
    internal const string GeneratedFileSuffix = ".g.cs";
}
