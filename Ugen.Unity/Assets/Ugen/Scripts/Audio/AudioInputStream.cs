using System;
using Lasp;
using UnityEngine;

namespace Ugen.Audio
{
    public sealed class AudioInputStream : IDisposable
    {
        readonly AudioInputDeviceInfo audioInputDeviceInfo;
        readonly int channel;
        readonly InputStream stream;
        readonly GameObject gameObject;

        // Audio Level Trackers
        readonly AudioLevelTracker levelTracker;
        readonly AudioLevelTracker lowTracker;
        readonly AudioLevelTracker midTracker;
        readonly AudioLevelTracker highTracker;

        public InputStream Stream => stream;
        public AudioInputDeviceInfo DeviceInfo => audioInputDeviceInfo;
        public int Channel => channel;

        public bool IsValid => stream is { IsValid: true };

        // Level properties
        public float Level => levelTracker.normalizedLevel;
        public float Low => lowTracker.normalizedLevel;
        public float Mid => midTracker.normalizedLevel;
        public float High => highTracker.normalizedLevel;

        public bool AutoGain
        {
            get => levelTracker.autoGain;
            set
            {
                levelTracker.autoGain = value;
                lowTracker.autoGain = value;
                midTracker.autoGain = value;
                highTracker.autoGain = value;
            }
        }

        public bool SmoothFall
        {
            get => levelTracker.smoothFall;
            set
            {
                levelTracker.smoothFall = value;
                lowTracker.smoothFall = value;
                midTracker.smoothFall = value;
                highTracker.smoothFall = value;
            }
        }

        AudioInputStream(AudioInputDeviceInfo deviceInfo, int channelIndex, GameObject gameObject,
            AudioLevelTracker levelTracker, AudioLevelTracker lowTracker,
            AudioLevelTracker midTracker, AudioLevelTracker highTracker)
        {
            this.audioInputDeviceInfo = deviceInfo;
            this.channel = channelIndex;
            this.gameObject = gameObject;
            this.levelTracker = levelTracker;
            this.lowTracker = lowTracker;
            this.midTracker = midTracker;
            this.highTracker = highTracker;
            this.stream = AudioSystem.GetInputStream(deviceInfo.Id);
        }

        public static AudioInputStream Create(AudioInputDeviceInfo deviceInfo, int channel, Transform parent)
        {
            var go = new GameObject($"AudioInputStream_{deviceInfo.Name}_Ch{channel}");
            go.transform.SetParent(parent);

            var stream = AudioSystem.GetInputStream(deviceInfo.Id);
            if (stream == null || !stream.IsValid)
            {
                Debug.LogError($"Failed to create audio input stream for device: {deviceInfo.Name}");
                UnityEngine.Object.Destroy(go);
                return null;
            }

            // Create audio level trackers
            var levelTracker = CreateLevelTracker(go.transform, "LevelTracker", deviceInfo.Id, channel, FilterType.Bypass);
            var lowTracker = CreateLevelTracker(go.transform, "LowTracker", deviceInfo.Id, channel, FilterType.LowPass);
            var midTracker = CreateLevelTracker(go.transform, "MidTracker", deviceInfo.Id, channel, FilterType.BandPass);
            var highTracker = CreateLevelTracker(go.transform, "HighTracker", deviceInfo.Id, channel, FilterType.HighPass);

            return new AudioInputStream(deviceInfo, channel, go, levelTracker, lowTracker, midTracker, highTracker);
        }

        static AudioLevelTracker CreateLevelTracker(Transform parent, string name, string deviceId, int channel, FilterType filterType)
        {
            var trackerObject = new GameObject(name);
            trackerObject.transform.SetParent(parent);
            var tracker = trackerObject.AddComponent<AudioLevelTracker>();
            tracker.deviceID = deviceId;
            tracker.channel = channel;
            tracker.filterType = filterType;
            tracker.autoGain = true;
            tracker.smoothFall = true;
            return tracker;
        }

        public void Dispose()
        {
            if (gameObject != null)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}
