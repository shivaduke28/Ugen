using Ugen.Attributes;
using Ugen.Graph;
using Ugen.UI.Nodes;
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

        [UgenOutput(0)] readonly UgenOutput<float> value;

        public UgenSlider()
        {
            value = new UgenOutput<float>("value", 0, _slider.OnValueChangeAsObservable());
            RegisterOutput(value);
            Add(_slider);
        }
    }
}
