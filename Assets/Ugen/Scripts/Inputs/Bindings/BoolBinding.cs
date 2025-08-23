using System;
using R3;
using Ugen.Attributes;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Bindings
{
    [Serializable]
    public sealed class BoolBinding
    {
        [SerializeField, UgenInputSelector] UgenInput _input;

        public Observable<bool> AsObservable()
        {
            return _input switch
            {
                UgenInput<bool> input => input.AsObservable(),
                UgenInput<int> input => input.AsObservable().Select(x => x != 0),
                UgenInput<uint> input => input.AsObservable().Select(x => x != 0),
                UgenInput<float> input => input.AsObservable().Select(x => x != 0f),
                UgenInput<double> input => input.AsObservable().Select(x => x != 0.0),
                UgenInput<Vector2> input => input.AsObservable().Select(x => x != Vector2.zero),
                UgenInput<Vector3> input => input.AsObservable().Select(x => x != Vector3.zero),
                UgenInput<Vector4> input => input.AsObservable().Select(x => x != Vector4.zero),
                UgenInput<Color> input => input.AsObservable().Select(x => x.a > 0f),
                _ => Observable.Never<bool>()
            };
        }
    }
}
