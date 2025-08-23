using R3;
using UnityEngine;

namespace Ugen.Inputs
{
    public abstract class UgenInput<T> : MonoBehaviour
    {
        public abstract Observable<T> AsObservable();
    }
}
