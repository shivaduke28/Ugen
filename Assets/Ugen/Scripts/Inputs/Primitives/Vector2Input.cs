using R3;
using Ugen.Inputs.Bindings;
using UnityEngine;

namespace Ugen.Inputs.Primitives
{
    [AddComponentMenu("Ugen/Ugen Vector2 Input")]
    public sealed class Vector2Input : UgenInput<Vector2>
    {
        [SerializeField] FloatBinding _x;
        [SerializeField] FloatBinding _y;

        public override Observable<Vector2> AsObservable()
        {
            var x1 = _x != null ? _x.AsObservable() : Observable.Return(0f);
            var y1 = _y != null ? _y.AsObservable() : Observable.Return(0f);
            return x1.CombineLatest(y1, (x, y) => new Vector2(x, y));
        }
    }
}
