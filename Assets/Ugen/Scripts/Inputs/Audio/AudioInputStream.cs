using System;
using Lasp;
using Unity.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ugen.Inputs.Audio
{
    /// <summary>
    /// Laspのwrapper
    /// </summary>
    public sealed class AudioInputStream : IDisposable
    {
        readonly AudioLevelTracker _levelTracker;
        readonly AudioLevelTracker _lowTracker;
        readonly AudioLevelTracker _midTracker;
        readonly AudioLevelTracker _highTracker;
        readonly SpectrumAnalyzer _spectrum;
        bool _disposed;

        public bool IsValid => !_disposed;

        public float Level => _levelTracker.normalizedLevel;
        public float LevelLow => _lowTracker.normalizedLevel;
        public float LevelMid => _midTracker.normalizedLevel;
        public float LevelHigh => _highTracker.normalizedLevel;

        public bool AutoGain
        {
            get => _levelTracker.autoGain;
            set
            {
                _levelTracker.autoGain = value;
                _lowTracker.autoGain = value;
                _midTracker.autoGain = value;
                _highTracker.autoGain = value;
            }
        }

        public bool SmoothFall
        {
            get => _levelTracker.smoothFall;
            set
            {
                _levelTracker.smoothFall = value;
                _lowTracker.smoothFall = value;
                _midTracker.smoothFall = value;
                _highTracker.smoothFall = value;
            }
        }

        /// <summary>
        /// 音声データ（波形）。マイフレームサイズが変わるので注意。
        /// </summary>
        public NativeSlice<float> AudioDataSlice => _levelTracker.audioDataSlice;

        /// <summary>
        /// スペクトルをログスケールしたもの。サイズは<see cref="SpectrumSize"/>で固定。
        /// 呼ぶたびにログスケールが実行されるので注意。
        /// </summary>
        public NativeArray<float> LogSpectrum => _spectrum.logSpectrumArray;

        public const int SpectrumSize = 512;


        AudioInputStream(AudioLevelTracker levelTracker,
            AudioLevelTracker lowTracker,
            AudioLevelTracker midTracker,
            AudioLevelTracker highTracker,
            SpectrumAnalyzer spectrum)
        {
            _levelTracker = levelTracker;
            _lowTracker = lowTracker;
            _midTracker = midTracker;
            _highTracker = highTracker;
            _spectrum = spectrum;
        }

        public static AudioInputStream Create(AudioInputDeviceInfo info, int channel, Transform parent)
        {
            var go = new GameObject(info.Name);
            go.transform.SetParent(parent, false);

            var levelTracker = go.AddComponent<AudioLevelTracker>();
            levelTracker.deviceID = info.Id;
            levelTracker.channel = channel;
            levelTracker.filterType = FilterType.Bypass;

            var lowTracker = go.AddComponent<AudioLevelTracker>();
            lowTracker.deviceID = info.Id;
            lowTracker.channel = channel;
            lowTracker.filterType = FilterType.LowPass;

            var midTracker = go.AddComponent<AudioLevelTracker>();
            midTracker.deviceID = info.Id;
            midTracker.channel = channel;
            midTracker.filterType = FilterType.BandPass;

            var highTracker = go.AddComponent<AudioLevelTracker>();
            highTracker.deviceID = info.Id;
            highTracker.channel = channel;
            highTracker.filterType = FilterType.HighPass;

            var spectrum = go.AddComponent<SpectrumAnalyzer>();
            spectrum.deviceID = info.Id;

            return new AudioInputStream(
                levelTracker,
                lowTracker,
                midTracker,
                highTracker,
                spectrum);
        }

        public void Dispose()
        {
            Object.Destroy(_levelTracker.gameObject);
            _disposed = true;
        }
    }
}
