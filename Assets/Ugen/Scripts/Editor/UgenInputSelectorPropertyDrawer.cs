using Ugen.Attributes;
using Ugen.Inputs;
using UnityEditor;
using UnityEngine;

namespace Ugen.Editor
{
    [CustomPropertyDrawer(typeof(UgenInputSelectorAttribute))]
    public class UgenInputSelectorPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // ラベルを描画
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // 現在の値を取得
            var currentInput = property.objectReferenceValue as UgenInput;

            // 表示するテキストを作成
            var displayText = "None";
            if (currentInput != null)
            {
                var componentTypeName = currentInput.GetType().Name;
                displayText = $"{currentInput.gameObject.name} ({componentTypeName})";
            }

            // フィールドとボタンを配置
            var fieldRect = new Rect(position.x, position.y, position.width - 22, position.height);
            var buttonRect = new Rect(position.x + position.width - 20, position.y, 20, position.height);

            // 読み取り専用風のフィールド表示
            GUI.enabled = false;
            EditorGUI.TextField(fieldRect, displayText);
            GUI.enabled = true;

            // 選択ボタン
            if (GUI.Button(buttonRect, "..."))
            {
                UgenInputSelectionWindow.ShowWindow((selectedInput) =>
                {
                    property.objectReferenceValue = selectedInput;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
