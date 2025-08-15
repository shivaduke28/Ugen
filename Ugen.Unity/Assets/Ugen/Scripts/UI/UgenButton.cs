using System;
using R3;
using UnityEngine.UIElements;

namespace Ugen.UI
{
    [UxmlElement("UgenButton")]
    public partial class UgenButton : VisualElement
    {
        [UxmlAttribute]
        public string Text
        {
            get => _button.text;
            set => _button.text = value;
        }

        readonly Button _button = new();
        const string UssClassName = "ugen-button";
        const string UssClassNameFocused = "ugen-button--focused";
        const string UssClassNameHighlighted = "ugen-button--highlighted";

        public UgenButton()
        {
            AddToClassList(UssClassName);
            AddToClassList(UssClassNameFocused);
            AddToClassList(UssClassNameHighlighted);

            EnableInClassList(UssClassNameFocused, false);
            EnableInClassList(UssClassNameHighlighted, false);

            Add(_button);
        }

        public IDisposable Bind(UgenButtonState state)
        {
            return new CompositeDisposable(
                Observable.FromEvent(h => _button.clicked += h, h => _button.clicked -= h).Subscribe(_ => state.OnClick()),
                state.IsFocused.Subscribe(x => EnableInClassList(UssClassNameFocused, x)),
                state.IsHighlighted.Subscribe(x => EnableInClassList(UssClassNameHighlighted, x)),
                state.Text.Subscribe(x => _button.text = x));
        }
    }
}
