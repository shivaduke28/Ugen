using System;
using Ugen.Behaviours;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public abstract class UgenBehaviourNodeData<T> : UgenNodeData where T : UgenBehaviour
    {
        [SerializeField] T behaviour;
        public T Behaviour => behaviour;
    }
}
