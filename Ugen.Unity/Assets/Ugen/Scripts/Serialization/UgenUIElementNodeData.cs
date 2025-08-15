using System;
using Ugen.UI.Nodes;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public abstract class UgenUIElementNodeData : UgenNodeData
    {
        public abstract UgenUIElement UIElement { get; }
        public abstract void SetUIElement(UgenUIElement uiElement);
    }

    [Serializable]
    public abstract class UgenUIElementNodeData<T> : UgenUIElementNodeData where T : UgenUIElement
    {
        [SerializeField] T _uiElement;
        public override UgenUIElement UIElement => _uiElement;
        public override void SetUIElement(UgenUIElement uiElement) => _uiElement = (T)uiElement;
    }
}
