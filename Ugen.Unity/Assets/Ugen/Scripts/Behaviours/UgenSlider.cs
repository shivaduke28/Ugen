using R3;
using Ugen.Attributes;
using Ugen.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace Ugen.Behaviours
{
    [RequireComponent(typeof(Slider)), UgenBehaviour]
    public sealed class UgenSlider : UgenBehaviour
    {
        [SerializeField] Slider slider;

        [UgenOutput(0)]
        UgenOutput<float> value;

        protected override void InitializePorts()
        {
            Debug.Log("initialize ports ugen slider");
            value = new UgenOutput<float>("value", 0, slider.OnValueChangedAsObservable());
            RegisterOutput(value);
        }

        void Reset()
        {
            slider = GetComponent<Slider>();
        }
    }
}
