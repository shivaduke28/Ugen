using System;
using R3;
using Ugen.Attributes;
using UnityEngine;

namespace Ugen.Inputs.Bindings
{
    [Serializable]
    public sealed class Vector3Binding
    {
        [SerializeField, UgenInputSelector] UgenInput _input;

        public Observable<Vector3> AsObservable()
        {
            return _input switch
            {
                UgenInput<Vector3> input => input.AsObservable(),
                UgenInput<Vector2> input => input.AsObservable().Select(x => new Vector3(x.x, x.y, 0f)),
                UgenInput<Vector4> input => input.AsObservable().Select(x => new Vector3(x.x, x.y, x.z)),
                UgenInput<float> input => input.AsObservable().Select(x => new Vector3(x, x, x)),
                UgenInput<double> input => input.AsObservable().Select(x => new Vector3((float)x, (float)x, (float)x)),
                UgenInput<int> input => input.AsObservable().Select(x => new Vector3(x, x, x)),
                UgenInput<uint> input => input.AsObservable().Select(x => new Vector3(x, x, x)),
                UgenInput<Color> input => input.AsObservable().Select(x => new Vector3(x.r, x.g, x.b)),
                _ => Observable.Never<Vector3>()
            };
        }
    }
}
