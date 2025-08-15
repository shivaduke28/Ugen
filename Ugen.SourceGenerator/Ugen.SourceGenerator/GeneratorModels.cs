using System.Collections.Generic;

namespace Ugen.SourceGenerator;

/// <summary>
/// ソースジェネレーターで使用する共通のモデル定義
/// </summary>
internal static class GeneratorModels
{
    /// <summary>
    /// ポート情報を表す構造体
    /// </summary>
    internal readonly struct PortInfo
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

    /// <summary>
    /// クラス情報を表すクラス
    /// </summary>
    internal sealed class ClassInfo
    {
        public string ClassName { get; set; }
        public List<PortInfo> InputPorts { get; set; }
        public List<PortInfo> OutputPorts { get; set; }
    }
}