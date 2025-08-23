using System;
using R3;
using Ugen.Attributes;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Bindings
{
    [Serializable]
    public sealed class Vector2Binding
    {
        [SerializeField, UgenInputSelector] UgenInput _input;

        public Observable<Vector2> AsObservable()
        {
            return _input switch
            {
                UgenInput<Vector2> input => input.AsObservable(),
                UgenInput<Vector3> input => input.AsObservable().Select(x => new Vector2(x.x, x.y)),
                UgenInput<Vector4> input => input.AsObservable().Select(x => new Vector2(x.x, x.y)),
                UgenInput<float> input => input.AsObservable().Select(x => new Vector2(x, x)),
                UgenInput<double> input => input.AsObservable().Select(x => new Vector2((float)x, (float)x)),
                UgenInput<int> input => input.AsObservable().Select(x => new Vector2(x, x)),
                UgenInput<uint> input => input.AsObservable().Select(x => new Vector2(x, x)),
                UgenInput<Color> input => input.AsObservable().Select(x => new Vector2(x.r, x.g)),
                _ => Observable.Never<Vector2>()
            };
        }
    }
}
