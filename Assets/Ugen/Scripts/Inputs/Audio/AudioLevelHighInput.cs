using R3;
using UnityEngine;

namespace Ugen.Inputs.Audio
{
    [AddComponentMenu("Ugen/Ugen Audio Level High Input")]
    public sealed class AudioLevelHighInput : UgenInput<float>
    {
        [SerializeField] AudioInputManager _manager;

        public override Observable<float> AsObservable()
        {
            return _manager.High;
        }
    }
}
