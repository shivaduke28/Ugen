using R3;
using Ugen.Inputs.Bindings;
using UnityEngine;

namespace Ugen.Binders
{
    [AddComponentMenu("Ugen/Ugen GameObject Binder")]
    public sealed class GameObjectBinder : MonoBehaviour
    {
        [SerializeField] BoolBinding _active;

        void Start()
        {
            _active.AsObservable().Subscribe(x => gameObject.SetActive(x))
                .AddTo(this);
        }
    }
}
