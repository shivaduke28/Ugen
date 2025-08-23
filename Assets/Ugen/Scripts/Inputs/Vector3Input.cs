using R3;
using UnityEngine;

namespace Ugen.Inputs
{
    [AddComponentMenu("Ugen/Ugen Vector3 Input")]
    public sealed class Vector3Input : UgenInput<Vector3>
    {
        [SerializeField] UgenInput<float> _x;
        [SerializeField] UgenInput<float> _y;
        [SerializeField] UgenInput<float> _z;

        public override Observable<Vector3> AsObservable()
        {
            if (_x == null || _y == null || _z == null)
            {
                return Observable.Never<Vector3>();
            }
            return _x.AsObservable().CombineLatest(_y.AsObservable(), _z.AsObservable(), (x, y, z) => new Vector3(x, y, z));
        }
    }
}
