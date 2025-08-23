using R3;
using UnityEngine;

namespace Ugen.Inputs
{
    public abstract class UgenInput : MonoBehaviour
    {
    }

    public abstract class UgenInput<T> : UgenInput
    {
        public abstract Observable<T> AsObservable();
    }
}
