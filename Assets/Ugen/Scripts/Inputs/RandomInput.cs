using R3;
using Ugen.Bindings;
using UnityEngine;

namespace Ugen.Inputs
{
    [AddComponentMenu("Ugen/Ugen Random Input")]
    public sealed class RandomInput : UgenInput<float>
    {
        [SerializeField] float _min = 0f;
        [SerializeField] float _max = 1f;

        [SerializeField] UnitBinding _update;

        public override Observable<float> AsObservable()
        {
            if (_update != null)
            {
                return _update.AsObservable()
                    .Select(_ => Random.Range(_min, _max));
            }

            return Observable.Return(Random.Range(_min, _max));
        }
    }
}
