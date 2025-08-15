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
        readonly UgenBehaviourNodeData _behaviourNode;
        readonly Button _selectBehaviourButton;
        readonly UgenGraphData _currentGraph;
        readonly BehaviourSearchProvider _searchProvider;

        public UgenBehaviourNodeView(UgenBehaviourNodeData node, UgenGraphData graph) : base(node)
        {
            _behaviourNode = node;
            _currentGraph = graph;

            // Remove the base title and create custom title label
            titleContainer.Clear();

            // Create a button for selecting behaviour
            _selectBehaviourButton = new Button(OpenBehaviourSearch)
            {
                text = GetButtonText(),
            };
            _selectBehaviourButton.AddToClassList("behaviour-select-button");
            _selectBehaviourButton.style.flexGrow = 1;
            _selectBehaviourButton.style.unityTextAlign = TextAnchor.MiddleCenter;

            titleContainer.Add(_selectBehaviourButton);

            // Initialize search provider
            _searchProvider = ScriptableObject.CreateInstance<BehaviourSearchProvider>();

            UpdateTitle();
        }

        void OpenBehaviourSearch()
        {
            if (_currentGraph == null) return;

            // Get the required behaviour type
            var requiredType = GetRequiredBehaviourType();

            // Initialize and open search window
            _searchProvider.Initialize(_currentGraph, requiredType, OnBehaviourSelected);

            var mousePosition = Event.current?.mousePosition ?? Vector2.zero;
            var screenPoint = GUIUtility.GUIToScreenPoint(mousePosition);

            SearchWindow.Open(new SearchWindowContext(screenPoint), _searchProvider);
        }

        Type GetRequiredBehaviourType()
        {
            var nodeType = _behaviourNode.GetType();
            var baseType = nodeType.BaseType;

            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(UgenBehaviourNodeData<>)) return baseType.GetGenericArguments()[0];
                baseType = baseType.BaseType;
            }

            return null;
        }

        void OnBehaviourSelected(UgenBehaviour behaviour)
        {
            _behaviourNode.SetBehaviour(behaviour);
            UpdateTitle();
        }

        string GetButtonText()
        {
            if (_behaviourNode.Behaviour != null)
            {
                var behaviourName = _behaviourNode.Behaviour.gameObject != null
                    ? _behaviourNode.Behaviour.gameObject.name
                    : _behaviourNode.Behaviour.name;
                return $"{_behaviourNode.Name}: {behaviourName}";
            }

            return $"{_behaviourNode.Name}: <None>";
        }

        void UpdateTitle()
        {
            _selectBehaviourButton.text = GetButtonText();

            // Also update the node's visual state
            if (_behaviourNode.Behaviour != null)
                RemoveFromClassList("no-behaviour");
            else
                AddToClassList("no-behaviour");
        }
    }
}
