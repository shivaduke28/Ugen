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

        public override Observable<Vector3> Observable()
        {
            if (_x == null || _y == null || _z == null)
            {
                return R3.Observable.Never<Vector3>();
            }
            return _x.Observable().CombineLatest(_y.Observable(), _z.Observable(), (x, y, z) => new Vector3(x, y, z));
        }
    }
}
