using R3;
using UnityEngine;

namespace Ugen.Inputs
{
    [AddComponentMenu("Ugen/Ugen Color Input")]
    public sealed class ColorInput : UgenInput<Color>
    {
        [SerializeField] UgenInput<float> _r;
        [SerializeField] UgenInput<float> _g;
        [SerializeField] UgenInput<float> _b;
        [SerializeField] UgenInput<float> _a;

        public override Observable<Color> Observable()
        {
            if (_r == null || _g == null || _b == null || _a == null)
            {
                return R3.Observable.Never<Color>();
            }
            return _r.Observable().CombineLatest(_g.Observable(), _b.Observable(), _a.Observable(),
                (r, g, b, a) => new Color(r, g, b, a));
        }
    }
}
