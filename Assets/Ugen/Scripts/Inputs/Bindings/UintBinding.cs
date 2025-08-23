using System;
using R3;
using Ugen.Attributes;
using UnityEngine;

namespace Ugen.Inputs.Bindings
{
    [Serializable]
    public sealed class UintBinding
    {
        [SerializeField, UgenInputSelector] UgenInput _input;

        public Observable<uint> AsObservable()
        {
            return _input switch
            {
                UgenInput<uint> input => input.AsObservable(),
                UgenInput<int> input => input.AsObservable().Select(x => (uint)x),
                UgenInput<float> input => input.AsObservable().Select(x => (uint)x),
                UgenInput<double> input => input.AsObservable().Select(x => (uint)x),
                UgenInput<bool> input => input.AsObservable().Select(x => x ? 1u : 0u),
                UgenInput<Vector2> input => input.AsObservable().Select(x => (uint)x.x),
                UgenInput<Vector3> input => input.AsObservable().Select(x => (uint)x.x),
                UgenInput<Vector4> input => input.AsObservable().Select(x => (uint)x.x),
                UgenInput<Color> input => input.AsObservable().Select(x => (uint)x.r),
                _ => Observable.Never<uint>()
            };
        }
    }
}
