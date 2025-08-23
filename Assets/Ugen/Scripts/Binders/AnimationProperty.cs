using System;
using R3;
using Ugen.Bindings;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Binders
{
    [Serializable]
    public abstract class AnimationProperty : ISerializationCallbackReceiver
    {
        [SerializeField] string _property;
        public int Id { get; private set; } = -1;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            Id = Animator.StringToHash(_property);
        }
    }

    [Serializable]
    public sealed class FloatAnimationProperty : AnimationProperty
    {
        public FloatBinding Binding;
    }

    [Serializable]
    public sealed class IntAnimationProperty : AnimationProperty
    {
        public IntBinding Binding;
    }

    [Serializable]
    public sealed class BoolAnimationProperty : AnimationProperty
    {
        public BoolBinding Binding;
    }

    [Serializable]
    public sealed class TriggerAnimationProperty : AnimationProperty
    {
        public UgenInput<Unit> Input;
    }
}
