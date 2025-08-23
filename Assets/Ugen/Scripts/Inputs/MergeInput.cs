using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Ugen.Bindings;
using UnityEngine;

namespace Ugen.Inputs
{
    [Serializable]
    public abstract class MergeInput<T> : UgenInput<T>
    {
    }

    [Serializable]
    public class FloatMergeInput : MergeInput<float>
    {
        [SerializeField] List<FloatBinding> _bindings = new();
        
        public override Observable<float> AsObservable() => _bindings.Select(x => x.AsObservable()).Merge();
    }

    [Serializable]
    public class IntMergeInput : MergeInput<int>
    {
        [SerializeField] List<IntBinding> _bindings = new();
        
        public override Observable<int> AsObservable() => _bindings.Select(x => x.AsObservable()).Merge();
    }

    [Serializable]
    public class BoolMergeInput : MergeInput<bool>
    {
        [SerializeField] List<BoolBinding> _bindings = new();
        
        public override Observable<bool> AsObservable() => _bindings.Select(x => x.AsObservable()).Merge();
    }

    [Serializable]
    public class Vector2MergeInput : MergeInput<Vector2>
    {
        [SerializeField] List<Vector2Binding> _bindings = new();
        
        public override Observable<Vector2> AsObservable() => _bindings.Select(x => x.AsObservable()).Merge();
    }

    [Serializable]
    public class Vector3MergeInput : MergeInput<Vector3>
    {
        [SerializeField] List<Vector3Binding> _bindings = new();
        
        public override Observable<Vector3> AsObservable() => _bindings.Select(x => x.AsObservable()).Merge();
    }

    [Serializable]
    public class Vector4MergeInput : MergeInput<Vector4>
    {
        [SerializeField] List<Vector4Binding> _bindings = new();
        
        public override Observable<Vector4> AsObservable() => _bindings.Select(x => x.AsObservable()).Merge();
    }

    [Serializable]
    public class ColorMergeInput : MergeInput<Color>
    {
        [SerializeField] List<ColorBinding> _bindings = new();
        
        public override Observable<Color> AsObservable() => _bindings.Select(x => x.AsObservable()).Merge();
    }
}
