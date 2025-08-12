using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Ugen.SourceGenerator;

[Generator]
public class ExampleGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 対象のクラス宣言を収集する
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                // 対象のシンタックスノードをフィルタリングする
                static (s, _) =>
                {
                    // クラス宣言だけを対象とする
                    if (s is not ClassDeclarationSyntax classDeclaration)
                        return false;

                    // Namespaceが定義されているクラスだけを対象とする
                    if (classDeclaration.Parent is not NamespaceDeclarationSyntax &&
                        classDeclaration.Parent is not FileScopedNamespaceDeclarationSyntax)
                        return false;

                    // ExampleあるいはExampleAttributeがついているクラスのみを対象とする
                    return classDeclaration.AttributeLists
                        .SelectMany(x => x.Attributes)
                        .Any(x =>
                        {
                            var name = x.Name.ToString();
                            return name is "Example" or "ExampleAttribute";
                        });
                },
                // フィルタリングされたノードから必要な情報を抽出する
                static (ctx, _) => (ClassDeclarationSyntax)ctx.Node);

        // 収集したクラス宣言に対してコードを生成する
        context.RegisterSourceOutput(classDeclarations,
            static (spc, classDeclaration) => { GenerateSource(spc, classDeclaration!); });
    }

    static void GenerateSource(SourceProductionContext context, ClassDeclarationSyntax classDeclaration)
    {
        var className = classDeclaration.Identifier.Text;
        var namespaceName = ((NamespaceDeclarationSyntax)classDeclaration.Parent!).Name.ToString();

        // 「Hello, World!」という文字列をプロパティとして持つ部分クラスを生成する
        var source = $$"""
                       using System;
                       namespace {{namespaceName}}
                       {
                           public partial class {{className}}
                           {
                               public string GeneratedProperty { get; set; } = "Hello, World!";
                           }
                       }
                       """;
        var fileName = $"{className}.g.cs";
        context.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
    }
}
