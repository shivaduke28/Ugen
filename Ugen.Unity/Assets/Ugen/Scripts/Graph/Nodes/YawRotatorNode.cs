using System;
using R3;
using Ugen.Behaviours;
using UnityEngine;

namespace Ugen.Graph.Nodes
{
    [Serializable]
    public sealed class YawRotatorNode : UgenBehaviourNode<UgenYawRotator>, IInitializable, IDisposable
    {
        public override string NodeName => "Yaw Rotator";

        readonly UgenInput<float> speedInput;
        IDisposable disposable;

        public YawRotatorNode()
        {
            speedInput = new UgenInput<float>("speed", 0, 1f);
            AddInputPort(speedInput);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }

        public void Initialize()
        {
            if (behaviour != null && behaviour.GetInput(0) is UgenInput<float> floatInput)
            {
                disposable = speedInput.Observable.Subscribe(value => floatInput.Send(value));
            }
            else
            {
                Debug.LogWarning("behaviour is null");
            }
        }
    }
}
