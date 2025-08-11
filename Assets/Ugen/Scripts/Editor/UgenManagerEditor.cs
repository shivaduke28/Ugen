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
                    manager.Graph.Clear();
                    EditorUtility.SetDirty(manager);
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Graph Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Nodes: {manager.Graph.Nodes.Count}");
            EditorGUILayout.LabelField($"Connections: {manager.Graph.Edges.Count}");
            EditorGUILayout.LabelField($"Bindings: {manager.Graph.Bindings.Count}");

            if (manager.Graph.Nodes.Count > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Nodes:", EditorStyles.boldLabel);
                foreach (var node in manager.Graph.Nodes)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"  - {node.NodeId} ({node.NodeName})");

                    if (node is Graph.Nodes.UgenBehaviourNode behaviourNode && !string.IsNullOrEmpty(behaviourNode.BindingId))
                    {
                        var behaviour = manager.Graph.GetBoundBehaviourByBindingId(behaviourNode.BindingId);
                        if (behaviour != null)
                        {
                            EditorGUILayout.ObjectField(behaviour, typeof(UgenBehaviour), true);
                        }
                        else
                        {
                            EditorGUILayout.LabelField($"(Binding ID: {behaviourNode.BindingId}, Not found)");
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("(Not bound)");
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (manager.Graph.Edges.Count > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Connections:", EditorStyles.boldLabel);
                foreach (var connection in manager.Graph.Edges)
                {
                    EditorGUILayout.LabelField($"  - {connection.SourceNodeId}[{connection.SourcePortIndex}] → {connection.TargetNodeId}[{connection.TargetPortIndex}]");
                }
            }
        }
    }
}
