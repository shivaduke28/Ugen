using System;
using R3;
using Ugen.Attributes;
using UnityEngine;

namespace Ugen.Inputs.Bindings
{
    [Serializable]
    public sealed class IntBinding
    {
        [SerializeField, UgenInputSelector] UgenInput _input;

        public Observable<int> AsObservable()
        {
            return _input switch
            {
                UgenInput<int> input => input.AsObservable(),
                UgenInput<uint> input => input.AsObservable().Select(x => (int)x),
                UgenInput<float> input => input.AsObservable().Select(x => (int)x),
                UgenInput<double> input => input.AsObservable().Select(x => (int)x),
                UgenInput<bool> input => input.AsObservable().Select(x => x ? 1 : 0),
                UgenInput<Vector2> input => input.AsObservable().Select(x => (int)x.x),
                UgenInput<Vector3> input => input.AsObservable().Select(x => (int)x.x),
                UgenInput<Vector4> input => input.AsObservable().Select(x => (int)x.x),
                UgenInput<Color> input => input.AsObservable().Select(x => (int)(x.r * 255)),
                _ => Observable.Never<int>()
            };
        }
    }
}
