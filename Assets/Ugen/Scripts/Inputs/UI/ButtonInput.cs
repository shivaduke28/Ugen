using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Ugen.Inputs.UI
{
    [RequireComponent(typeof(Button))]
    [AddComponentMenu("Ugen/Ugen Button Input")]
    public sealed class ButtonInput : UgenInput<Unit>
    {
        [SerializeField] Button _button;

        void Reset()
        {
            _button = GetComponent<Button>();
        }

        public override Observable<Unit> AsObservable() => _button.OnClickAsObservable();
    }
}
