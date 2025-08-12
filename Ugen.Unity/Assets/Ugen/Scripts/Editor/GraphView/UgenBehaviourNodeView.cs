using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Ugen.Behaviours;
using Ugen.Serialization;

namespace Ugen.Editor.GraphView
{
    public sealed class UgenBehaviourNodeView : UgenNodeView
    {
        readonly UgenBehaviourNodeData behaviourNode;
        readonly Button selectBehaviourButton;
        readonly UgenGraphData currentGraph;
        readonly BehaviourSearchProvider searchProvider;

        public UgenBehaviourNodeView(UgenBehaviourNodeData node, UgenGraphData graph) : base(node)
        {
            behaviourNode = node;
            currentGraph = graph;

            // Remove the base title and create custom title label
            titleContainer.Clear();

            // Create a button for selecting behaviour
            selectBehaviourButton = new Button(OpenBehaviourSearch)
            {
                text = GetButtonText()
            };
            selectBehaviourButton.AddToClassList("behaviour-select-button");
            selectBehaviourButton.style.flexGrow = 1;
            selectBehaviourButton.style.unityTextAlign = TextAnchor.MiddleCenter;

            titleContainer.Add(selectBehaviourButton);

            // Initialize search provider
            searchProvider = ScriptableObject.CreateInstance<BehaviourSearchProvider>();

            UpdateTitle();
        }

        void OpenBehaviourSearch()
        {
            if (currentGraph == null) return;

            // Get the required behaviour type
            var requiredType = GetRequiredBehaviourType();

            // Initialize and open search window
            searchProvider.Initialize(currentGraph, requiredType, OnBehaviourSelected);

            var mousePosition = Event.current?.mousePosition ?? Vector2.zero;
            var screenPoint = GUIUtility.GUIToScreenPoint(mousePosition);

            SearchWindow.Open(new SearchWindowContext(screenPoint), searchProvider);
        }

        Type GetRequiredBehaviourType()
        {
            var nodeType = behaviourNode.GetType();
            var baseType = nodeType.BaseType;

            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(UgenBehaviourNodeData<>))
                {
                    return baseType.GetGenericArguments()[0];
                }
                baseType = baseType.BaseType;
            }

            return null;
        }

        void OnBehaviourSelected(UgenBehaviour behaviour)
        {
            behaviourNode.SetBehaviour(behaviour);
            UpdateTitle();
        }

        string GetButtonText()
        {
            if (behaviourNode.Behaviour != null)
            {
                var behaviourName = behaviourNode.Behaviour.gameObject != null
                    ? behaviourNode.Behaviour.gameObject.name
                    : behaviourNode.Behaviour.name;
                return $"{behaviourNode.Name}: {behaviourName}";
            }
            return $"{behaviourNode.Name}: <None>";
        }

        void UpdateTitle()
        {
            selectBehaviourButton.text = GetButtonText();

            // Also update the node's visual state
            if (behaviourNode.Behaviour != null)
            {
                RemoveFromClassList("no-behaviour");
            }
            else
            {
                AddToClassList("no-behaviour");
            }
        }
    }
}
