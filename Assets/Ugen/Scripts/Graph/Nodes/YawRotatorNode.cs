using System;
using Ugen.Behaviours;

namespace Ugen.Graph.Nodes
{
    [Serializable]
    public sealed class YawRotatorNode : UgenBehaviourNode<UgenYawRotator>
    {
        public override string NodeName => "Yaw Rotator";

        protected override void InitializePorts()
        {
            AddInputPort("speed", typeof(float));
        }
    }
}
