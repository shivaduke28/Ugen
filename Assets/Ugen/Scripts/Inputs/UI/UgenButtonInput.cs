using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Ugen.Inputs.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class UgenButtonInput : UgenInput<Unit>
    {
        [SerializeField] Button _button;

        void Reset()
        {
            _button = GetComponent<Button>();
        }

        public override Observable<Unit> Observable() => _button.OnClickAsObservable();
    }
}
