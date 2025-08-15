using System;
using Ugen.UI.Nodes;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public abstract class UgenUIElementNodeData : UgenNodeData
    {
        [SerializeField] protected string _uiElementName;
        public string UIElementName => _uiElementName;
        public abstract void SetUIElement(UgenUIElement uiElement);
    }

    [Serializable]
    public abstract class UgenUIElementNodeData<T> : UgenUIElementNodeData where T : UgenUIElement
    {
        public override void SetUIElement(UgenUIElement uiElement)
        {
            _uiElementName = uiElement?.name;
        }
    }
}
