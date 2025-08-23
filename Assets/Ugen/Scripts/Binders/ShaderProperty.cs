using System;
using Ugen.Inputs;
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
    public abstract class ShaderProperty<T> : ShaderProperty
    {
        public UgenInput<T> Input;
    }

    [Serializable]
    public sealed class FloatProperty : ShaderProperty<float>
    {
    }

    [Serializable]
    public sealed class IntProperty : ShaderProperty<int>
    {
    }

    [Serializable]
    public sealed class BoolProperty : ShaderProperty<bool>
    {
    }

    [Serializable]
    public sealed class UintProperty : ShaderProperty<uint>
    {
    }

    [Serializable]
    public sealed class Vector2Property : ShaderProperty<Vector2>
    {
    }

    [Serializable]
    public sealed class Vector3Property : ShaderProperty<Vector3>
    {
    }

    [Serializable]
    public sealed class Vector4Property : ShaderProperty<Vector4>
    {
    }

    [Serializable]
    public sealed class ColorProperty : ShaderProperty<Color>
    {
    }
}
