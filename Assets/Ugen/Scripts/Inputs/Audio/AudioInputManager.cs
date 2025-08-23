using R3;
using UnityEngine;

namespace Ugen.Inputs.Audio
{
    [AddComponentMenu("Ugen/Ugen Audio Input Manager")]
    public sealed class AudioInputManager : MonoBehaviour
    {
        [SerializeField] AudioInputDeviceManager _deviceManager;
        readonly ReactiveProperty<float> _low = new(0f);
        readonly ReactiveProperty<float> _mid = new(0f);
        readonly ReactiveProperty<float> _high = new(0f);
        readonly ReactiveProperty<float> _level = new(0f);
        public Observable<float> Low => _low;
        public Observable<float> Mid => _mid;
        public Observable<float> High => _high;
        public Observable<float> Level => _level;

        void LateUpdate()
        {
            var stream = _deviceManager.InputStream;
            if (stream == null)
            {
                _low.Value = 0f;
                _mid.Value = 0f;
                _high.Value = 0f;
                _level.Value = 0f;
                return;
            }

            _low.Value = stream.LevelLow;
            _mid.Value = stream.LevelMid;
            _high.Value = stream.LevelHigh;
            _level.Value = stream.Level;
        }
    }
}
