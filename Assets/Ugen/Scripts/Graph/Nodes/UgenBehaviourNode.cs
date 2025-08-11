using System;
using UnityEngine;

namespace Ugen.Graph.Nodes
{
    [Serializable]
    public abstract class UgenBehaviourNode : UgenNode
    {
        [SerializeField] string bindingId;

        public string BindingId
        {
            get => bindingId;
            set => bindingId = value;
        }

        public abstract Type BehaviourType { get; }
    }
}
