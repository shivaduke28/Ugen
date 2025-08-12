using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Ugen.Graph;

namespace Ugen.Editor.GraphView
{
    public class UgenGraphWindow : EditorWindow
    {
        UgenGraphView graphView;
        UgenManager targetManager;

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

            var saveButton = new Button(SaveGraph) { text = "Save" };
            toolbar.Add(saveButton);

            var loadButton = new Button(LoadGraph) { text = "Load" };
            toolbar.Add(loadButton);

            toolbar.Add(new ToolbarSpacer());

            var clearButton = new Button(() =>
            {
                if (EditorUtility.DisplayDialog("Clear Graph", "Clear all nodes and connections?", "Clear", "Cancel"))
                {
                    graphView.ClearGraph();
                }
            }) { text = "Clear" };
            toolbar.Add(clearButton);

            toolbar.Add(new ToolbarSpacer() { style = { flexGrow = 1 } });

            var managerField = new ObjectField("Manager")
            {
                objectType = typeof(UgenManager),
                value = targetManager
            };
            managerField.RegisterValueChangedCallback(evt =>
            {
                targetManager = evt.newValue as UgenManager;
            });
            toolbar.Add(managerField);

            var loadManagerButton = new Button(() =>
            {
                if (targetManager != null)
                {
                    LoadManager(targetManager);
                }
            }) { text = "Load Manager" };
            toolbar.Add(loadManagerButton);

            parent.Add(toolbar);
        }

        void CreateGraphView(VisualElement parent)
        {
            graphView = new UgenGraphView();
            graphView.style.flexGrow = 1;
            parent.Add(graphView);
        }

        void SaveGraph()
        {
            if (targetManager == null)
            {
                EditorUtility.DisplayDialog("No Manager", "Please select a UgenManager to save to.", "OK");
                return;
            }

            graphView.SaveToGraph(targetManager.Graph);
            EditorUtility.SetDirty(targetManager);
            Debug.Log("Graph saved to manager");
        }

        void LoadGraph()
        {
            if (targetManager == null)
            {
                EditorUtility.DisplayDialog("No Manager", "Please select a UgenManager to load from.", "OK");
                return;
            }

            LoadManager(targetManager);
        }

        void LoadManager(UgenManager manager)
        {
            targetManager = manager;
            graphView.LoadFromGraph(manager.Graph);
            Debug.Log($"Loaded graph with {manager.Graph.Nodes.Count} nodes");
        }
    }
}
