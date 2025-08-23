using Ugen.Attributes;
using Ugen.Inputs;
using UnityEditor;
using UnityEngine;

namespace Ugen.Editor
{
    [CustomPropertyDrawer(typeof(UgenInputSelectorAttribute))]
    public sealed class UgenInputSelectorPropertyDrawer : PropertyDrawer
    {
        const float ButtonHeight = 20f;
        const float Spacing = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // ラベルを表示
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // インデントレベルを保存して一時的にリセット
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // 通常のObjectFieldを表示（ドラッグ&ドロップ対応）
            var objectFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.ObjectField(objectFieldRect, property.objectReferenceValue, typeof(UgenInput), true);
            if (EditorGUI.EndChangeCheck())
            {
                property.objectReferenceValue = newValue;
            }

            // 選択ボタンを表示
            var buttonRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + Spacing,
                position.width, ButtonHeight);

            if (GUI.Button(buttonRect, "Select"))
            {
                UgenInputSelectionWindow.ShowWindow(selectedInput =>
                {
                    property.objectReferenceValue = selectedInput;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + Spacing + EditorGUIUtility.singleLineHeight;
        }
    }
}
