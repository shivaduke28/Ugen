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
        [SerializeField] T _behaviour;
        public override UgenBehaviour Behaviour => _behaviour;
        public override void SetBehaviour(UgenBehaviour behaviour) => _behaviour = (T)behaviour;
    }
}
