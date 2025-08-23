using System.Collections.Generic;
using R3;
using Ugen.Attributes;
using UnityEngine;

namespace Ugen.Binders
{
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("Ugen/Ugen Animator Binder")]
    public sealed class UgenAnimatorBinder : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        [SerializeReference, SerializeReferenceSelector]
        List<AnimationProperty> _properties = new();

        void Start()
        {
            foreach (var property in _properties)
            {
                Bind(property);
            }
        }

        void Bind(AnimationProperty property)
        {
            switch (property)
            {
                case FloatAnimationProperty floatProperty:
                    floatProperty.Binding.AsObservable()
                        .Subscribe(x => _animator.SetFloat(property.Id, x))
                        .AddTo(this);
                    break;
                case IntAnimationProperty intProperty:
                    intProperty.Binding.AsObservable()
                        .Subscribe(x => _animator.SetInteger(property.Id, x))
                        .AddTo(this);
                    break;
                case BoolAnimationProperty boolProperty:
                    boolProperty.Binding.AsObservable()
                        .Subscribe(x => _animator.SetBool(property.Id, x))
                        .AddTo(this);
                    break;
                case TriggerAnimationProperty triggerProperty:
                    triggerProperty.Input.AsObservable()
                        .Subscribe(_ => _animator.SetTrigger(property.Id))
                        .AddTo(this);
                    break;
            }
        }

        void Reset()
        {
            _animator = GetComponent<Animator>();
        }
    }
}
