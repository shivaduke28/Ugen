using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Ugen.SourceGenerator;

[Generator]
public class UgenBehaviourNodeDataGenerator : IIncrementalGenerator
{
    // Attribute names
    const string UgenBehaviourAttributeName = "UgenBehaviourAttribute";
    const string UgenInputAttributeName = "UgenInputAttribute";
    const string UgenOutputAttributeName = "UgenOutputAttribute";

    // Namespaces
    const string UgenAttributesNamespace = "Ugen.Attributes";
    const string UgenBehavioursNamespace = "Ugen.Behaviours";
    const string UgenGraphNamespace = "Ugen.Graph";
    const string UgenSerializationNamespace = "Ugen.Serialization";

    // Type names
    const string UgenInputTypeName = "UgenInput";
    const string UgenOutputTypeName = "UgenOutput";
    const string PortTypeName = "Port";
    const string UgenNodeDataTypeName = "UgenBehaviourNodeData";

    // Suffix
    const string NodeDataClassSuffix = "NodeData";
    const string GeneratedFileSuffix = ".g.cs";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // UgenBehaviour属性がついたクラスを収集
        var classDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                $"{UgenAttributesNamespace}.{UgenBehaviourAttributeName}",
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, _) => GetClassInfo(ctx))
            .Where(m => m is not null);


        context.RegisterSourceOutput(classDeclarations, GenerateSource);
    }

    static bool IsBehaviourClass(SyntaxNode node)
    {
        if (node is not ClassDeclarationSyntax classDeclaration) return false;

        // namespace確認のためにparentを辿る
        var parent = node.Parent;
        while (parent != null)
        {
            if (parent is NamespaceDeclarationSyntax namespaceDeclaration)
            {
                var namespaceName = namespaceDeclaration.Name.ToString();
                return namespaceName == UgenBehavioursNamespace;
            }

            if (parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespace)
            {
                var namespaceName = fileScopedNamespace.Name.ToString();
                return namespaceName == UgenBehavioursNamespace;
            }

            parent = parent.Parent;
        }

        return false;
    }

    static ClassInfo GetBehaviourClassInfo(GeneratorSyntaxContext context)
    {
        var classDeclaration = context.Node as ClassDeclarationSyntax;
        if (classDeclaration == null) return null;

        var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
        if (symbol == null) return null;

        // Ugen.Behaviours名前空間でない場合はnull
        if (symbol.ContainingNamespace?.ToDisplayString() != UgenBehavioursNamespace)
            return null;

        return ExtractClassInfo(symbol);
    }

    static ClassInfo GetClassInfo(GeneratorAttributeSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.TargetNode;
        var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
        if (symbol == null) return null;

        return ExtractClassInfo(symbol);
    }

    static ClassInfo ExtractClassInfo(INamedTypeSymbol symbol)
    {
        var className = symbol.Name;

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
                var portInfo = ExtractPortInfo(field, namedType, true);
                if (portInfo.HasValue)
                {
                    inputPorts.Add(portInfo.Value);
                }
            }
            // UgenOutput<T> かチェック
            else if (IsUgenOutput(namedType))
            {
                var portInfo = ExtractPortInfo(field, namedType, false);
                if (portInfo.HasValue)
                {
                    outputPorts.Add(portInfo.Value);
                }
            }
        }

        // InitializePortsメソッド内での初期化も解析する（簡易的な実装）
        // TODO: より詳細な解析が必要な場合は拡張する

        // indexでソート（ポートにindexがない場合は0とする）
        inputPorts.Sort((a, b) => a.Index.CompareTo(b.Index));
        outputPorts.Sort((a, b) => a.Index.CompareTo(b.Index));

        return new ClassInfo
        {
            ClassName = className,
            InputPorts = inputPorts,
            OutputPorts = outputPorts
        };
    }

    static PortInfo? ExtractPortInfo(IFieldSymbol field, INamedTypeSymbol namedType, bool isInput)
    {
        var genericType = namedType.TypeArguments.FirstOrDefault();
        if (genericType == null) return null;

        // 属性から情報を取得（あれば）
        var attributeName = isInput ? UgenInputAttributeName : UgenOutputAttributeName;
        var attribute = field.GetAttributes()
            .FirstOrDefault(a => a.AttributeClass?.Name == attributeName &&
                                 a.AttributeClass?.ContainingNamespace?.ToDisplayString() == UgenAttributesNamespace);

        int index = 0;
        string name = field.Name;

        if (attribute != null)
        {
            // indexとnameを属性から取得
            if (attribute.ConstructorArguments.Length > 0)
            {
                index = (int)attribute.ConstructorArguments[0].Value!;
            }

            if (attribute.ConstructorArguments.Length > 1 &&
                attribute.ConstructorArguments[1].Value is string attrName &&
                !string.IsNullOrEmpty(attrName))
            {
                name = attrName;
            }
        }

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
                       using Ugen.Behaviours;

                       namespace {{UgenSerializationNamespace}}
                       {
                           [Serializable]
                           public sealed class {{classInfo.ClassName}}{{NodeDataClassSuffix}} : {{UgenNodeDataTypeName}}<{{classInfo.ClassName}}>
                           {
                               {{PortTypeName}}[] inputPorts;
                               {{PortTypeName}}[] outputPorts;
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

        var fileName = $"{classInfo.ClassName}{NodeDataClassSuffix}{GeneratedFileSuffix}";
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
