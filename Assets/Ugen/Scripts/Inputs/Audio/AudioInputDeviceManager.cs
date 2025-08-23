using System.Collections.Generic;
using System.Linq;
using Lasp;
using R3;
using UnityEngine;
using UnityEngine.Assertions;

namespace Ugen.Inputs.Audio
{
    [AddComponentMenu("Ugen/Ugen Audio Input Device Manager")]
    public sealed class AudioInputDeviceManager : MonoBehaviour
    {
        readonly ReactiveProperty<AudioInputDeviceInfo> _currentInputDevice = new(AudioInputDeviceInfo.Empty);
        public ReadOnlyReactiveProperty<AudioInputDeviceInfo> CurrentInputDevice => _currentInputDevice;

        const string PrefsKey = "Ugen_AudioInputDeviceId";

        public AudioInputStream InputStream { get; private set; }

        void Start()
        {
            // TODO: UIからデバイスを選ぶ
            var device = GetInputDevices().First(x => x.Name.Contains("Stream"));
            SwitchDevice(device);
        }

        public IEnumerable<AudioInputDeviceInfo> GetInputDevices()
        {
            return AudioSystem.InputDevices.Select(d => new AudioInputDeviceInfo(d.Name, d.ID));
        }

        public void SwitchDevice(AudioInputDeviceInfo audioInputDeviceInfo)
        {
            Assert.IsTrue(audioInputDeviceInfo.IsValid);
            var deviceDescriptor = AudioSystem.GetInputDevice(audioInputDeviceInfo.Id);
            if (!deviceDescriptor.IsValid)
            {
                Debug.LogError($"Invalid device: {audioInputDeviceInfo.Name}");
                return;
            }

            if (InputStream != null)
            {
                InputStream.Dispose();
                InputStream = null;
            }

            // NOTE: ステレオの場合、Lしか見てないのに注意
            InputStream = AudioInputStream.Create(audioInputDeviceInfo, 0, transform);
            _currentInputDevice.Value = audioInputDeviceInfo;
            PlayerPrefs.SetString(PrefsKey, audioInputDeviceInfo.Id);
            Debug.Log($"[Ugen] Audio input device changed: {audioInputDeviceInfo.Name}");
        }

        public void Clear()
        {
            if (InputStream != null)
            {
                InputStream.Dispose();
                InputStream = null;
            }

            _currentInputDevice.Value = AudioInputDeviceInfo.Empty;
            PlayerPrefs.DeleteKey(PrefsKey);
        }

        public void ReloadLastDevice()
        {
            var lastDeviceId = PlayerPrefs.GetString(PrefsKey, "");
            if (string.IsNullOrEmpty(lastDeviceId)) return;
            try
            {
                var descriptor = AudioSystem.GetInputDevice(lastDeviceId);
                if (descriptor.IsValid)
                {
                    SwitchDevice(new AudioInputDeviceInfo(descriptor.Name, descriptor.ID));
                }
            }
            catch
            {
                PlayerPrefs.DeleteKey(PrefsKey);
                throw;
            }
        }
    }
}
