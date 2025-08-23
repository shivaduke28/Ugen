using R3;
using Ugen.Inputs.Bindings;
using UnityEngine;

namespace Ugen.Inputs.Primitives
{
    [AddComponentMenu("Ugen/Ugen Color Input")]
    public sealed class ColorInput : UgenInput<Color>
    {
        [SerializeField] FloatBinding _r;
        [SerializeField] FloatBinding _g;
        [SerializeField] FloatBinding _b;
        [SerializeField] FloatBinding _a;

        public override Observable<Color> AsObservable()
        {
            var r = _r != null ? _r.AsObservable() : Observable.Return(0f);
            var g = _g != null ? _g.AsObservable() : Observable.Return(0f);
            var b = _b != null ? _b.AsObservable() : Observable.Return(0f);
            var a = _a != null ? _a.AsObservable() : Observable.Return(1f);
            return r.CombineLatest(g, b, a, (r1, g1, b1, a1) => new Color(r1, g1, b1, a1));
        }
    }
}
