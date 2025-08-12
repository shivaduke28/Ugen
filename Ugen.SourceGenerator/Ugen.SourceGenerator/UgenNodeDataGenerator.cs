using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Ugen.SourceGenerator.GeneratorConstants;
using static Ugen.SourceGenerator.GeneratorModels;

namespace Ugen.SourceGenerator;

[Generator]
public class UgenNodeDataGenerator : IIncrementalGenerator
{
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
        var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;
        if (symbol == null) return null;

        return PortInfoExtractor.ExtractClassInfo(symbol);
    }

    static void GenerateSource(SourceProductionContext context, ClassInfo classInfo)
    {
        var source = SourceCodeBuilder.GenerateNodeDataSource(classInfo);
        var fileName = $"{classInfo.ClassName}{DataClassSuffix}{GeneratedFileSuffix}";
        context.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
    }
}