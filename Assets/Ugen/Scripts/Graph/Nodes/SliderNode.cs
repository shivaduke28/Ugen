using System;
using Ugen.Behaviours;

namespace Ugen.Graph.Nodes
{
    [Serializable]
    public sealed class SliderNode : UgenBehaviourNode
    {
        public override Type BehaviourType => typeof(UgenSlider);
        public override string NodeName => "Slider";

        protected override void InitializePorts()
        {
            AddOutputPort("value", typeof(float));
        }
    }
}
