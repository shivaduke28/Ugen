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

        [UgenInput(0, "spawn")] readonly UgenInputSubject<Unit> _spawnInput = new("spawn");

        protected override void InitializePorts()
        {
            RegisterInput(_spawnInput);
        }

        void Start()
        {
            _spawnInput.Observable.Subscribe(_ =>
            {
                Debug.Log("Spawning VFX");
                _visualEffect.SendEvent("OnPlay");
            }).AddTo(this);
        }
    }
}
