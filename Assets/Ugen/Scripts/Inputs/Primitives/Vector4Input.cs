using R3;
using Ugen.Inputs.Bindings;
using UnityEngine;

namespace Ugen.Inputs.Primitives
{
    [AddComponentMenu("Ugen/Ugen Vector4 Input")]
    public sealed class Vector4Input : UgenInput<Vector4>
    {
        [SerializeField] FloatBinding _x;
        [SerializeField] FloatBinding _y;
        [SerializeField] FloatBinding _z;
        [SerializeField] FloatBinding _w;

        public override Observable<Vector4> AsObservable()
        {
            var x = _x != null ? _x.AsObservable() : Observable.Return(0f);
            var y = _y != null ? _y.AsObservable() : Observable.Return(0f);
            var z = _z != null ? _z.AsObservable() : Observable.Return(0f);
            var w = _w != null ? _w.AsObservable() : Observable.Return(0f);
            return x.CombineLatest(y, z, w, (x1, y1, z1, w1) => new Vector4(x1, y1, z1, w1));
        }
    }
}
