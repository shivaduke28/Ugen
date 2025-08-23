using System;
using R3;
using UnityEngine;

namespace Ugen.Inputs
{
    public sealed class IntervalInput : UgenInput<Unit>
    {
        [SerializeField] float _intervalSec = 1f;


        public override Observable<Unit> AsObservable()
        {
            return Observable.Interval(TimeSpan.FromSeconds(_intervalSec));
        }
    }
}
