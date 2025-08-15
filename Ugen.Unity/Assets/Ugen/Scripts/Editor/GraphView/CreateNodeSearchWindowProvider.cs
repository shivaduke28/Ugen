using System;
using System.Collections.Generic;
using Ugen.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Ugen.Editor.GraphView
{
    public class CreateNodeSearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        UgenGraphView _graphView;
        UgenGraphData _graph;

        public void Initialize(UgenGraphView graphView, UgenGraphData graph)
        {
            _graphView = graphView;
            _graph = graph;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry> { new SearchTreeGroupEntry(new GUIContent("Create Node")), };

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(UgenNodeData)))
                    entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type, });
            }

            return entries;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (searchTreeEntry.userData is Type type && type.IsSubclassOf(typeof(UgenNodeData)))
            {
                var node = Activator.CreateInstance(type) as UgenNodeData;

                UgenNodeView nodeView;
                if (node is UgenBehaviourNodeData behaviourNode)
                {
                    nodeView = new UgenBehaviourNodeView(behaviourNode, _graph);
                }
                else
                {
                    nodeView = new UgenNodeView(node);
                }

                _graphView.AddNodeView(nodeView);
                return true;
            }

            return false;
        }
    }
}
