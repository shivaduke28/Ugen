using R3;
using Ugen.Inputs;
using Unity.Cinemachine;
using UnityEngine;

namespace Ugen.Binders
{
    [RequireComponent(typeof(CinemachineSplineDolly))]
    public class UgenCinemachineSplineDollyBinder : MonoBehaviour
    {
        [SerializeField] CinemachineSplineDolly _splineDolly;
        [SerializeField] UgenInputList<float> _speed = new();

        void Start()
        {
            _speed.Observable()
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
