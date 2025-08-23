using R3;
using UnityEngine;

namespace Ugen.Inputs
{
    [AddComponentMenu("Ugen/Ugen Vector2 Input")]
    public sealed class Vector2Input : UgenInput<Vector2>
    {
        [SerializeField] UgenInput<float> _x;
        [SerializeField] UgenInput<float> _y;

        public override Observable<Vector2> Observable()
        {
            if (_x == null || _y == null)
            {
                return R3.Observable.Never<Vector2>();
            }

            return _x.Observable().CombineLatest(_y.Observable(), (x, y) => new Vector2(x, y));
        }
    }
}
