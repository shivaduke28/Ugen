using System;
using Ugen.Behaviours;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public abstract class UgenBehaviourNodeData : UgenNodeData
    {
        public abstract UgenBehaviour Behaviour { get; }
        public abstract void SetBehaviour(UgenBehaviour behaviour);
    }

    [Serializable]
    public abstract class UgenBehaviourNodeData<T> : UgenBehaviourNodeData where T : UgenBehaviour
    {
        [SerializeField] T behaviour;
        public override UgenBehaviour Behaviour => behaviour;
        public override void SetBehaviour(UgenBehaviour behaviour) => this.behaviour = (T)behaviour;
    }
}
