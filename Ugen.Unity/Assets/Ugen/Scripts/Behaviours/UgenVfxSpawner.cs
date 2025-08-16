using R3;
using Ugen.Attributes;
using Ugen.Graph;
using UnityEngine;
using UnityEngine.VFX;

namespace Ugen.Behaviours
{
    [UgenBehaviour]
    public class UgenVfxSpawner : UgenBehaviour
    {
        [SerializeField] VisualEffect _visualEffect;

        [SerializeField, UgenInput(0, "spawn")]
        SerializableUgenInputSubject<Unit> _spawnInput = new();

        protected override void InitializePorts()
        {
            RegisterInput(_spawnInput);
        }

        void Start()
        {
            _spawnInput.Observable.Subscribe(_ => _visualEffect.SendEvent("OnPlay")).AddTo(this);
        }
    }
}
