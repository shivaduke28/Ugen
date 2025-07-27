using UnityEngine;

namespace Ugen.Graphs
{
    [CreateAssetMenu(fileName = "UgenGraphAsset", menuName = "Ugen/Graph Asset")]
    public sealed class UgenGraphAsset : ScriptableObject
    {
        [SerializeField] UgenGraph graph;
        [SerializeField] string sceneName;

        public UgenGraph Graph => graph;
        public string SceneName => sceneName;

        public void SetGraph(UgenGraph newGraph)
        {
            graph = newGraph;
        }

        public void SetSceneName(string name)
        {
            sceneName = name;
        }
    }
}