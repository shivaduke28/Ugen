using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using static Ugen.SourceGenerator.GeneratorConstants;
using static Ugen.SourceGenerator.GeneratorModels;

namespace Ugen.SourceGenerator;

/// <summary>
/// ポート情報の抽出に関する責務を持つヘルパークラス
/// </summary>
internal static class PortInfoExtractor
{
    /// <summary>
    /// INamedTypeSymbolからクラス情報を抽出する
    /// </summary>
    internal static ClassInfo ExtractClassInfo(INamedTypeSymbol symbol)
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

    /// <summary>
    /// フィールドからポート情報を抽出する
    /// </summary>
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

    /// <summary>
    /// 型がUgenInputかどうかをチェック
    /// </summary>
    internal static bool IsUgenInput(INamedTypeSymbol type)
    {
        return type.Name.Contains("Input") &&
               type.ContainingNamespace.ToDisplayString() == UgenGraphNamespace &&
               type.IsGenericType;
    }

    /// <summary>
    /// 型がUgenOutputかどうかをチェック
    /// </summary>
    internal static bool IsUgenOutput(INamedTypeSymbol type)
    {
        return type.Name.Contains("Output") &&
               type.ContainingNamespace.ToDisplayString() == UgenGraphNamespace &&
               type.IsGenericType;
    }
}
