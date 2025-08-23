using R3;
using UnityEngine;

namespace Ugen.Inputs.Audio
{
    [AddComponentMenu("Ugen/Ugen Audio Level Mid Input")]
    public class AudioLevelMidInput : UgenInput<float>
    {
        [SerializeField] AudioInputManager _manager;

        public override Observable<float> AsObservable()
        {
            return _manager.Mid;
        }
    }
}
