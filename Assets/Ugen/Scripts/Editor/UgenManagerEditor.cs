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

            EditorGUILayout.Space();

            if (GUILayout.Button("Clear Graph"))
            {
                if (EditorUtility.DisplayDialog("Clear Graph",
                    "Are you sure you want to clear all nodes and connections?",
                    "Clear", "Cancel"))
                {
                    Undo.RecordObject(manager, "Clear Graph");
                    manager.Graph.Clear();
                    EditorUtility.SetDirty(manager);
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Graph Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Nodes: {manager.Graph.Nodes.Count}");
            EditorGUILayout.LabelField($"Connections: {manager.Graph.Connections.Count}");
            EditorGUILayout.LabelField($"Bindings: {manager.Graph.Bindings.Count}");

            if (manager.Graph.Nodes.Count > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Nodes:", EditorStyles.boldLabel);
                foreach (var node in manager.Graph.Nodes)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"  - {node.NodeId} ({node.NodeName})");

                    var binding = manager.Graph.GetBoundBehaviour(node.NodeId);
                    if (binding != null)
                    {
                        EditorGUILayout.ObjectField(binding, typeof(UgenBehaviour), true);
                    }
                    else
                    {
                        EditorGUILayout.LabelField("(Not bound)");
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (manager.Graph.Connections.Count > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Connections:", EditorStyles.boldLabel);
                foreach (var connection in manager.Graph.Connections)
                {
                    EditorGUILayout.LabelField($"  - {connection.SourceNodeId}[{connection.SourcePortIndex}] → {connection.TargetNodeId}[{connection.TargetPortIndex}]");
                }
            }
        }
    }
}
