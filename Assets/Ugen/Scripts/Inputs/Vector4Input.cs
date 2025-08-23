using R3;
using UnityEngine;

namespace Ugen.Inputs
{
    [AddComponentMenu("Ugen/Ugen Vector4 Input")]
    public sealed class Vector4Input : UgenInput<Vector4>
    {
        [SerializeField] UgenInput<float> _x;
        [SerializeField] UgenInput<float> _y;
        [SerializeField] UgenInput<float> _z;
        [SerializeField] UgenInput<float> _w;

        public override Observable<Vector4> Observable()
        {
            if (_x == null || _y == null || _z == null || _w == null)
            {
                return R3.Observable.Never<Vector4>();
            }
            return _x.Observable().CombineLatest(_y.Observable(), _z.Observable(), _w.Observable(),
                (x, y, z, w) => new Vector4(x, y, z, w));
        }
    }
}
