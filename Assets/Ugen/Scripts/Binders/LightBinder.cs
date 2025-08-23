using R3;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Binders
{
    [RequireComponent(typeof(Light))]
    [AddComponentMenu("Ugen/Ugen Light Binder")]
    public sealed class LightBinder : MonoBehaviour
    {
        [SerializeField] Light _light;
        [SerializeField] UgenInput<float> _intensity;

        void Reset()
        {
            _light = GetComponent<Light>();
        }

        void Start()
        {
            _intensity?.Observable().Subscribe(x => _light.intensity = x).AddTo(this);
        }
    }
}
