using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Ugen.Behaviours;
using Ugen.Graph;
using Ugen.Graph.Nodes;

namespace Ugen.Editor.GraphView
{
    public sealed class UgenBehaviourNodeView : UgenNodeView
    {
        readonly UgenBehaviourNode behaviourNode;
        readonly Button selectBehaviourButton;
        readonly UgenGraph currentGraph;
        readonly BehaviourSearchProvider searchProvider;

        public UgenBehaviourNodeView(UgenBehaviourNode node, UgenGraph graph) : base(node)
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
            searchProvider.Initialize(behaviourNode, currentGraph, requiredType, OnBehaviourSelected);

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
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(UgenBehaviourNode<>))
                {
                    return baseType.GetGenericArguments()[0];
                }
                baseType = baseType.BaseType;
            }

            return null;
        }

        void OnBehaviourSelected(UgenBehaviour behaviour)
        {
            SetBehaviourInternal(behaviour);
        }

        void SetBehaviourInternal(UgenBehaviour behaviour)
        {
            // Use reflection to call the SetBehaviour method with the correct type
            var nodeType = behaviourNode.GetType();
            var baseType = nodeType.BaseType;

            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(UgenBehaviourNode<>))
                {
                    var setBehaviourMethod = nodeType.GetMethod("SetBehaviour");
                    if (setBehaviourMethod != null)
                    {
                        setBehaviourMethod.Invoke(behaviourNode, new object[] { behaviour });
                        UpdateTitle();
                        return;
                    }
                    break;
                }
                baseType = baseType.BaseType;
            }
        }

        string GetButtonText()
        {
            if (behaviourNode.Behaviour != null)
            {
                var behaviourName = behaviourNode.Behaviour.gameObject != null
                    ? behaviourNode.Behaviour.gameObject.name
                    : behaviourNode.Behaviour.name;
                return $"{behaviourNode.NodeName}: {behaviourName}";
            }
            return $"{behaviourNode.NodeName}: <None>";
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
