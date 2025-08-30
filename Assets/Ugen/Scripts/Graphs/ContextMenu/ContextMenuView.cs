using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs.ContextMenu
{
    public sealed class ContextMenuView : IDisposable
    {
        readonly CompositeDisposable _disposable = new();
        public VisualElement Root { get; }

        public ContextMenuView(VisualElement container, ContextMenuViewModel viewModel)
        {
            Root = container.Q<VisualElement>("context-menu");
            foreach (var item in viewModel.Items)
            {
                var itemView = new ContextMenuItemView();
                itemView.Bind(item).AddTo(_disposable);
                Root.Add(itemView);
            }

            viewModel.State.Subscribe(state =>
            {
                if (state.IsVisible)
                {
                    ShowMenu(state.Position);
                }
                else
                {
                    HideMenu();
                }
            }).AddTo(_disposable);
        }

        void ShowMenu(Vector2 position)
        {
            Root.style.display = DisplayStyle.Flex;
            Root.style.left = position.x;
            Root.style.top = position.y;
        }

        void HideMenu()
        {
            Root.style.display = DisplayStyle.None;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
