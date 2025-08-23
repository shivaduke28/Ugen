using R3;

namespace Ugen.Inputs
{
    public sealed class UpdateInput : UgenInput<Unit>
    {
        public override Observable<Unit> AsObservable()
        {
            return Observable.EveryUpdate();
        }
    }
}
