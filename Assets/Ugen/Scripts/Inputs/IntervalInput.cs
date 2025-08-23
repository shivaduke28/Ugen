using System;
using R3;
using Ugen.Inputs.Bindings;
using UnityEngine;

namespace Ugen.Inputs
{
    public sealed class IntervalInput : UgenInput<Unit>
    {
        [SerializeField] SerializableReactiveProperty<float> _interval = new(1f);
        [SerializeField] FloatBinding _intervalBinding;

        void Start()
        {
            if (_intervalBinding != null)
            {
                _intervalBinding.AsObservable()
                    .Where(interval => interval >= 0) // 負の値は無視
                    .Subscribe(interval => _interval.Value = interval)
                    .AddTo(this);
            }
        }

        public override Observable<Unit> AsObservable()
        {
            return _interval
                .Select(interval => interval == 0 ? Observable.EveryUpdate() : Observable.Interval(TimeSpan.FromSeconds(interval)))
                .Switch();
        }
    }
}
