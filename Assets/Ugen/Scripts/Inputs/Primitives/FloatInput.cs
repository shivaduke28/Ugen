using R3;
using UnityEngine;

namespace Ugen.Inputs.Primitives
{
    [AddComponentMenu("Ugen/Ugen Float Input")]
    public sealed class FloatInput : UgenInput<float>
    {
        [SerializeField] SerializableReactiveProperty<float> _value = new();
        [SerializeField] UgenInput<float> _input;

        void Start()
        {
            if (_input != null)
            {
                _input.AsObservable().Subscribe(x => _value.OnNext(x));
            }
        }

        public override Observable<float> AsObservable() => _value;
    }
}
