using System;
using R3;
using Ugen.Graphs.ContextMenu;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs.GraphContextMenu
{
    public sealed class GraphContextMenuView : IDisposable
    {
        readonly VisualElement _root;
        readonly VisualElement _menuContainer;
        readonly CompositeDisposable _disposables = new();

        public GraphContextMenuView(VisualElement root, GraphContextMenuViewModel viewModel)
        {
            _root = root;
            _menuContainer = _root.Q<VisualElement>("context-menu");

            var menuItems = viewModel.GetMenuItems();
            foreach (var item in menuItems)
            {
                var itemView = new ContextMenuItemView();
                itemView.Bind(item).AddTo(_disposables);
                _menuContainer.Add(itemView);
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
            }).AddTo(_disposables);
        }

        void ShowMenu(Vector2 position)
        {
            // メニューを表示
            _root.style.display = DisplayStyle.Flex;
            _root.style.left = position.x;
            _root.style.top = position.y;
        }

        void HideMenu()
        {
            _root.style.display = DisplayStyle.None;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
