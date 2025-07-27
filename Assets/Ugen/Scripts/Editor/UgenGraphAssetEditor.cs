using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ugen.Behaviours;
using Ugen.Graphs;

namespace Ugen.Editor
{
    [CustomEditor(typeof(UgenGraphAsset))]
    public sealed class UgenGraphAssetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Load Current Scene"))
            {
                LoadFromCurrentScene();
            }
        }

        void LoadFromCurrentScene()
        {
            var asset = (UgenGraphAsset)target;

            var ugenBehaviours = FindObjectsByType<UgenBehaviour>(FindObjectsSortMode.None);

            if (ugenBehaviours == null || ugenBehaviours.Length == 0)
            {
                EditorUtility.DisplayDialog("Error", "No UgenBehaviour found in current scene.", "OK");
                return;
            }

            // Create graph from all UgenBehaviours
            var graph = CreateGraphFromBehaviours(ugenBehaviours);

            // Set the graph and scene name
            Undo.RecordObject(asset, "Load Graph from Scene");
            asset.SetGraph(graph);
            asset.SetSceneName(SceneManager.GetActiveScene().name);

            EditorUtility.SetDirty(asset);

            Debug.Log($"Loaded {ugenBehaviours.Length} UgenBehaviours from scene '{SceneManager.GetActiveScene().name}'");
        }

        UgenGraph CreateGraphFromBehaviours(UgenBehaviour[] behaviours)
        {
            var behaviourRefs = new UgenBehaviourRef[behaviours.Length];
            for (var i = 0; i < behaviours.Length; i++)
            {
                behaviourRefs[i] = UgenBehaviourConverter.Convert(behaviours[i]);
            }

            return new UgenGraph(behaviourRefs);
        }
    }
}
