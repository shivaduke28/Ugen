using System;
using R3;
using Ugen.Attributes;
using UnityEngine;

namespace Ugen.Inputs.Bindings
{
    [Serializable]
    public sealed class FloatBinding
    {
        [SerializeField, UgenInputSelector] UgenInput _input;

        public Observable<float> AsObservable()
        {
            return _input switch
            {
                UgenInput<float> input => input.AsObservable(),
                UgenInput<double> input => input.AsObservable().Select(x => (float)x),
                UgenInput<uint> input => input.AsObservable().Select(x => (float)x),
                UgenInput<int> input => input.AsObservable().Select(x => (float)x),
                UgenInput<bool> input => input.AsObservable().Select(x => x ? 1f : 0f),
                UgenInput<Vector2> input => input.AsObservable().Select(x => x.x),
                UgenInput<Vector3> input => input.AsObservable().Select(x => x.x),
                UgenInput<Vector4> input => input.AsObservable().Select(x => x.x),
                UgenInput<Color> input => input.AsObservable().Select(x => x.r),
                _ => Observable.Never<float>()
            };
        }
    }
}
