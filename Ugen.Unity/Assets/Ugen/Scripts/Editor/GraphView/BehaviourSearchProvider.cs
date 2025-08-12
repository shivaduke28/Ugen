using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Ugen.Behaviours;
using Ugen.Graph;
using Ugen.Graph.Nodes;
using Ugen.Serialization;

namespace Ugen.Editor.GraphView
{
    public sealed class BehaviourSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        UgenGraphData graph;
        Type requiredBehaviourType;
        Action<UgenBehaviour> onBehaviourSelected;

        public void Initialize(UgenGraphData graph, Type requiredType, Action<UgenBehaviour> callback)
        {
            this.graph = graph;
            requiredBehaviourType = requiredType;
            onBehaviourSelected = callback;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Select Behaviour")));

            // Add "None" option
            entries.Add(new SearchTreeEntry(new GUIContent("None"))
            {
                level = 1,
                userData = null
            });

            if (graph == null || graph.Behaviours == null)
                return entries;

            // Group behaviours by type
            var behavioursByType = new Dictionary<Type, List<UgenBehaviour>>();

            foreach (var behaviour in graph.Behaviours)
            {
                if (behaviour == null) continue;

                // Check if behaviour matches the required type
                if (requiredBehaviourType == null || requiredBehaviourType.IsAssignableFrom(behaviour.GetType()))
                {
                    var type = behaviour.GetType();
                    if (!behavioursByType.ContainsKey(type))
                    {
                        behavioursByType[type] = new List<UgenBehaviour>();
                    }
                    behavioursByType[type].Add(behaviour);
                }
            }

            // Add behaviours grouped by type
            foreach (var kvp in behavioursByType)
            {
                // Add type group
                entries.Add(new SearchTreeGroupEntry(new GUIContent(kvp.Key.Name))
                {
                    level = 1
                });

                // Add behaviours of this type
                foreach (var behaviour in kvp.Value)
                {
                    var displayName = behaviour.gameObject != null
                        ? behaviour.gameObject.name
                        : behaviour.name;

                    entries.Add(new SearchTreeEntry(new GUIContent(displayName))
                    {
                        level = 2,
                        userData = behaviour
                    });
                }
            }

            return entries;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var behaviour = searchTreeEntry.userData as UgenBehaviour;
            onBehaviourSelected?.Invoke(behaviour);
            return true;
        }
    }
}
