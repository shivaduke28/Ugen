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
        List<AnimatorProperty> _properties = new();

        void Start()
        {
            foreach (var property in _properties)
            {
                Bind(property);
            }
        }

        void Bind(AnimatorProperty property)
        {
            switch (property)
            {
                case FloatAnimatorProperty floatProperty:
                    floatProperty.Input.AsObservable()
                        .Subscribe(x => _animator.SetFloat(property.Id, x))
                        .AddTo(this);
                    break;
                case IntAnimatorProperty intProperty:
                    intProperty.Input.AsObservable()
                        .Subscribe(x => _animator.SetInteger(property.Id, x))
                        .AddTo(this);
                    break;
                case BoolAnimatorProperty boolProperty:
                    boolProperty.Input.AsObservable()
                        .Subscribe(x => _animator.SetBool(property.Id, x))
                        .AddTo(this);
                    break;
                case TriggerAnimatorProperty triggerProperty:
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
