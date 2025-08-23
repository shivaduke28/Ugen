using System;
using System.Collections.Generic;
using System.Linq;
using Ugen.Inputs;
using UnityEditor;
using UnityEngine;

namespace Ugen.Editor
{
    public class UgenInputSelectionWindow : EditorWindow
    {
        Action<UgenInput> _onSelectionCallback;
        Vector2 _scrollPosition;
        string _searchString = "";
        GameObject _selectedGameObject;
        readonly Dictionary<GameObject, List<UgenInput>> _gameObjectToInputs = new();
        bool _showComponents;

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

            // 状態をリセット（常にGameObject一覧から開始）
            _scrollPosition = Vector2.zero;
            _searchString = "";
            _selectedGameObject = null;
            _showComponents = false;

            RefreshGameObjectList();
        }

        void OnEnable()
        {
            RefreshGameObjectList();
        }

        void RefreshGameObjectList()
        {
            _gameObjectToInputs.Clear();

            // シーン内のすべてのUgenInputを検索
            var allInputs = FindObjectsByType<UgenInput>(FindObjectsSortMode.None);

            // GameObjectごとにグループ化
            foreach (var input in allInputs)
            {
                if (!_gameObjectToInputs.ContainsKey(input.gameObject))
                {
                    _gameObjectToInputs[input.gameObject] = new List<UgenInput>();
                }

                _gameObjectToInputs[input.gameObject].Add(input);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            // ヘッダー
            EditorGUILayout.LabelField("Select UgenInput", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // リフレッシュボタン
            if (GUILayout.Button("Refresh List"))
            {
                RefreshGameObjectList();
                _showComponents = false;
                _selectedGameObject = null;
            }

            EditorGUILayout.Space();

            // パンくずリスト風のナビゲーション
            if (_showComponents && _selectedGameObject != null)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                if (GUILayout.Button("← GameObjects", EditorStyles.toolbarButton, GUILayout.Width(100)))
                {
                    _showComponents = false;
                }

                GUILayout.Label($"/ {_selectedGameObject.name}", EditorStyles.toolbarButton);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            // 検索フィールド
            if (!_showComponents)
            {
                EditorGUI.BeginChangeCheck();
                _searchString = EditorGUILayout.TextField("Search", _searchString, EditorStyles.toolbarSearchField);
                if (EditorGUI.EndChangeCheck())
                {
                    Repaint();
                }

                EditorGUILayout.Space(5);
            }

            // メインコンテンツ
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));

            if (!_showComponents)
            {
                // GameObject一覧を表示
                ShowGameObjectList();
            }
            else if (_selectedGameObject != null)
            {
                // Component一覧を表示
                ShowComponentList();
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        void ShowGameObjectList()
        {
            // Noneオプションを最初に表示
            if (GUILayout.Button("None - Clear Selection", GUILayout.Height(25)))
            {
                _onSelectionCallback?.Invoke(null);
                Close();
            }

            EditorGUILayout.Space(2);

            // フィルタリングされたGameObject一覧
            var filteredGameObjects = string.IsNullOrEmpty(_searchString)
                ? _gameObjectToInputs.Keys.ToList()
                : _gameObjectToInputs.Keys.Where(go =>
                    go.name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            // 名前でソート
            filteredGameObjects.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));

            if (filteredGameObjects.Count == 0)
            {
                EditorGUILayout.HelpBox("No GameObjects with UgenInput found", MessageType.Info);
                return;
            }

            foreach (var go in filteredGameObjects)
            {
                var count = _gameObjectToInputs[go].Count;

                EditorGUILayout.BeginHorizontal();

                // GameObject名とコンポーネント数を表示
                string buttonLabel;
                if (count == 1)
                {
                    var input = _gameObjectToInputs[go][0];
                    var genericType = GetGenericArgumentType(input.GetType());
                    var typeName = genericType != null ? GetFriendlyTypeName(genericType) : "?";
                    buttonLabel = $"{go.name} [{typeName}]";
                }
                else
                {
                    buttonLabel = $"{go.name} ({count} components)";
                }

                if (GUILayout.Button(buttonLabel, GUILayout.Height(25)))
                {
                    _selectedGameObject = go;

                    // コンポーネントが1つの場合は自動選択
                    if (count == 1)
                    {
                        var singleInput = _gameObjectToInputs[go][0];
                        _onSelectionCallback?.Invoke(singleInput);
                        Close();
                    }
                    else
                    {
                        _showComponents = true;
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        void ShowComponentList()
        {
            if (!_gameObjectToInputs.ContainsKey(_selectedGameObject))
            {
                EditorGUILayout.HelpBox("No UgenInput components found", MessageType.Warning);
                return;
            }

            var inputs = _gameObjectToInputs[_selectedGameObject];

            EditorGUILayout.LabelField($"Select Component from {_selectedGameObject.name}:", EditorStyles.miniBoldLabel);
            EditorGUILayout.Space();

            foreach (var input in inputs)
            {
                // コンポーネントの型を取得
                var genericType = GetGenericArgumentType(input.GetType());
                var typeName = genericType != null ? GetFriendlyTypeName(genericType) : "Unknown";
                var componentName = input.GetType().Name;

                // 色付きボタンで表示
                var typeColor = GetTypeColor(genericType);
                var oldColor = GUI.backgroundColor;
                GUI.backgroundColor = typeColor;

                var buttonLabel = $"{componentName} ({typeName})";
                if (GUILayout.Button(buttonLabel, GUILayout.Height(30)))
                {
                    _onSelectionCallback?.Invoke(input);
                    Close();
                }

                GUI.backgroundColor = oldColor;
                EditorGUILayout.Space(3);
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

        Color GetTypeColor(Type type)
        {
            if (type == null) return Color.gray;
            if (type == typeof(bool)) return new Color(0.8f, 0.3f, 0.3f);
            if (type == typeof(int) || type == typeof(uint)) return new Color(0.3f, 0.6f, 0.8f);
            if (type == typeof(float) || type == typeof(double)) return new Color(0.3f, 0.8f, 0.3f);
            if (type == typeof(Vector2) || type == typeof(Vector3) || type == typeof(Vector4)) return new Color(0.8f, 0.6f, 0.3f);
            if (type == typeof(Color)) return new Color(0.8f, 0.3f, 0.8f);
            return Color.gray;
        }
    }
}
