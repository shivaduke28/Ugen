using System;
using R3;
using Ugen.Attributes;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Bindings
{
    [Serializable]
    public sealed class UnitBinding
    {
        [SerializeField, UgenInputSelector] UgenInput _input;

        public Observable<Unit> AsObservable()
        {
            return _input switch
            {
                UgenInput<Unit> input => input.AsObservable(),
                UgenInput<bool> input => input.AsObservable().AsUnitObservable(),
                UgenInput<int> input => input.AsObservable().AsUnitObservable(),
                UgenInput<uint> input => input.AsObservable().AsUnitObservable(),
                UgenInput<float> input => input.AsObservable().AsUnitObservable(),
                UgenInput<double> input => input.AsObservable().AsUnitObservable(),
                UgenInput<Vector2> input => input.AsObservable().AsUnitObservable(),
                UgenInput<Vector3> input => input.AsObservable().AsUnitObservable(),
                UgenInput<Vector4> input => input.AsObservable().AsUnitObservable(),
                UgenInput<Color> input => input.AsObservable().AsUnitObservable(),
                _ => Observable.Never<Unit>()
            };
        }
    }
}
