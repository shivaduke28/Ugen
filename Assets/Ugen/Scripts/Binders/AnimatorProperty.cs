using System;
using R3;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Binders
{
    [Serializable]
    public abstract class AnimatorProperty : ISerializationCallbackReceiver
    {
        int _id = -1;
        [SerializeField] string _property;
        public int Id => _id;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _id = Animator.StringToHash(_property);
        }
    }

    [Serializable]
    public abstract class AnimatorProperty<T> : AnimatorProperty
    {
        public UgenInput<T> Input;
    }

    [Serializable]
    public sealed class FloatAnimatorProperty : AnimatorProperty<float>
    {
    }

    [Serializable]
    public sealed class IntAnimatorProperty : AnimatorProperty<int>
    {
    }

    [Serializable]
    public sealed class BoolAnimatorProperty : AnimatorProperty<bool>
    {
    }

    [Serializable]
    public sealed class TriggerAnimatorProperty : AnimatorProperty<Unit>
    {
    }
}
