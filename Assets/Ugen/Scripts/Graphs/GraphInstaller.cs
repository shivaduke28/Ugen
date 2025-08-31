using R3;
using Ugen.Graphs.Nodes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public class GraphInstaller : MonoBehaviour
    {
        [SerializeField] UIDocument _document;
        [SerializeField] VisualTreeAssetSettings _visualTreeAssetSettings;

        GraphViewModel _graphViewModel;
        GraphView _graphView;

        void Start()
        {
            VisualElementFactory.Initialize(_visualTreeAssetSettings);

            // ViewModelを作成
            _graphViewModel = new GraphViewModel();

            // Viewを作成
            _graphView = new GraphView(_document.rootVisualElement);

            // ViewとViewModelをBind
            _graphView.Bind(_graphViewModel).AddTo(this);

            // GraphViewのDisposeを登録
            _graphView.AddTo(this);

            AddTestNodes();
        }

        void AddTestNodes()
        {
            var f1 = _graphViewModel.AddNode(new FloatNode(NodeId.New()), new Vector2(100, 100));
            var f2 = _graphViewModel.AddNode(new FloatNode(NodeId.New()), new Vector2(100, 200));
            var f3 = _graphViewModel.AddNode(new FloatNode(NodeId.New()), new Vector2(100, 300));
            var v3 = _graphViewModel.AddNode(new Vector3Node(NodeId.New()), new Vector2(200, 100));
            var u = _graphViewModel.AddNode(new UpdateNode(NodeId.New()), new Vector2(300, 200));
            var af = _graphViewModel.AddNode(new AddForceNode(NodeId.New()), new Vector2(300, 300));

            _graphViewModel.CreateEdge(f1.Id, 0, v3.Id, 0);
            _graphViewModel.CreateEdge(f2.Id, 0, v3.Id, 1);
            _graphViewModel.CreateEdge(f3.Id, 0, v3.Id, 2);
            _graphViewModel.CreateEdge(u.Id, 0, af.Id, 0);
        }
    }
}
