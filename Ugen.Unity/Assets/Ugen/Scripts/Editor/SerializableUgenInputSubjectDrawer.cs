using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Ugen.Graph;

namespace Ugen.Editor
{
    [CustomPropertyDrawer(typeof(SerializableUgenInputSubject<>), true)]
    public sealed class SerializableUgenInputSubjectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var valueProperty = property.FindPropertyRelative("_value");
            if (valueProperty != null)
            {
                var valueHeight = EditorGUI.GetPropertyHeight(valueProperty);
                EditorGUI.PropertyField(position, valueProperty, label, true);
                position.y += valueHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                EditorGUI.LabelField(position, label.text);
            }

            // Sendボタンを描画
            var buttonRect = new Rect(
                position.x + EditorGUIUtility.labelWidth,
                position.y,
                position.width - EditorGUIUtility.labelWidth,
                EditorGUIUtility.singleLineHeight);

            if (GUI.Button(buttonRect, "Send"))
            {
                SendValue(property);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("_value");

            var valueHeight = valueProperty == null ? 0 : EditorGUI.GetPropertyHeight(valueProperty, true);
            var buttonHeight = EditorGUIUtility.singleLineHeight;
            var spacing = EditorGUIUtility.standardVerticalSpacing;

            return valueHeight + spacing + buttonHeight;
        }

        void SendValue(SerializedProperty property)
        {
            // FIXME: ネストしている場合に対応していない
            var targetObject = property.serializedObject.targetObject;
            var fldInfo = targetObject.GetType().GetField(property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var propObject = fldInfo?.GetValue(targetObject);

            if (propObject == null) return;
            var subjectType = propObject.GetType();
            var sendMethod = subjectType.GetMethod("ForceNotify", BindingFlags.Public | BindingFlags.Instance);

            if (sendMethod != null)
            {
                // Send メソッドを呼び出す
                sendMethod.Invoke(propObject, null);
            }
        }
    }
}
