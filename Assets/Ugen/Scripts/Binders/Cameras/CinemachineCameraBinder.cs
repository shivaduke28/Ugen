using R3;
using Ugen.Bindings;
using Unity.Cinemachine;
using UnityEngine;

namespace Ugen.Binders
{
    [RequireComponent(typeof(CinemachineCamera))]
    [AddComponentMenu("Ugen/Ugen Cinemachine Camera Binder")]
    public sealed class CinemachineCameraBinder : MonoBehaviour
    {
        [SerializeField] CinemachineCamera _camera;
        [SerializeField] FloatBinding _fov;
        [SerializeField] FloatBinding _dutch;
        [SerializeField] UnitBinding _switch;
        [SerializeField] int _activePriority = 1;
        [SerializeField] int _inactivePriority = 0;

        public Observable<Unit> OnSwitchRequested() => _switch.AsObservable();

        void Start()
        {
            _fov.AsObservable()
                .Subscribe(x => _camera.Lens.FieldOfView = x)
                .AddTo(this);
            _dutch.AsObservable().Subscribe(x => _camera.Lens.Dutch = x).AddTo(this);
        }

        public void Switch(bool active)
        {
            _camera.Priority.Value = active ? _activePriority : _inactivePriority;
        }

        void Reset()
        {
            _camera = GetComponent<CinemachineCamera>();
            _camera.Priority.Enabled = true;
        }
    }
}
