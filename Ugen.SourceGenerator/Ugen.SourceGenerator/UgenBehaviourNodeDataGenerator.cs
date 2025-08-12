using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Ugen.SourceGenerator.GeneratorConstants;
using static Ugen.SourceGenerator.GeneratorModels;

namespace Ugen.SourceGenerator;

[Generator]
public class UgenBehaviourNodeDataGenerator : IIncrementalGenerator
{
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

    static ClassInfo GetClassInfo(GeneratorAttributeSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.TargetNode;
        var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;
        if (symbol == null) return null;

        // Ugen.Behaviours名前空間でない場合はnull
        if (symbol.ContainingNamespace?.ToDisplayString() != UgenBehavioursNamespace)
            return null;

        return PortInfoExtractor.ExtractClassInfo(symbol);
    }

    static void GenerateSource(SourceProductionContext context, ClassInfo classInfo)
    {
        var source = SourceCodeBuilder.GenerateBehaviourNodeDataSource(classInfo);
        var fileName = $"{classInfo.ClassName}{NodeDataClassSuffix}{GeneratedFileSuffix}";
        context.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
    }
}
