using System;
using Ugen.Behaviours;
using UnityEngine;

namespace Ugen.Graph.Nodes
{
    public abstract class UgenBehaviourNode : UgenNode
    {
        public abstract UgenBehaviour Behaviour { get; }
    }


    [Serializable]
    public abstract class UgenBehaviourNode<T> : UgenBehaviourNode where T : UgenBehaviour
    {
        [SerializeField] protected T behaviour;
        public override UgenBehaviour Behaviour => behaviour;

        public void SetBehaviour(T behaviour)
        {
            this.behaviour = behaviour;
        }
    }
}
