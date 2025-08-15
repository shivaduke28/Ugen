using System;
using System.Collections.Generic;
using Ugen.UI.AudioInputController;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen
{
    public sealed class UgenInstaller : MonoBehaviour
    {
        [SerializeField] UIDocument uiDocument;
        [SerializeField] Transform audioInputStreamParent;

        public Audio.AudioInputDeviceManager AudioInputDeviceManager { get; private set; }

        AudioInputControllerView audioInputControllerView;
        AudioInputControllerViewModel audioInputControllerViewModel;
        IDisposable audioInputControllerBinding;

        readonly List<IInitializable> initializables = new();
        readonly List<IDisposable> disposables = new();

        void Awake()
        {
            if (audioInputStreamParent == null)
            {
                Debug.LogError("[UgenInstaller] AudioInputStreamParent is not set!");
                return;
            }

            if (uiDocument == null)
            {
                Debug.LogError("[UgenInstaller] UIDocument is not set!");
                return;
            }

            // Initialize AudioInputDeviceManager
            AudioInputDeviceManager = new Audio.AudioInputDeviceManager(audioInputStreamParent);
            Debug.Log("[UgenInstaller] AudioInputDeviceManager initialized");

            // Initialize AudioInputController UI
            var audioInputControllerRoot = uiDocument.rootVisualElement.Q<VisualElement>("audio-input-controller");
            if (audioInputControllerRoot != null)
            {
                audioInputControllerView = new AudioInputControllerView(audioInputControllerRoot);
                audioInputControllerViewModel = Register(new AudioInputControllerViewModel(AudioInputDeviceManager));
                audioInputControllerBinding = Register(audioInputControllerView.Bind(audioInputControllerViewModel));
                Debug.Log("[UgenInstaller] AudioInputController UI initialized");
            }
            else
            {
                Debug.LogWarning("[UgenInstaller] AudioInputController element not found in UIDocument");
            }

            // Initialize all registered initializables
            foreach (var initializable in initializables) initializable.Initialize();
        }

        T Register<T>(T instance)
        {
            if (instance is IDisposable disposable) disposables.Add(disposable);

            if (instance is IInitializable initializable) initializables.Add(initializable);

            return instance;
        }

        void OnDestroy()
        {
            // Dispose all registered disposables
            foreach (var disposable in disposables) disposable?.Dispose();

            disposables.Clear();
        }
    }
}