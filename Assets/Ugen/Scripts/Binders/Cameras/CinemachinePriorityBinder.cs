using R3;
using Ugen.Bindings;
using Unity.Cinemachine;
using UnityEngine;

namespace Ugen.Binders
{
    [RequireComponent(typeof(CinemachineCamera))]
    [AddComponentMenu("Ugen/Ugen Cinemachine Switch Binder")]
    public sealed class CinemachinePriorityBinder : MonoBehaviour
    {
        [SerializeField] CinemachineCamera _camera;
        [SerializeField] UnitBinding _binding;
        [SerializeField] int _activePriority = 1;
        [SerializeField] int _inactivePriority = 0;

        public Observable<Unit> OnSwitchRequested() => _binding.AsObservable();

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
