using R3;
using Ugen.Inputs.Bindings;
using UnityEngine;

namespace Ugen.Binders
{
    [AddComponentMenu("Ugen/Ugen Transform Binder")]
    public sealed class TransformBinder : MonoBehaviour
    {
        [SerializeField] Vector3Binding _position;
        [SerializeField] Vector3Binding _rotation;
        [SerializeField] Vector3Binding _scale;

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
