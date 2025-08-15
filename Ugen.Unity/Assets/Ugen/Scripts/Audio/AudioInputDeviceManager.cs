using System.Collections.Generic;
using System.Linq;
using Lasp;
using R3;
using UnityEngine;
using UnityEngine.Assertions;

namespace Ugen.Audio
{
    public sealed class AudioInputDeviceManager
    {
        readonly ReactiveProperty<AudioInputDeviceInfo> currentInputDevice = new(AudioInputDeviceInfo.Empty);
        public ReadOnlyReactiveProperty<AudioInputDeviceInfo> CurrentInputDevice => currentInputDevice;

        readonly Transform audioInputStreamParent;

        const string PrefsKey = "Ugen_AudioInputDevice";

        public AudioInputStream InputStream { get; private set; }

        public AudioInputDeviceManager(Transform audioInputStreamParent)
        {
            this.audioInputStreamParent = audioInputStreamParent;
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
            InputStream = AudioInputStream.Create(audioInputDeviceInfo, 0, audioInputStreamParent);

            if (InputStream == null)
            {
                Debug.LogError($"Failed to create AudioInputStream for device: {audioInputDeviceInfo.Name}");
                return;
            }

            currentInputDevice.Value = audioInputDeviceInfo;
            PlayerPrefs.SetString(PrefsKey, audioInputDeviceInfo.Id);
            Debug.Log($"[Ugen] Switched audio input device: {audioInputDeviceInfo.Name} ({audioInputDeviceInfo.Id})");
        }

        public void Clear()
        {
            if (InputStream != null)
            {
                InputStream.Dispose();
                InputStream = null;
            }

            currentInputDevice.Value = AudioInputDeviceInfo.Empty;
        }

        public void ReloadLastDevice()
        {
            var lastDeviceId = PlayerPrefs.GetString(PrefsKey, "");
            Debug.Log($"[Ugen] Reloading last audio input device: {lastDeviceId}");
            if (string.IsNullOrEmpty(lastDeviceId)) return;
            try
            {
                var descriptor = AudioSystem.GetInputDevice(lastDeviceId);
                if (descriptor.IsValid) SwitchDevice(new AudioInputDeviceInfo(descriptor.Name, descriptor.ID));
            }
            catch
            {
                PlayerPrefs.DeleteKey(PrefsKey);
                throw;
            }
        }
    }
}