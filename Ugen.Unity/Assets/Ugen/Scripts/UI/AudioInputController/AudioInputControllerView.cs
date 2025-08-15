using System;
using R3;
using UnityEngine.UIElements;

namespace Ugen.UI.AudioInputController
{
    public sealed class AudioInputControllerView
    {
        readonly DropdownField deviceDropdown;
        readonly UgenButton reloadButton;
        readonly UgenButton clearButton;
        readonly Label statusLabel;
        readonly Label deviceInfoLabel;
        readonly ProgressBar levelBar;

        public AudioInputControllerView(VisualElement root)
        {
            deviceDropdown = root.Q<DropdownField>("device-dropdown");
            reloadButton = root.Q<UgenButton>("reload-button");
            clearButton = root.Q<UgenButton>("clear-button");
            statusLabel = root.Q<Label>("status-label");
            deviceInfoLabel = root.Q<Label>("device-info-label");
            levelBar = root.Q<ProgressBar>("level-bar");
        }

        public IDisposable Bind(AudioInputControllerViewModel viewModel)
        {
            return new CompositeDisposable(
                // Bind dropdown choices and selection
                viewModel.DeviceNames.Subscribe(names => deviceDropdown.choices = names),
                viewModel.SelectedDeviceIndex.Subscribe(index => deviceDropdown.index = index),
                viewModel.IsDropdownEnabled.Subscribe(enabled => deviceDropdown.SetEnabled(enabled)),

                // Bind dropdown selection changes back to ViewModel
                deviceDropdown.OnValueChangeAsObservable()
                    .Subscribe(evt =>
                    {
                        if (!string.IsNullOrEmpty(evt.newValue) && evt.newValue != "No devices found")
                        {
                            viewModel.SelectedDeviceIndex.Value = deviceDropdown.index;
                        }
                    }),

                // Bind buttons
                reloadButton.Bind(viewModel.ReloadButtonState),
                clearButton.Bind(viewModel.ClearButtonState),

                // Bind status
                viewModel.StatusText.Subscribe(text => statusLabel.text = text),
                viewModel.StatusColor.Subscribe(color => statusLabel.style.color = color),
                viewModel.DeviceInfoText.Subscribe(text => deviceInfoLabel.text = text),

                // Bind audio level
                viewModel.AudioLevel.Subscribe(level => levelBar.value = level)
            );
        }
    }
}
