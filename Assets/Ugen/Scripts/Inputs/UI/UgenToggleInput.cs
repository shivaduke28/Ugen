using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Ugen.Inputs.UI
{
    [RequireComponent(typeof(Toggle))]
    public sealed class UgenToggleInput : UgenInput<bool>
    {
        [SerializeField] Toggle _toggle;

        void Reset()
        {
            _toggle = GetComponent<Toggle>();
        }

        public override Observable<bool> Observable() => _toggle.OnValueChangedAsObservable();
    }
}
