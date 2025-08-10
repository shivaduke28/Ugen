using UnityEngine;
using UnityEngine.UI;

namespace Ugen.Behaviours
{
    [RequireComponent(typeof(Slider))]
    public sealed class UgenSlider : UgenBehaviour
    {
        [SerializeField] float defaultValue = 0.5f;

        UgenOutput<float> output;
        Slider slider;

        protected override void InitializePorts()
        {
            output = new UgenOutput<float>("value", defaultValue);
            RegisterOutput(output);
        }

        void Start()
        {
            SetupSlider();
        }

        void SetupSlider()
        {
            slider = GetComponent<Slider>();
            if (slider == null)
            {
                Debug.LogError("Slider component not found!");
                return;
            }

            slider.value = defaultValue;

            slider.onValueChanged.AddListener(OnSliderValueChanged);

            output.SetValue(slider.value);
        }

        void OnSliderValueChanged(float value)
        {
            output.SetValue(value);
        }

        protected override void OnDestroy()
        {
            if (slider != null)
            {
                slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            }
            base.OnDestroy();
        }
    }
}
