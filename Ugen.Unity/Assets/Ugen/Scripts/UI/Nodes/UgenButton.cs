using R3;
using Ugen.Attributes;
using Ugen.Graph;
using UnityEngine.UIElements;

namespace Ugen.UI.Nodes
{
    [UxmlElement, UgenUIElement("Button")]
    public partial class UgenButton : UgenUIElement
    {
        [UgenOutput(0, "clicked")] readonly UgenOutput<Unit> _clicked;

        [UxmlAttribute]
        public string Text
        {
            get => _button.text;
            set => _button.text = value;
        }

        readonly Button _button = new();

        public UgenButton()
        {
            _clicked = new UgenOutput<Unit>("clicked", 0, _button.OnClickAsObservable());
            RegisterOutput(_clicked);
            Add(_button);
        }
    }
}
