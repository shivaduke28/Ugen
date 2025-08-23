using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Ugen.Inputs.UI
{
    [RequireComponent(typeof(Toggle))]
    [AddComponentMenu("Ugen/Ugen Toggle Input")]
    public sealed class ToggleInput : UgenInput<bool>
    {
        [SerializeField] Toggle _toggle;

        void Reset()
        {
            _toggle = GetComponent<Toggle>();
        }

        public override Observable<bool> AsObservable() => _toggle.OnValueChangedAsObservable();
    }
}
