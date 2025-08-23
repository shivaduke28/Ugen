using System;
using R3;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Bindings
{
    [Serializable]
    public sealed class UintBinding
    {
        [SerializeField] UgenInput _input;

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
                UgenInput<Color> input => input.AsObservable().Select(x => (uint)(x.r * 255)),
                _ => Observable.Never<uint>()
            };
        }
    }
}