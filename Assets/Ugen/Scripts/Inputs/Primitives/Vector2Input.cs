using R3;
using UnityEngine;

namespace Ugen.Inputs.Primitives
{
    [AddComponentMenu("Ugen/Ugen Vector2 Input")]
    public sealed class Vector2Input : UgenInput<Vector2>
    {
        [SerializeField] UgenInput<float> _x;
        [SerializeField] UgenInput<float> _y;

        public override Observable<Vector2> AsObservable()
        {
            if (_x == null || _y == null)
            {
                return Observable.Never<Vector2>();
            }

            return _x.AsObservable().CombineLatest(_y.AsObservable(), (x, y) => new Vector2(x, y));
        }
    }
}
