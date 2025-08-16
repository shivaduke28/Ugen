using R3;
using Ugen.Attributes;
using Ugen.Graph;
using UnityEngine;

namespace Ugen.Behaviours
{
    [UgenBehaviour]
    public sealed class UgenYawRotator : UgenBehaviour
    {
        [SerializeField] float _speedMultiplier = 30f;

        [SerializeField, UgenInput(0, "speed")]
        SerializableUgenInputProperty<float> _speed = new(1f);

        Transform _targetTransform;

        protected override void InitializePorts()
        {
            RegisterInput(_speed);
        }

        void Start()
        {
            _targetTransform = transform;

            Observable.EveryUpdate()
                .WithLatestFrom(_speed.Observable, (_, s) => s)
                .Subscribe(s =>
                {
                    var rotationSpeed = s * _speedMultiplier;
                    _targetTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                })
                .AddTo(this);
        }
    }
}
