using R3;
using Ugen.Inputs.Bindings;
using UnityEngine;

namespace Ugen.Inputs.Primitives
{
    [AddComponentMenu("Ugen/Ugen Vector3 Input")]
    public sealed class Vector3Input : UgenInput<Vector3>
    {
        [SerializeField] FloatBinding _x;
        [SerializeField] FloatBinding _y;
        [SerializeField] FloatBinding _z;

        public override Observable<Vector3> AsObservable()
        {
            var x = _x != null ? _x.AsObservable() : Observable.Return(0f);
            var y = _y != null ? _y.AsObservable() : Observable.Return(0f);
            var z = _z != null ? _z.AsObservable() : Observable.Return(0f);
            return x.CombineLatest(y, z, (x1, y1, z1) => new Vector3(x1, y1, z1));
        }
    }
}
