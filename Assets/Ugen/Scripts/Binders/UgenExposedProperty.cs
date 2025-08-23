using System;
using Ugen.Inputs;
using UnityEngine;
using UnityEngine.VFX.Utility;

namespace Ugen.Binders
{
    [Serializable]
    public abstract class UgenExposedProperty
    {
        public ExposedProperty Property;
    }

    [Serializable]
    public abstract class UgenExposedProperty<T> : UgenExposedProperty
    {
        public UgenInputList<T> InputList;
    }

    [Serializable]
    public sealed class FloatProperty : UgenExposedProperty<float>
    {
    }

    [Serializable]
    public sealed class IntProperty : UgenExposedProperty<int>
    {
    }

    [Serializable]
    public sealed class BoolProperty : UgenExposedProperty<bool>
    {
    }

    [Serializable]
    public sealed class UintProperty : UgenExposedProperty<uint>
    {
    }

    [Serializable]
    public sealed class Vector2Property : UgenExposedProperty<Vector2>
    {
    }

    [Serializable]
    public sealed class Vector3Property : UgenExposedProperty<Vector3>
    {
    }

    [Serializable]
    public sealed class Vector4Property : UgenExposedProperty<Vector4>
    {
    }

    [Serializable]
    public sealed class ColorProperty : UgenExposedProperty<Color>
    {
    }
}
