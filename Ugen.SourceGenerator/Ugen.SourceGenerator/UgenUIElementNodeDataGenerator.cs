using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Ugen.SourceGenerator.GeneratorConstants;
using static Ugen.SourceGenerator.GeneratorModels;

namespace Ugen.SourceGenerator;

[Generator]
public sealed class UgenUIElementNodeDataGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // UgenUIElement属性がついたクラスを収集
        var classDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                $"{UgenAttributesNamespace}.UgenUIElementAttribute",
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, _) => GetClassInfo(ctx))
            .Where(m => m is not null);

        context.RegisterSourceOutput(classDeclarations, GenerateSource);
    }

    static ClassInfo GetClassInfo(GeneratorAttributeSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.TargetNode;
        var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;
        if (symbol == null) return null;

        // Ugen.UI.Nodes名前空間でない場合はnull
        if (symbol.ContainingNamespace?.ToDisplayString() != "Ugen.UI.Nodes")
            return null;

        return PortInfoExtractor.ExtractClassInfo(symbol);
    }

    static void GenerateSource(SourceProductionContext context, ClassInfo classInfo)
    {
        var source = GenerateUIElementNodeDataSource(classInfo);
        var fileName = $"{classInfo.ClassName}{NodeDataClassSuffix}{GeneratedFileSuffix}";
        context.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
    }

    /// <summary>
    /// UgenUIElementNodeData用のソースコードを生成する
    /// </summary>
    static string GenerateUIElementNodeDataSource(ClassInfo classInfo)
    {
        var inputPortsCode = SourceCodeBuilder.GeneratePortsArray(classInfo.InputPorts);
        var outputPortsCode = SourceCodeBuilder.GeneratePortsArray(classInfo.OutputPorts);

        return $$"""
                 using System;
                 using UnityEngine;
                 using Ugen.UI.Nodes;

                 namespace {{UgenSerializationNamespace}}
                 {
                     [Serializable]
                     public sealed class {{classInfo.ClassName}}{{NodeDataClassSuffix}} : UgenUIElementNodeData<{{classInfo.ClassName}}>
                     {
                         {{PortTypeName}}[] _inputPorts;
                         {{PortTypeName}}[] _outputPorts;
                         public override string Name => "{{classInfo.ClassName}}";

                         public {{classInfo.ClassName}}{{NodeDataClassSuffix}}()
                         {
                         }

                         public override {{PortTypeName}}[] InputPorts => _inputPorts ??= new {{PortTypeName}}[]
                         {
                 {{inputPortsCode}}
                         };

                         public override {{PortTypeName}}[] OutputPorts => _outputPorts ??= new {{PortTypeName}}[]
                         {
                 {{outputPortsCode}}
                         };
                     }
                 }
                 """;
    }
}
