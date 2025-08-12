using System.Collections.Generic;
using System.Text;
using static Ugen.SourceGenerator.GeneratorConstants;
using static Ugen.SourceGenerator.GeneratorModels;

namespace Ugen.SourceGenerator;

/// <summary>
/// ソースコード生成に関する責務を持つヘルパークラス
/// </summary>
internal static class SourceCodeBuilder
{
    /// <summary>
    /// ポート配列のコードを生成する
    /// </summary>
    internal static string GeneratePortsArray(List<PortInfo> ports)
    {
        if (ports.Count == 0)
            return "";

        var sb = new StringBuilder();
        foreach (var port in ports)
        {
            sb.AppendLine($"            new {PortTypeName}({port.Index}, \"{port.Name}\", typeof({port.TypeFullName})),");
        }

        return sb.ToString();
    }

    /// <summary>
    /// UgenBehaviourNodeData用のソースコードを生成する
    /// </summary>
    internal static string GenerateBehaviourNodeDataSource(ClassInfo classInfo)
    {
        var inputPortsCode = GeneratePortsArray(classInfo.InputPorts);
        var outputPortsCode = GeneratePortsArray(classInfo.OutputPorts);

        return $$"""
                 using System;
                 using UnityEngine;
                 using Ugen.Behaviours;

                 namespace {{UgenSerializationNamespace}}
                 {
                     [Serializable]
                     public sealed class {{classInfo.ClassName}}{{NodeDataClassSuffix}} : {{UgenBehaviourNodeDataTypeName}}<{{classInfo.ClassName}}>
                     {
                         {{PortTypeName}}[] inputPorts;
                         {{PortTypeName}}[] outputPorts;
                         public override string Name => "{{classInfo.ClassName}}";

                         public {{classInfo.ClassName}}{{NodeDataClassSuffix}}()
                         {
                         }

                         public override {{PortTypeName}}[] InputPorts => inputPorts ??= new {{PortTypeName}}[]
                         {
                 {{inputPortsCode}}
                         };

                         public override {{PortTypeName}}[] OutputPorts => outputPorts ??= new {{PortTypeName}}[]
                         {
                 {{outputPortsCode}}
                         };
                     }
                 }
                 """;
    }

    /// <summary>
    /// UgenNodeData用のソースコードを生成する
    /// </summary>
    internal static string GenerateNodeDataSource(ClassInfo classInfo)
    {
        var inputPortsCode = GeneratePortsArray(classInfo.InputPorts);
        var outputPortsCode = GeneratePortsArray(classInfo.OutputPorts);

        return $$"""
                 using System;
                 using UnityEngine;

                 namespace {{UgenSerializationNamespace}}
                 {
                     [Serializable]
                     public sealed class {{classInfo.ClassName}}{{DataClassSuffix}} : {{UgenNodeDataTypeName}}
                     {
                         {{PortTypeName}}[] inputPorts;
                         {{PortTypeName}}[] outputPorts;

                         public override string Name => "{{classInfo.ClassName}}";

                         public {{classInfo.ClassName}}{{DataClassSuffix}}()
                         {
                         }

                         public override {{PortTypeName}}[] InputPorts => inputPorts ??= new {{PortTypeName}}[]
                         {
                 {{inputPortsCode}}
                         };

                         public override {{PortTypeName}}[] OutputPorts => outputPorts ??= new {{PortTypeName}}[]
                         {
                 {{outputPortsCode}}
                         };
                     }
                 }
                 """;
    }
}
