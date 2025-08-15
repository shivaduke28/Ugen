using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Ugen.Graph;

namespace Ugen.Editor.GraphView
{
    public class UgenGraphWindow : EditorWindow
    {
        UgenGraphView _graphView;
        UgenManager _targetManager;

        public static void OpenWithManager(UgenManager manager)
        {
            var window = GetWindow<UgenGraphWindow>("Ugen Graph Editor");
            window.LoadManager(manager);
            window.Show();
        }

        void CreateGUI()
        {
            // Create vertical container
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Column;
            container.style.flexGrow = 1;
            rootVisualElement.Add(container);

            CreateToolbar(container);
            CreateGraphView(container);
        }

        void CreateToolbar(VisualElement parent)
        {
            var toolbar = new Toolbar();

            var saveButton = new Button(SaveGraph) { text = "Save", };
            toolbar.Add(saveButton);

            var loadButton = new Button(LoadGraph) { text = "Load", };
            toolbar.Add(loadButton);

            toolbar.Add(new ToolbarSpacer());

            var clearButton = new Button(() =>
            {
                if (EditorUtility.DisplayDialog("Clear Graph", "Clear all nodes and connections?", "Clear", "Cancel")) _graphView.ClearGraph();
            }) { text = "Clear", };
            toolbar.Add(clearButton);

            toolbar.Add(new ToolbarSpacer { style = { flexGrow = 1, }, });

            var managerField = new ObjectField("Manager")
            {
                objectType = typeof(UgenManager),
                value = _targetManager,
            };
            managerField.RegisterValueChangedCallback(evt => { _targetManager = evt.newValue as UgenManager; });
            toolbar.Add(managerField);

            var loadManagerButton = new Button(() =>
            {
                if (_targetManager != null) LoadManager(_targetManager);
            }) { text = "Load Manager", };
            toolbar.Add(loadManagerButton);

            parent.Add(toolbar);
        }

        void CreateGraphView(VisualElement parent)
        {
            _graphView = new UgenGraphView();
            _graphView.style.flexGrow = 1;
            parent.Add(_graphView);
        }

        void SaveGraph()
        {
            if (_targetManager == null)
            {
                EditorUtility.DisplayDialog("No Manager", "Please select a UgenManager to save to.", "OK");
                return;
            }

            _targetManager.SaveGraph(_graphView.ExportToGraph());
            EditorUtility.SetDirty(_targetManager);
            Debug.Log("Graph saved to manager");
        }

        void LoadGraph()
        {
            if (_targetManager == null)
            {
                EditorUtility.DisplayDialog("No Manager", "Please select a UgenManager to load from.", "OK");
                return;
            }

            LoadManager(_targetManager);
        }

        void LoadManager(UgenManager manager)
        {
            _targetManager = manager;
            _graphView.LoadFromGraph(manager.GraphData);
        }
    }
}
