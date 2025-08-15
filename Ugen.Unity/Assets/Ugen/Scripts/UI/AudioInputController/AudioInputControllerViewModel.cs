using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Ugen.Audio;
using UnityEngine;

namespace Ugen.UI.AudioInputController
{
    public sealed class AudioInputControllerViewModel : IDisposable, IInitializable
    {
        readonly AudioInputDeviceManager deviceManager;

        // Observable properties
        public ReactiveProperty<List<string>> DeviceNames { get; } = new(new List<string>());
        public ReactiveProperty<int> SelectedDeviceIndex { get; } = new(-1);
        public ReactiveProperty<string> StatusText { get; } = new("Status: Not Connected");
        public ReactiveProperty<Color> StatusColor { get; } = new(Color.red);
        public ReactiveProperty<string> DeviceInfoText { get; } = new("Device: None");
        public ReactiveProperty<float> AudioLevel { get; } = new(0f);
        public ReactiveProperty<bool> IsDropdownEnabled { get; } = new(false);

        // Button states
        public UgenButtonState ReloadButtonState { get; }
        public UgenButtonState ClearButtonState { get; }

        readonly CompositeDisposable disposables = new();
        IDisposable updateSubscription;

        public AudioInputControllerViewModel(AudioInputDeviceManager deviceManager)
        {
            this.deviceManager = deviceManager;

            ReloadButtonState = new UgenButtonState("Reload", OnReloadClicked);
            ClearButtonState = new UgenButtonState("Clear", OnClearClicked);
        }

        public void Initialize()
        {
            // Subscribe to device changes
            disposables.Add(deviceManager.CurrentInputDevice.Subscribe(OnCurrentDeviceChanged));

            // Subscribe to selected index changes
            disposables.Add(SelectedDeviceIndex.Subscribe(OnDeviceSelectionChanged));

            // Initial setup
            RefreshDeviceList();
            deviceManager.ReloadLastDevice();

            // Start update loop for audio level
            StartAudioLevelUpdate();
        }

        void RefreshDeviceList()
        {
            var devices = deviceManager.GetInputDevices().ToList();

            if (devices.Count == 0)
            {
                DeviceNames.Value = new List<string> { "No devices found", };
                IsDropdownEnabled.Value = false;
            }
            else
            {
                DeviceNames.Value = devices.Select(d => d.Name).ToList();
                IsDropdownEnabled.Value = true;

                // Select current device if any
                var currentDevice = deviceManager.CurrentInputDevice.CurrentValue;
                if (currentDevice.IsValid)
                {
                    var index = devices.FindIndex(d => d.Id == currentDevice.Id);
                    if (index >= 0) SelectedDeviceIndex.Value = index;
                }
            }
        }

        void OnReloadClicked()
        {
            RefreshDeviceList();
        }

        void OnClearClicked()
        {
            deviceManager.Clear();
            SelectedDeviceIndex.Value = -1;
        }

        void OnDeviceSelectionChanged(int index)
        {
            if (index < 0 || !IsDropdownEnabled.Value) return;

            var devices = deviceManager.GetInputDevices().ToList();
            if (index < devices.Count)
            {
                var selectedDevice = devices[index];
                if (selectedDevice.IsValid) deviceManager.SwitchDevice(selectedDevice);
            }
        }

        void OnCurrentDeviceChanged(AudioInputDeviceInfo device)
        {
            if (device.IsValid)
            {
                StatusText.Value = "Status: Connected";
                StatusColor.Value = Color.green;
                DeviceInfoText.Value = $"Device: {device.Name}";

                // Update selected index in dropdown
                var devices = deviceManager.GetInputDevices().ToList();
                var index = devices.FindIndex(d => d.Id == device.Id);
                if (index >= 0 && index != SelectedDeviceIndex.CurrentValue) SelectedDeviceIndex.Value = index;
            }
            else
            {
                StatusText.Value = "Status: Not Connected";
                StatusColor.Value = Color.red;
                DeviceInfoText.Value = "Device: None";
                AudioLevel.Value = 0f;
            }
        }

        void StartAudioLevelUpdate()
        {
            updateSubscription?.Dispose();
            updateSubscription = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (deviceManager.InputStream != null && deviceManager.InputStream.IsValid)
                    {
                        // Use AudioLevelTracker's normalized level
                        var level = deviceManager.InputStream.Level;
                        AudioLevel.Value = Mathf.Clamp01(level);
                    }
                    else
                    {
                        AudioLevel.Value = 0f;
                    }
                });
            disposables.Add(updateSubscription);
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}