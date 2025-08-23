using R3;
using Ugen.Bindings;
using UnityEngine;

namespace Ugen.Binders
{
    [RequireComponent(typeof(Light))]
    [AddComponentMenu("Ugen/Ugen Light Binder")]
    public sealed class LightBinder : MonoBehaviour
    {
        [SerializeField] Light _light;
        [SerializeField] FloatBinding _intensity;

        void Reset()
        {
            _light = GetComponent<Light>();
        }

        void Start()
        {
            _intensity?.AsObservable().Subscribe(x => _light.intensity = x).AddTo(this);
        }
    }
}
