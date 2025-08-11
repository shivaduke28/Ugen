using R3;
using Ugen.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace Ugen.Behaviours
{
    [RequireComponent(typeof(Slider))]
    public sealed class UgenSlider : UgenBehaviour
    {
        [SerializeField] Slider slider;

        UgenOutput<float> output;

        protected override void InitializePorts()
        {
            output = new UgenOutput<float>("value", slider.OnValueChangedAsObservable());
            RegisterOutput(output);
        }

        void Reset()
        {
            slider = GetComponent<Slider>();
        }
    }
}
