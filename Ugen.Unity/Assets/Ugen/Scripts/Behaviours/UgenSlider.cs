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
            Debug.Log("initialize ports ugen slider");
            output = new UgenOutput<float>("value", 0, slider.OnValueChangedAsObservable());
            RegisterOutput(output);
        }

        void Reset()
        {
            slider = GetComponent<Slider>();
        }
    }
}
