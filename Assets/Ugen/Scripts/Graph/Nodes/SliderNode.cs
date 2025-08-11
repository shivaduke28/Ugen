using System;
using Ugen.Behaviours;

namespace Ugen.Graph.Nodes
{
    [Serializable]
    public sealed class SliderNode : UgenBehaviourNode<UgenSlider>
    {
        public override string NodeName => "Slider";

        protected override void InitializePorts()
        {
            AddOutputPort("value", typeof(float));
        }
    }
}
