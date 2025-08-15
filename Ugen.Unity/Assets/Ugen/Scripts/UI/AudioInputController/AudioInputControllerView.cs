using System;
using R3;
using UnityEngine.UIElements;

namespace Ugen.UI.AudioInputController
{
    public sealed class AudioInputControllerView
    {
        readonly DropdownField _deviceDropdown;
        readonly UgenButton _reloadButton;
        readonly UgenButton _clearButton;
        readonly Label _statusLabel;
        readonly Label _deviceInfoLabel;
        readonly ProgressBar _levelBar;

        public AudioInputControllerView(VisualElement root)
        {
            _deviceDropdown = root.Q<DropdownField>("device-dropdown");
            _reloadButton = root.Q<UgenButton>("reload-button");
            _clearButton = root.Q<UgenButton>("clear-button");
            _statusLabel = root.Q<Label>("status-label");
            _deviceInfoLabel = root.Q<Label>("device-info-label");
            _levelBar = root.Q<ProgressBar>("level-bar");
        }

        public IDisposable Bind(AudioInputControllerViewModel viewModel)
        {
            return new CompositeDisposable(
                // Bind dropdown choices and selection
                viewModel.DeviceNames.Subscribe(names => _deviceDropdown.choices = names),
                viewModel.SelectedDeviceIndex.Subscribe(index => _deviceDropdown.index = index),
                viewModel.IsDropdownEnabled.Subscribe(enabled => _deviceDropdown.SetEnabled(enabled)),

                // Bind dropdown selection changes back to ViewModel
                _deviceDropdown.OnValueChangeAsObservable()
                    .Subscribe(evt =>
                    {
                        if (!string.IsNullOrEmpty(evt.newValue) && evt.newValue != "No devices found") viewModel.SelectedDeviceIndex.Value = _deviceDropdown.index;
                    }),

                // Bind buttons
                _reloadButton.Bind(viewModel.ReloadButtonState),
                _clearButton.Bind(viewModel.ClearButtonState),

                // Bind status
                viewModel.StatusText.Subscribe(text => _statusLabel.text = text),
                viewModel.StatusColor.Subscribe(color => _statusLabel.style.color = color),
                viewModel.DeviceInfoText.Subscribe(text => _deviceInfoLabel.text = text),

                // Bind audio level
                viewModel.AudioLevel.Subscribe(level => _levelBar.value = level)
            );
        }
    }
}
