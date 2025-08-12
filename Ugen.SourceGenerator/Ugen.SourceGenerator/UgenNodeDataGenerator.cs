using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Ugen.SourceGenerator;

[Generator]
public class UgenNodeDataGenerator : IIncrementalGenerator
{
    // Attribute names
    const string UgenNodeAttributeName = "UgenNodeAttribute";
    const string UgenInputAttributeName = "UgenInputAttribute";
    const string UgenOutputAttributeName = "UgenOutputAttribute";

    // Namespaces
    const string UgenAttributesNamespace = "Ugen.Attributes";
    const string UgenGraphNamespace = "Ugen.Graph";
    const string UgenSerializationNamespace = "Ugen.Serialization";

    // Type names
    const string UgenInputTypeName = "UgenInput";
    const string UgenOutputTypeName = "UgenOutput";
    const string PortTypeName = "Port";
    const string UgenNodeDataTypeName = "UgenNodeData";

    // Suffix
    const string DataClassSuffix = "Data";
    const string GeneratedFileSuffix = ".g.cs";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 対象のクラス宣言を収集する
        var classDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                $"{UgenAttributesNamespace}.{UgenNodeAttributeName}",
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, _) => GetClassInfo(ctx));

        // 収集したクラス宣言に対してコードを生成する
        context.RegisterSourceOutput(classDeclarations.Where(m => m is not null),
            static (spc, model) => { GenerateSource(spc, model!); });
    }

    static ClassInfo GetClassInfo(GeneratorAttributeSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.TargetNode;
        var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
        if (symbol == null) return null;

        var className = symbol.Name;
        symbol.ContainingNamespace.ToDisplayString();

        // フィールドを収集
        var fields = symbol.GetMembers()
            .OfType<IFieldSymbol>()
            .Where(f => !f.IsStatic && !f.IsConst);

        var inputPorts = new List<PortInfo>();
        var outputPorts = new List<PortInfo>();

        foreach (var field in fields)
        {
            var fieldType = field.Type;
            if (fieldType is not INamedTypeSymbol namedType) continue;

            // UgenInput<T> かチェック
            if (IsUgenInput(namedType))
            {
                var portInfo = ExtractPortInfo(field, namedType, UgenInputAttributeName);
                if (portInfo.HasValue)
                {
                    inputPorts.Add(portInfo.Value);
                }
            }
            // UgenOutput<T> かチェック
            else if (IsUgenOutput(namedType))
            {
                var portInfo = ExtractPortInfo(field, namedType, UgenOutputAttributeName);
                if (portInfo.HasValue)
                {
                    outputPorts.Add(portInfo.Value);
                }
            }
        }

        // indexでソート
        inputPorts.Sort((a, b) => a.Index.CompareTo(b.Index));
        outputPorts.Sort((a, b) => a.Index.CompareTo(b.Index));

        return new ClassInfo
        {
            ClassName = className,
            InputPorts = inputPorts,
            OutputPorts = outputPorts
        };
    }

    static PortInfo? ExtractPortInfo(IFieldSymbol field, INamedTypeSymbol namedType, string attributeName)
    {
        var genericType = namedType.TypeArguments.FirstOrDefault();
        if (genericType == null) return null;

        // 対応するAttribute を取得
        var attribute = field.GetAttributes()
            .FirstOrDefault(a => a.AttributeClass?.Name == attributeName &&
                                 a.AttributeClass?.ContainingNamespace?.ToDisplayString() == UgenAttributesNamespace);

        if (attribute == null) return null;

        // index と name を取得
        var index = (int)attribute.ConstructorArguments[0].Value!;
        var name = attribute.ConstructorArguments.Length > 1 &&
                   attribute.ConstructorArguments[1].Value is string attrName &&
                   !string.IsNullOrEmpty(attrName)
            ? attrName
            : field.Name; // nameが空の場合はフィールド名を使用

        return new PortInfo(index, name, genericType.ToDisplayString());
    }

    static bool IsUgenInput(INamedTypeSymbol type)
    {
        return type.Name == UgenInputTypeName &&
               type.ContainingNamespace.ToDisplayString() == UgenGraphNamespace &&
               type.IsGenericType;
    }

    static bool IsUgenOutput(INamedTypeSymbol type)
    {
        return type.Name == UgenOutputTypeName &&
               type.ContainingNamespace.ToDisplayString() == UgenGraphNamespace &&
               type.IsGenericType;
    }

    static void GenerateSource(SourceProductionContext context, ClassInfo classInfo)
    {
        var inputPortsCode = GeneratePortsArray(classInfo.InputPorts);
        var outputPortsCode = GeneratePortsArray(classInfo.OutputPorts);

        var source = $$"""
                       using System;
                       using UnityEngine;

                       namespace {{UgenSerializationNamespace}}
                       {
                           [Serializable]
                           public sealed class {{classInfo.ClassName}}{{DataClassSuffix}} : {{UgenNodeDataTypeName}}
                           {
                               {{PortTypeName}}[] inputPorts;
                               {{PortTypeName}}[] outputPorts;
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

        var fileName = $"{classInfo.ClassName}{DataClassSuffix}{GeneratedFileSuffix}";
        context.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
    }

    static string GeneratePortsArray(List<PortInfo> ports)
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

    class ClassInfo
    {
        public string ClassName { get; set; }
        public List<PortInfo> InputPorts { get; set; }
        public List<PortInfo> OutputPorts { get; set; }
    }

    readonly struct PortInfo
    {
        public readonly int Index;
        public readonly string Name;
        public readonly string TypeFullName;

        public PortInfo(int index, string name, string typeFullName)
        {
            Index = index;
            Name = name;
            TypeFullName = typeFullName;
        }
    }
}
