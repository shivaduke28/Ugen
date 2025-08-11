using System;
using System.Collections.Generic;
using Ugen.Graph;
using Ugen.Graph.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Ugen.Editor.GraphView
{
    public class SampleSearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        UgenGraphView graphView;
        UgenGraph graph;

        public void Initialize(UgenGraphView graphView, UgenGraph graph)
        {
            this.graphView = graphView;
            this.graph = graph;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && (type.IsSubclassOf(typeof(UgenNode)) || type.IsSubclassOf(typeof(UgenNode))))
                    {
                        entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type });
                    }
                }
            }

            return entries;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (searchTreeEntry.userData is Type type && type.IsSubclassOf(typeof(UgenNode)))
            {
                var node = Activator.CreateInstance(type) as UgenNode;

                UgenNodeView nodeView;
                if (node is UgenBehaviourNode behaviourNode)
                {
                    nodeView = new UgenBehaviourNodeView(behaviourNode, graph);
                }
                else
                {
                    nodeView = new UgenNodeView(node);
                }

                graphView.AddElement(nodeView);
                return true;
            }

            return false;
        }
    }
}
