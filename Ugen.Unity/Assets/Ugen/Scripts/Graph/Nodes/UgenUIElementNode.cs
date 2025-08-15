using Ugen.UI.Nodes;

namespace Ugen.Graph.Nodes
{
    public sealed class UgenUIElementNode : UgenNode
    {
        readonly UgenUIElement uiElement;

        public UgenUIElementNode(string nodeId, UgenUIElement uiElement) : base(nodeId)
        {
            this.uiElement = uiElement;
            foreach (var input in uiElement.Inputs) AddInputPort(input);

            foreach (var output in uiElement.Outputs) AddOutputPort(output);
        }
    }
}