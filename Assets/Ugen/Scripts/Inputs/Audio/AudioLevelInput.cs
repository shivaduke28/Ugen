using R3;
using UnityEngine;

namespace Ugen.Inputs.Audio
{
    [AddComponentMenu("Ugen/Ugen Audio Level Input")]
    public sealed class AudioLevelInput : UgenInput<float>
    {
        [SerializeField] AudioInputManager _manager;

        public override Observable<float> AsObservable()
        {
            return _manager.Level;
        }
    }
}
