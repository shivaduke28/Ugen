using Ugen.Attributes;
using Ugen.Inputs;
using UnityEditor;
using UnityEngine;

namespace Ugen.Editor
{
    [CustomPropertyDrawer(typeof(UgenInputSelectorAttribute))]
    public sealed class UgenInputSelectorPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var currentInput = property.objectReferenceValue as UgenInput;

            var displayText = "None";
            if (currentInput != null)
            {
                var componentTypeName = currentInput.GetType().Name;
                displayText = $"{currentInput.gameObject.name} ({componentTypeName})";
            }

            // 全体をボタンとして表示
            var buttonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(8, 8, 2, 2)
            };

            if (GUI.Button(position, displayText, buttonStyle))
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
