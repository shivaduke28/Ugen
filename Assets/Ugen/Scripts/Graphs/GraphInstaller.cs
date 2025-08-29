using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class GraphInstaller : MonoBehaviour
    {
        [SerializeField] UIDocument _document;
        [SerializeField] VisualTreeAssetSettings _visualTreeAssetSettings;

        void Start()
        {
            VisualElementFactory.Initialize(_visualTreeAssetSettings);
            var graphView = new GraphView(_document.rootVisualElement);

            graphView.AddTo(this);
        }
    }
}
