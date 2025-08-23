using System;
using R3;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Bindings
{
    [Serializable]
    public sealed class ColorBinding
    {
        [SerializeField] UgenInput _input;

        public Observable<Color> AsObservable()
        {
            return _input switch
            {
                UgenInput<Color> input => input.AsObservable(),
                UgenInput<Vector4> input => input.AsObservable().Select(x => new Color(x.x, x.y, x.z, x.w)),
                UgenInput<Vector3> input => input.AsObservable().Select(x => new Color(x.x, x.y, x.z, 1f)),
                UgenInput<Vector2> input => input.AsObservable().Select(x => new Color(x.x, x.y, 0f, 1f)),
                UgenInput<float> input => input.AsObservable().Select(x => new Color(x, x, x, 1f)),
                UgenInput<double> input => input.AsObservable().Select(x => new Color((float)x, (float)x, (float)x, 1f)),
                UgenInput<int> input => input.AsObservable().Select(x => new Color(x / 255f, x / 255f, x / 255f, 1f)),
                UgenInput<uint> input => input.AsObservable().Select(x => new Color(x / 255f, x / 255f, x / 255f, 1f)),
                UgenInput<bool> input => input.AsObservable().Select(x => x ? Color.white : Color.black),
                _ => Observable.Never<Color>()
            };
        }
    }
}
