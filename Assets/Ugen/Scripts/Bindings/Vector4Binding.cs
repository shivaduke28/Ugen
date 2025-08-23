using System;
using R3;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Bindings
{
    [Serializable]
    public sealed class Vector4Binding
    {
        [SerializeField] UgenInput _input;

        public Observable<Vector4> AsObservable()
        {
            return _input switch
            {
                UgenInput<Vector4> input => input.AsObservable(),
                UgenInput<Vector2> input => input.AsObservable().Select(x => new Vector4(x.x, x.y, 0f, 0f)),
                UgenInput<Vector3> input => input.AsObservable().Select(x => new Vector4(x.x, x.y, x.z, 0f)),
                UgenInput<float> input => input.AsObservable().Select(x => new Vector4(x, x, x, x)),
                UgenInput<double> input => input.AsObservable().Select(x => new Vector4((float)x, (float)x, (float)x, (float)x)),
                UgenInput<int> input => input.AsObservable().Select(x => new Vector4(x, x, x, x)),
                UgenInput<uint> input => input.AsObservable().Select(x => new Vector4(x, x, x, x)),
                UgenInput<Color> input => input.AsObservable().Select(x => new Vector4(x.r, x.g, x.b, x.a)),
                _ => Observable.Never<Vector4>()
            };
        }
    }
}
