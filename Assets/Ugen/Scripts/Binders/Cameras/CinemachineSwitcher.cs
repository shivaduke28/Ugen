using System.Collections.Generic;
using System.Linq;
using R3;
using Ugen.Bindings;
using UnityEngine;

namespace Ugen.Binders
{
    [AddComponentMenu("Ugen/Ugen Cinemachine Switcher")]
    public sealed class CinemachineSwitcher : MonoBehaviour
    {
        [SerializeField] List<CinemachinePriorityBinder> _binders = new();
        [SerializeField] UnitBinding _step;
        int _index;

        void Start()
        {
            _binders
                .Select((b, i) => b.OnSwitchRequested().Select(_ => i))
                .Merge().Subscribe(Switch)
                .AddTo(this);

            _step?.AsObservable()
                .Subscribe(_ => Switch((_index + 1) % _binders.Count))
                .AddTo(this);

            if (_binders.Any())
            {
                Switch(0);
            }
        }

        void Switch(int index)
        {
            for (var i = 0; i < _binders.Count; i++)
            {
                _binders[i].Switch(i == index);
            }

            _index = index;
        }
    }
}
