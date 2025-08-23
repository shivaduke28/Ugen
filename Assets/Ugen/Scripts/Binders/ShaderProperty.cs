using System;
using Ugen.Bindings;
using UnityEngine;
using UnityEngine.VFX.Utility;

namespace Ugen.Binders
{
    [Serializable]
    public abstract class ShaderProperty
    {
        public ExposedProperty Property;
    }

    [Serializable]
    public sealed class FloatProperty : ShaderProperty
    {
        public FloatBinding Binding;
    }

    [Serializable]
    public sealed class IntProperty : ShaderProperty
    {
        public IntBinding Binding;
    }

    [Serializable]
    public sealed class BoolProperty : ShaderProperty
    {
        public BoolBinding Binding;
    }

    [Serializable]
    public sealed class UintProperty : ShaderProperty
    {
        public UintBinding Binding;
    }

    [Serializable]
    public sealed class Vector2Property : ShaderProperty
    {
        public Vector2Binding Binding;
    }

    [Serializable]
    public sealed class Vector3Property : ShaderProperty
    {
        public Vector3Binding Binding;
    }

    [Serializable]
    public sealed class Vector4Property : ShaderProperty
    {
        public Vector4Binding Binding;
    }

    [Serializable]
    public sealed class ColorProperty : ShaderProperty
    {
        public ColorBinding Binding;
    }
}
