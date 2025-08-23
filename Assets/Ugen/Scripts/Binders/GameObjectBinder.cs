using R3;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Binders
{
    [AddComponentMenu("Ugen/Ugen GameObject Binder")]
    public sealed class GameObjectBinder : MonoBehaviour
    {
        [SerializeField] UgenInput<bool> _active;

        void Start()
        {
            _active.Observable().Subscribe(x => gameObject.SetActive(x))
                .AddTo(this);
        }
    }
}
