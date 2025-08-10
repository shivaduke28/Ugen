using System;
using Ugen.Behaviours;

namespace Ugen.Graph.Nodes
{
    [Serializable]
    public sealed class YawRotatorNode : UgenNode
    {
        public override Type BehaviourType => typeof(UgenYawRotator);
        public override string NodeName => "Yaw Rotator";

        protected override void InitializePorts()
        {
            AddInputPort("speed", typeof(float));
        }
    }
}