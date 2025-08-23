using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Ugen.Inputs.UI
{
    [RequireComponent(typeof(Slider))]
    public sealed class UgenSliderInput : UgenInput<float>
    {
        [SerializeField] Slider _slider;
        public override Observable<float> AsObservable() => _slider.OnValueChangedAsObservable();

        void Reset()
        {
            _slider = GetComponent<Slider>();
        }
    }
}
