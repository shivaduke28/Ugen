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

        public override Observable<Color> AsObservable()
        {
            if (_r == null || _g == null || _b == null || _a == null)
            {
                return Observable.Never<Color>();
            }
            return _r.AsObservable().CombineLatest(_g.AsObservable(), _b.AsObservable(), _a.AsObservable(),
                (r, g, b, a) => new Color(r, g, b, a));
        }
    }
}
