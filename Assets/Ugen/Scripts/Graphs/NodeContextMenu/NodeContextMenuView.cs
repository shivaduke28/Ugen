using System;
using R3;
using UnityEngine.UIElements;

namespace Ugen.Graphs.NodeContextMenu
{
    public class NodeContextMenuView : IDisposable
    {
        readonly VisualElement _root;
        readonly Button _deleteButton;
        readonly CompositeDisposable _disposable = new();

        public NodeContextMenuView(VisualElement container, NodeContextMenuViewModel viewModel)
        {
            _root = container.Q<VisualElement>("context-menu");
            _deleteButton = _root.Q<Button>("delete-button");

            _deleteButton.OnClickAsObservable().Subscribe(_ => viewModel.Delete()).AddTo(_disposable);
            viewModel.State.Subscribe(state =>
            {
                if (state.IsVisible)
                {
                    var position = state.Position;
                    _root.style.left = position.x;
                    _root.style.top = position.y;
                    _root.style.display = DisplayStyle.Flex;
                }
                else
                {
                    _root.style.display = DisplayStyle.None;
                }
            });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
