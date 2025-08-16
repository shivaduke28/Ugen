using Ugen.Attributes;
using Ugen.Graph;
using UnityEngine.UIElements;

namespace Ugen.UI.Nodes
{
    [UxmlElement("UgenSlider"), UgenUIElement("Slider"),]
    public partial class UgenSlider : UgenUIElement
    {
        [UxmlAttribute]
        public string Label
        {
            get => _slider.label;
            set => _slider.label = value;
        }

        [UxmlAttribute]
        public float Value
        {
            get => _slider.value;
            set => _slider.value = value;
        }

        [UxmlAttribute]
        public float LowValue
        {
            get => _slider.lowValue;
            set => _slider.lowValue = value;
        }

        [UxmlAttribute]
        public float HighValue
        {
            get => _slider.highValue;
            set => _slider.highValue = value;
        }

        readonly Slider _slider = new();

        [UgenOutput(0)] readonly UgenOutput<float> _value;

        public UgenSlider()
        {
            _value = new UgenOutput<float>("value", _slider.OnValueChangeAsObservable());
            RegisterOutput(_value);
            Add(_slider);
        }
    }
}
