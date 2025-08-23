using R3;
using UnityEngine;

namespace Ugen.Inputs.Audio
{
    [AddComponentMenu("Ugen/Ugen Audio Level Low Input")]
    public sealed class AudioLevelLowInput : UgenInput<float>
    {
        [SerializeField] AudioInputManager _manager;

        public override Observable<float> AsObservable()
        {
            return _manager.Low;
        }
    }
}
