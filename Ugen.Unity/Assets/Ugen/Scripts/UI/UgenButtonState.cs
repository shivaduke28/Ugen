using System;
using R3;

namespace Ugen.UI
{
    public sealed class UgenButtonState
    {
        public ReactiveProperty<string> Text { get; } = new("");
        public ReactiveProperty<bool> IsFocused { get; } = new(false);
        public ReactiveProperty<bool> IsHighlighted { get; } = new(false);

        public readonly Action OnClick;

        public UgenButtonState(string text, Action onClick)
        {
            Text.Value = text;
            OnClick = onClick;
        }


        public UgenButtonState(Action onClick)
        {
            OnClick = onClick;
        }
    }
}
