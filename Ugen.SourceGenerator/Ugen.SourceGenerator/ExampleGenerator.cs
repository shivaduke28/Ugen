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
    const string AttributeName = "UgenNode";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 対象のクラス宣言を収集する
        var classDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                $"Ugen.{AttributeName}Attribute",
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
        var namespaceName = symbol.ContainingNamespace.ToDisplayString();

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
                var genericType = namedType.TypeArguments.FirstOrDefault();
                if (genericType != null)
                {
                    inputPorts.Add(new PortInfo
                    {
                        FieldName = field.Name,
                        TypeFullName = genericType.ToDisplayString()
                    });
                }
            }
            // UgenOutput<T> かチェック
            else if (IsUgenOutput(namedType))
            {
                var genericType = namedType.TypeArguments.FirstOrDefault();
                if (genericType != null)
                {
                    outputPorts.Add(new PortInfo
                    {
                        FieldName = field.Name,
                        TypeFullName = genericType.ToDisplayString()
                    });
                }
            }
        }

        return new ClassInfo
        {
            ClassName = className,
            NamespaceName = namespaceName,
            InputPorts = inputPorts,
            OutputPorts = outputPorts
        };
    }

    static bool IsUgenInput(INamedTypeSymbol type)
    {
        return type.Name == "UgenInput" &&
               type.ContainingNamespace.ToDisplayString() == "Ugen.Graph" &&
               type.IsGenericType;
    }

    static bool IsUgenOutput(INamedTypeSymbol type)
    {
        return type.Name == "UgenOutput" &&
               type.ContainingNamespace.ToDisplayString() == "Ugen.Graph" &&
               type.IsGenericType;
    }

    static void GenerateSource(SourceProductionContext context, ClassInfo classInfo)
    {
        var inputPortsCode = GeneratePortsArray(classInfo.InputPorts);
        var outputPortsCode = GeneratePortsArray(classInfo.OutputPorts);

        var source = $$"""
                       using System;
                       using Ugen.Serialization;
                       using UnityEngine;
                       
                       namespace {{classInfo.NamespaceName}}
                       {
                           public partial class {{classInfo.ClassName}}
                           {
                               [Serializable]
                               public class {{classInfo.ClassName}}Data : UgenNodeData
                               {
                                   public {{classInfo.ClassName}}Data()
                                   {
                                   }
                       
                                   public override Port[] InputPorts => new Port[]
                                   {
                       {{inputPortsCode}}
                                   };
                       
                                   public override Port[] OutputPorts => new Port[]
                                   {
                       {{outputPortsCode}}
                                   };
                               }
                           }
                       }
                       """;

        var fileName = $"{classInfo.ClassName}.g.cs";
        context.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
    }

    static string GeneratePortsArray(List<PortInfo> ports)
    {
        if (ports.Count == 0)
            return "";

        var sb = new StringBuilder();
        for (var i = 0; i < ports.Count; i++)
        {
            var port = ports[i];
            sb.Append($"                new Port({i}, \"{port.FieldName}\", typeof({port.TypeFullName})),\n");
        }
        return sb.ToString();
    }

    class ClassInfo
    {
        public string ClassName { get; set; }
        public string NamespaceName { get; set; }
        public List<PortInfo> InputPorts { get; set; }
        public List<PortInfo> OutputPorts { get; set; }
    }

    class PortInfo
    {
        public string FieldName { get; set; }
        public string TypeFullName { get; set; }
    }
}
