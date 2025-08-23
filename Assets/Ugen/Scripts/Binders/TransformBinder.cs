using R3;
using Ugen.Inputs;
using UnityEngine;

namespace Ugen.Binders
{
    [AddComponentMenu("Ugen/Ugen Transform Binder")]
    public sealed class TransformBinder : MonoBehaviour
    {
        [SerializeField] UgenInput<Vector3> _position;
        [SerializeField] UgenInput<Vector3> _rotation;
        [SerializeField] UgenInput<Vector3> _scale;

        void Start()
        {
            _position?.AsObservable()
                .Subscribe(x => transform.localPosition = x)
                .AddTo(this);
            _rotation?.AsObservable()
                .Subscribe(x => transform.localRotation = Quaternion.Euler(x))
                .AddTo(this);
            _scale?.AsObservable()
                .Subscribe(x => transform.localScale = x)
                .AddTo(this);
        }
    }
}
