using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Ugen.UI.Nodes;
using Ugen.Serialization;

namespace Ugen.Editor.GraphView
{
    public sealed class UgenUIElementNodeView : UgenNodeView
    {
        readonly UgenUIElementNodeData _uiElementNode;
        readonly Button _selectUIElementButton;
        readonly UIElementSearchProvider _searchProvider;
        readonly List<UgenUIElement> _uiElements;

        public UgenUIElementNodeView(UgenUIElementNodeData node, List<UgenUIElement> uiElements) : base(node)
        {
            _uiElementNode = node;
            _uiElements = uiElements;

            // Remove the base title and create custom title label
            titleContainer.Clear();

            // Create a button for selecting UI element
            _selectUIElementButton = new Button(OpenUIElementSearch)
            {
                text = GetButtonText(),
            };
            _selectUIElementButton.AddToClassList("uielement-select-button");
            _selectUIElementButton.style.flexGrow = 1;
            _selectUIElementButton.style.unityTextAlign = TextAnchor.MiddleCenter;

            titleContainer.Add(_selectUIElementButton);

            // Initialize search provider
            _searchProvider = ScriptableObject.CreateInstance<UIElementSearchProvider>();

            UpdateTitle();
        }

        void OpenUIElementSearch()
        {
            // Get the required UI element type
            var requiredType = GetRequiredUIElementType();

            // Initialize and open search window
            _searchProvider.Initialize(_uiElements, requiredType, OnUIElementSelected);

            var mousePosition = Event.current?.mousePosition ?? Vector2.zero;
            var screenPoint = GUIUtility.GUIToScreenPoint(mousePosition);

            SearchWindow.Open(new SearchWindowContext(screenPoint), _searchProvider);
        }

        Type GetRequiredUIElementType()
        {
            var nodeType = _uiElementNode.GetType();
            var baseType = nodeType.BaseType;

            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(UgenUIElementNodeData<>)) return baseType.GetGenericArguments()[0];
                baseType = baseType.BaseType;
            }

            return null;
        }

        void OnUIElementSelected(UgenUIElement uiElement)
        {
            _uiElementNode.SetUIElement(uiElement);
            UpdateTitle();
        }

        string GetButtonText()
        {
            var uiElementName = string.IsNullOrEmpty(_uiElementNode.UIElementName) ? "<None>" : _uiElementNode.UIElementName;
            return $"{_uiElementNode.Name}: {uiElementName}";
        }

        void UpdateTitle()
        {
            _selectUIElementButton.text = GetButtonText();
        }
    }
}
