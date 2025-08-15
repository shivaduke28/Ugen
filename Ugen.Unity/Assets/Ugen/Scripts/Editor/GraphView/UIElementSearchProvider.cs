using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Ugen.UI.Nodes;
using Ugen.Serialization;

namespace Ugen.Editor.GraphView
{
    public sealed class UIElementSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        UgenGraphData _graph;
        Type _requiredUIElementType;
        Action<UgenUIElement> _onUIElementSelected;
        List<UgenUIElement> _uiElements;

        public void Initialize(List<UgenUIElement> uiElements, Type requiredType, Action<UgenUIElement> callback)
        {
            _uiElements = uiElements;
            _requiredUIElementType = requiredType;
            _onUIElementSelected = callback;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Select UI Element")));

            // Add "None" option
            entries.Add(new SearchTreeEntry(new GUIContent("None"))
            {
                level = 1,
                userData = null,
            });

            // Group UI elements by type
            var uiElementsByType = new Dictionary<Type, List<UgenUIElement>>();

            foreach (var uiElement in _uiElements)
            {
                if (uiElement == null) continue;

                // Check if UI element matches the required type
                if (_requiredUIElementType == null || _requiredUIElementType.IsAssignableFrom(uiElement.GetType()))
                {
                    var type = uiElement.GetType();
                    if (!uiElementsByType.ContainsKey(type)) uiElementsByType[type] = new List<UgenUIElement>();
                    uiElementsByType[type].Add(uiElement);
                }
            }

            // Add UI elements grouped by type
            foreach (var kvp in uiElementsByType)
            {
                // Add type group
                entries.Add(new SearchTreeGroupEntry(new GUIContent(kvp.Key.Name))
                {
                    level = 1,
                });

                // Add UI elements of this type
                foreach (var uiElement in kvp.Value)
                {
                    entries.Add(new SearchTreeEntry(new GUIContent(uiElement.name))
                    {
                        level = 2,
                        userData = uiElement,
                    });
                }
            }

            return entries;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var uiElement = searchTreeEntry.userData as UgenUIElement;
            _onUIElementSelected?.Invoke(uiElement);
            return true;
        }
    }
}
