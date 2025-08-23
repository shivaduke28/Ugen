using R3;
using Ugen.Inputs;
using Unity.Cinemachine;
using UnityEngine;

namespace Ugen.Binders
{
    [RequireComponent(typeof(CinemachineSplineDolly))]
    [AddComponentMenu("Ugen/Ugen Cinemachine Spline Dolly Binder")]
    public class CinemachineSplineDollyBinder : MonoBehaviour
    {
        [SerializeField] CinemachineSplineDolly _splineDolly;
        [SerializeField] UgenInput<float> _speed;

        void Start()
        {
            _speed.AsObservable()
                .Subscribe(UpdateSpeed)
                .AddTo(this);
        }

        void UpdateSpeed(float x)
        {
            var automaticDolly = _splineDolly.AutomaticDolly;
            var method = automaticDolly.Method;
            if (method is SplineAutoDolly.FixedSpeed fixedSpeed)
            {
                fixedSpeed.Speed = x;
                automaticDolly.Method = fixedSpeed;
                _splineDolly.AutomaticDolly = automaticDolly;
            }
        }

        void Reset()
        {
            _splineDolly = GetComponent<CinemachineSplineDolly>();
        }
    }
}
