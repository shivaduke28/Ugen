using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    [CreateAssetMenu(menuName = "Ugen/Visual Tree Asset Settings")]
    public sealed class VisualTreeAssetSettings : ScriptableObject
    {
        [SerializeField] VisualTreeAsset _node;
        [SerializeField] VisualTreeAsset _inputPort;
        [SerializeField] VisualTreeAsset _outputPort;

        public VisualTreeAsset Node => _node;
        public VisualTreeAsset InputPort => _inputPort;
        public VisualTreeAsset OutputPort => _outputPort;
    }
}
