using System;
using System.Collections.Generic;
using R3;
using UnityEngine.UIElements;

namespace Ugen.Graphs.EdgeContextMenu
{
    public sealed class EdgeContextMenuView : IDisposable
    {
        readonly VisualElement _root;
        readonly VisualElement _menuContainer;
        readonly EdgeContextMenuViewModel _viewModel;
        readonly CompositeDisposable _disposable = new();
        readonly List<Button> _menuButtons = new();

        public EdgeContextMenuView(VisualElement container, EdgeContextMenuViewModel viewModel)
        {
            _root = container;
            _viewModel = viewModel;
            _menuContainer = _root.Q<VisualElement>("context-menu");

            _viewModel.State.Subscribe(state =>
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

        void ShowMenu(UnityEngine.Vector2 position)
        {
            // 既存のボタンをクリア
            ClearMenuItems();

            // ViewModelからメニュー項目を取得して動的に生成
            var menuItems = _viewModel.GetMenuItems();
            foreach (var item in menuItems)
            {
                var state = item.State.CurrentValue;
                var button = new Button(() =>
                {
                    state.OnClick?.Invoke();
                })
                {
                    text = state.Label
                };
                button.AddToClassList("ugen-context-menu-item");

                if (!state.IsEnabled)
                {
                    button.SetEnabled(false);
                }

                _menuContainer.Add(button);
                _menuButtons.Add(button);
            }

            // メニューを表示
            _root.style.display = DisplayStyle.Flex;
            _root.style.left = position.x;
            _root.style.top = position.y;
        }

        void HideMenu()
        {
            _root.style.display = DisplayStyle.None;
            ClearMenuItems();
        }

        void ClearMenuItems()
        {
            foreach (var button in _menuButtons)
            {
                _menuContainer.Remove(button);
            }
            _menuButtons.Clear();
        }

        public void Dispose()
        {
            ClearMenuItems();
            _disposable.Dispose();
        }
    }
}
