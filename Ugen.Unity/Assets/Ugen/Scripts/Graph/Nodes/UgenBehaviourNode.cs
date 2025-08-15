using Ugen.Behaviours;

namespace Ugen.Graph.Nodes
{
    public sealed class UgenBehaviourNode : UgenNode
    {
        readonly UgenBehaviour behaviour;

        public UgenBehaviourNode(string nodeId, UgenBehaviour behaviour) : base(nodeId)
        {
            this.behaviour = behaviour;
            foreach (var input in behaviour.Inputs) AddInputPort(input);

            foreach (var output in behaviour.Outputs) AddOutputPort(output);
        }
    }
}