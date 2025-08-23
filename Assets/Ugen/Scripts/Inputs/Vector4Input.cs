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

        public override Observable<Vector4> AsObservable()
        {
            if (_x == null || _y == null || _z == null || _w == null)
            {
                return Observable.Never<Vector4>();
            }

            return _x.AsObservable().CombineLatest(_y.AsObservable(), _z.AsObservable(), _w.AsObservable(),
                (x, y, z, w) => new Vector4(x, y, z, w));
        }
    }
}
