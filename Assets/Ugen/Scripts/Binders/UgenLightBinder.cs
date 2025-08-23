using System.Collections.Generic;
using R3;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Binders
{
    [RequireComponent(typeof(Light))]
    public sealed class UgenLightBinder : MonoBehaviour
    {
        [SerializeField] Light _light;
        [SerializeField] UgenInputList<float> _intensity = new();

        void Reset()
        {
            _light = GetComponent<Light>();
        }

        void Start()
        {
            _intensity.Observable().Subscribe(x => _light.intensity = x).AddTo(this);
        }
    }
}
