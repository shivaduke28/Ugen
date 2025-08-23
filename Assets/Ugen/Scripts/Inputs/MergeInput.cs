using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace Ugen.Inputs
{
    [Serializable]
    public abstract class MergeInput<T> : UgenInput<T>
    {
        [SerializeField] List<UgenInput<T>> _inputs = new();

        public override Observable<T> Observable() => _inputs.Select(x => x.Observable()).Merge();
    }

    [Serializable]
    public class FloatMergeInput : MergeInput<float>
    {
    }

    [Serializable]
    public class IntMergeInput : MergeInput<int>
    {
    }

    [Serializable]
    public class BoolMergeInput : MergeInput<bool>
    {
    }

    [Serializable]
    public class Vector2MergeInput : MergeInput<Vector2>
    {
    }

    [Serializable]
    public class Vector3MergeInput : MergeInput<Vector3>
    {
    }

    [Serializable]
    public class Vector4MergeInput : MergeInput<Vector4>
    {
    }

    [Serializable]
    public class ColorMergeInput : MergeInput<Color>
    {
    }
}
