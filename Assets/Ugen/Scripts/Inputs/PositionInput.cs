using R3;
using UnityEngine;

namespace Ugen.Inputs
{
    [AddComponentMenu("Ugen/Ugen Position Input")]
    public sealed class PositionInput : UgenInput<Vector3>
    {
        public override Observable<Vector3> AsObservable()
        {
            return Observable.EveryValueChanged(transform, x => x.position);
        }
    }
}
