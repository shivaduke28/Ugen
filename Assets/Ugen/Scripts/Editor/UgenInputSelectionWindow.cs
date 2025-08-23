using System;
using System.Collections.Generic;
using System.Linq;
using Ugen.Inputs;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Editor
{
    public class UgenInputSelectionWindow : EditorWindow
    {
        struct InputEntry
        {
            public UgenInput Input;
            public GameObject GameObject;
            public string DisplayName;
            public Type GenericType;
        }

        Action<UgenInput> _onSelectionCallback;
        readonly List<InputEntry> _allInputEntries = new();

        // UIElements
        VisualElement _root;
        ToolbarSearchField _searchField;
        Button _refreshButton;
        Button _noneButton;
        ScrollView _scrollView;
        VisualElement _listContainer;

        public static void ShowWindow(Action<UgenInput> onSelection)
        {
            var window = GetWindow<UgenInputSelectionWindow>(true, "Select UgenInput", true);
            window.minSize = new Vector2(400, 500);
            window.Initialize(onSelection);
            window.ShowUtility();
        }

        void Initialize(Action<UgenInput> onSelection)
        {
            _onSelectionCallback = onSelection;
            RefreshInputList();
        }

        void CreateGUI()
        {
            // Load UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/Ugen/Scripts/Editor/UgenInputSelectionWindow.uxml");

            if (visualTree == null)
            {
                Debug.LogError("Failed to load UgenInputSelectionWindow.uxml");
                return;
            }

            visualTree.CloneTree(rootVisualElement);

            // Load USS
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                "Assets/Ugen/Scripts/Editor/UgenInputSelectionWindow.uss");

            if (styleSheet != null)
            {
                rootVisualElement.styleSheets.Add(styleSheet);
            }

            // Get references
            _root = rootVisualElement.Q<VisualElement>("root");
            _searchField = rootVisualElement.Q<ToolbarSearchField>("search-field");
            _refreshButton = rootVisualElement.Q<Button>("refresh-button");
            _noneButton = rootVisualElement.Q<Button>("none-button");
            _scrollView = rootVisualElement.Q<ScrollView>("scroll-view");
            _listContainer = rootVisualElement.Q<VisualElement>("list-container");

            // Setup event handlers
            SetupEventHandlers();

            // Initial refresh
            RefreshInputList();
            UpdateInputList();
        }

        void SetupEventHandlers()
        {
            _refreshButton.clicked += () =>
            {
                RefreshInputList();
                UpdateInputList();
            };

            _noneButton.clicked += () =>
            {
                _onSelectionCallback?.Invoke(null);
                Close();
            };

            _searchField.RegisterValueChangedCallback(evt => { UpdateInputList(); });
        }

        void RefreshInputList()
        {
            _allInputEntries.Clear();

            // シーン内のすべてのUgenInputを検索
            var allInputs = FindObjectsByType<UgenInput>(FindObjectsSortMode.None);

            // 各Inputに対してエントリを作成
            foreach (var input in allInputs)
            {
                var genericType = GetGenericArgumentType(input.GetType());
                var typeName = genericType != null ? GetFriendlyTypeName(genericType) : "Unknown";
                var componentName = input.GetType().Name;

                // GameObjectに複数のUgenInputがある場合を考慮した表示名
                var displayName = $"{input.gameObject.name} ({componentName} - {typeName})";

                _allInputEntries.Add(new InputEntry
                {
                    Input = input,
                    GameObject = input.gameObject,
                    DisplayName = displayName,
                    GenericType = genericType
                });
            }

            // GameObject名、次にComponent名でソート
            _allInputEntries.Sort((a, b) =>
            {
                var gameObjectCompare = string.Compare(a.GameObject.name, b.GameObject.name, StringComparison.Ordinal);
                if (gameObjectCompare != 0) return gameObjectCompare;

                return string.Compare(a.DisplayName, b.DisplayName, StringComparison.Ordinal);
            });
        }

        void UpdateInputList()
        {
            _listContainer.Clear();

            var searchString = _searchField.value;

            // フィルタリングされたエントリ一覧
            var filteredEntries = string.IsNullOrEmpty(searchString)
                ? _allInputEntries
                : _allInputEntries.Where(entry =>
                    entry.DisplayName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            if (filteredEntries.Count == 0)
            {
                var infoLabel = new Label("No UgenInput found");
                infoLabel.AddToClassList("info-box");
                _listContainer.Add(infoLabel);
                return;
            }

            foreach (var entry in filteredEntries)
            {
                var button = new Button();
                button.AddToClassList("selection-button");

                // 3カラムレイアウトで表示するコンテナを作成
                button.text = ""; // デフォルトテキストをクリア

                var container = new VisualElement();
                container.style.flexDirection = FlexDirection.Row;
                container.style.flexGrow = 1;
                container.style.alignItems = Align.Center;
                container.pickingMode = PickingMode.Ignore;

                // GameObject名
                var gameObjectLabel = new Label(entry.GameObject.name);
                gameObjectLabel.AddToClassList("column-gameobject");
                gameObjectLabel.pickingMode = PickingMode.Ignore;

                // Component名
                var componentName = entry.Input.GetType().Name;
                var componentLabel = new Label(componentName);
                componentLabel.AddToClassList("column-component");
                componentLabel.pickingMode = PickingMode.Ignore;

                // Type名（背景色付き）
                var typeName = entry.GenericType != null ? GetFriendlyTypeName(entry.GenericType) : "Unknown";
                var typeLabel = new Label(typeName);
                typeLabel.AddToClassList("column-type");
                typeLabel.AddToClassList(GetTypeStyleClass(entry.GenericType)); // 型に応じたスタイルクラスを追加
                typeLabel.pickingMode = PickingMode.Ignore;

                container.Add(gameObjectLabel);
                container.Add(componentLabel);
                container.Add(typeLabel);

                button.Add(container);

                button.clicked += () =>
                {
                    _onSelectionCallback?.Invoke(entry.Input);
                    Close();
                };

                _listContainer.Add(button);
            }
        }

        static Type GetGenericArgumentType(Type type)
        {
            // UgenInput<T>のTを取得
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(UgenInput<>))
                {
                    return type.GetGenericArguments()[0];
                }

                type = type.BaseType;
            }

            return null;
        }

        static string GetFriendlyTypeName(Type type)
        {
            if (type == null) return "null";
            if (type == typeof(float)) return "Float";
            return type.Name;
        }

        static string GetTypeStyleClass(Type type)
        {
            if (type == null) return "type-unknown";
            if (type == typeof(bool)) return "type-bool";
            if (type == typeof(int) || type == typeof(uint)) return "type-int";
            if (type == typeof(float) || type == typeof(double)) return "type-float";
            if (type == typeof(Vector2) || type == typeof(Vector3) || type == typeof(Vector4)) return "type-vector";
            if (type == typeof(Color)) return "type-color";
            return "type-unknown";
        }
    }
}
