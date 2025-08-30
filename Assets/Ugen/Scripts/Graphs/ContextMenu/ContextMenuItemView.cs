using System;
using R3;
using UnityEngine.UIElements;

namespace Ugen.Graphs.ContextMenu
{
    [UxmlElement("context-menu-item")]
    public sealed partial class ContextMenuItemView : VisualElement
    {
        readonly Button _button;
        public ContextMenuItemView()
        {
            _button = new Button();
            Add(_button);
            _button.AddToClassList("ugen-context-menu-item");
        }

        public IDisposable Bind(ContextMenuItemViewModel viewModel)
        {
            return new CompositeDisposable(
                _button.OnClickAsObservable().Subscribe(_ => viewModel.State.CurrentValue.OnClick.Invoke()),
                viewModel.State.Subscribe(x =>
                {
                    _button.text = x.Label;
                    _button.SetEnabled(x.IsEnabled);
                })
            );
        }
    }
}
