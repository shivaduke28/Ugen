using R3;
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

            // テストデータを追加
            _graphViewModel.AddTestData();

            // GraphViewのDisposeを登録
            _graphView.AddTo(this);
        }
    }
}
