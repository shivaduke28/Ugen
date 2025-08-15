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

        [UgenInput(0, "speed")] UgenInputProperty<float> _speedInput;
        Transform _targetTransform;

        protected override void InitializePorts()
        {
            _speedInput = new UgenInputProperty<float>("speed", 1f);
            RegisterInput(_speedInput);
        }

        void Start()
        {
            _targetTransform = transform;

            Observable.EveryUpdate()
                .WithLatestFrom(_speedInput.Observable, (_, s) => s)
                .Subscribe(s =>
                {
                    var rotationSpeed = s * _speedMultiplier;
                    _targetTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                })
                .AddTo(this);
        }
    }
}
