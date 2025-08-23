using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ugen.Attributes;
using UnityEditor;
using UnityEngine;

namespace Ugen.Editor
{
    //         DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE
    //                    Version 2, December 2004
    //
    // Copyright (C) 2004 Sam Hocevar <sam@hocevar.net>
    //
    // Everyone is permitted to copy and distribute verbatim or modified
    // copies of this license document, and changing it is allowed as long
    // as the name is changed.
    //
    //            DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE
    //   TERMS AND CONDITIONS FOR COPYING, DISTRIBUTION AND MODIFICATION
    //
    //  0. You just DO WHAT THE FUCK YOU WANT TO.
    // from: https://light11.hatenadiary.com/entry/2021/11/30/190034
    [CustomPropertyDrawer(typeof(SerializeReferenceSelectorAttribute))]
    public sealed class SerializeReferenceSelectorAttributeDrawer : PropertyDrawer
    {
        readonly Dictionary<string, PropertyData> _dataPerPath = new();

        PropertyData _data;

        int _selectedIndex;

        void Init(SerializedProperty property)
        {
            if (_dataPerPath.TryGetValue(property.propertyPath, out _data)) return;

            _data = new PropertyData(property);
            _dataPerPath.Add(property.propertyPath, _data);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init(property);

            var fullTypeName = property.managedReferenceFullTypename.Split(' ').Last();
            _selectedIndex = Array.IndexOf(_data.DerivedFullTypeNames, fullTypeName);

            using (var ccs = new EditorGUI.ChangeCheckScope())
            {
                var selectorPosition = position;

                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                selectorPosition.width -= EditorGUIUtility.labelWidth;
                selectorPosition.x += EditorGUIUtility.labelWidth;
                selectorPosition.height = EditorGUIUtility.singleLineHeight;
                var selectedTypeIndex = EditorGUI.Popup(selectorPosition, _selectedIndex, _data.DerivedTypeNames);
                if (ccs.changed)
                {
                    _selectedIndex = selectedTypeIndex;
                    var selectedType = _data.DerivedTypes[selectedTypeIndex];
                    property.managedReferenceValue =
                        selectedType == null ? null : Activator.CreateInstance(selectedType);
                }

                EditorGUI.indentLevel = indent;
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);

            if (string.IsNullOrEmpty(property.managedReferenceFullTypename)) return EditorGUIUtility.singleLineHeight;

            return EditorGUI.GetPropertyHeight(property, true);
        }

        sealed class PropertyData
        {
            internal PropertyData(SerializedProperty property)
            {
                var managedReferenceFieldTypenameSplit = property.managedReferenceFieldTypename.Split(' ');
                var assemblyName = managedReferenceFieldTypenameSplit[0];
                var fieldTypeName = managedReferenceFieldTypenameSplit[1];
                var fieldType = GetAssembly(assemblyName).GetType(fieldTypeName);
                DerivedTypes = TypeCache.GetTypesDerivedFrom(fieldType).Where(x => !x.IsAbstract && !x.IsInterface && x.GetConstructor(Type.EmptyTypes) != null).ToArray();
                DerivedTypeNames = new string[DerivedTypes.Length];
                DerivedFullTypeNames = new string[DerivedTypes.Length];
                for (var i = 0; i < DerivedTypes.Length; i++)
                {
                    var type = DerivedTypes[i];
                    DerivedTypeNames[i] = ObjectNames.NicifyVariableName(type.Name);
                    DerivedFullTypeNames[i] = type.FullName?.Replace("+", "/"); // for nested class (modified from the original code)
                }
            }

            public Type[] DerivedTypes { get; }

            public string[] DerivedTypeNames { get; }

            public string[] DerivedFullTypeNames { get; }

            static Assembly GetAssembly(string name)
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(assembly => assembly.GetName().Name == name);
            }
        }
    }
}
