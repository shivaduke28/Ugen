using Ugen.Behaviours;
using UnityEditor;
using UnityEngine;
using Ugen.Graph;

namespace Ugen.Editor
{
    [CustomEditor(typeof(UgenManager))]
    public class UgenManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Scene Tools", EditorStyles.boldLabel);

            var manager = (UgenManager)target;

            if (GUILayout.Button("Collect Behaviours from Scene"))
            {
                Undo.RecordObject(manager, "Collect Behaviours");
                manager.CollectBehavioursFromScene();
                EditorUtility.SetDirty(manager);
            }

            if (GUILayout.Button("Open Graph Editor"))
            {
                GraphView.UgenGraphWindow.OpenWithManager(manager);
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Clear Graph"))
            {
                if (EditorUtility.DisplayDialog("Clear Graph",
                    "Are you sure you want to clear all nodes and connections?",
                    "Clear", "Cancel"))
                {
                    Undo.RecordObject(manager, "Clear Graph");
                    manager.Graph.ClearNodeAndEdges();
                    EditorUtility.SetDirty(manager);
                }
            }
        }
    }
}
