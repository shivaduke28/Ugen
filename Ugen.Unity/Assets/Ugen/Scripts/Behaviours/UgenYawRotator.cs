using R3;
using Ugen.Graph;
using UnityEngine;

namespace Ugen.Behaviours
{
    public sealed class UgenYawRotator : UgenBehaviour
    {
        [SerializeField] float speedMultiplier = 30f;

        UgenInput<float> speedInput;
        Transform targetTransform;

        protected override void InitializePorts()
        {
            speedInput = new UgenInput<float>("speed", 0, 1f);
            RegisterInput(speedInput);
        }

        void Start()
        {
            targetTransform = transform;

            Observable.EveryUpdate()
                .WithLatestFrom(speedInput.Observable.Do(x => Debug.Log($"speed:{x}")), (_, s) => s)
                .Subscribe(s =>
                {
                    var rotationSpeed = s * speedMultiplier;
                    targetTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                })
                .AddTo(this);
        }
    }
}
