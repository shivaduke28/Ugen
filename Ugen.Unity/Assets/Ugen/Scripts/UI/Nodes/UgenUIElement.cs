using System.Collections.Generic;
using Ugen.Graph;
using UnityEngine.UIElements;

namespace Ugen.UI.Nodes
{
    public abstract class UgenUIElement : VisualElement
    {
        readonly List<IUgenInput> _inputs = new();
        readonly List<IUgenOutput> _outputs = new();

        public IReadOnlyList<IUgenInput> Inputs => _inputs;
        public IReadOnlyList<IUgenOutput> Outputs => _outputs;

        protected void RegisterInput(IUgenInput input)
        {
            _inputs.Add(input);
        }

        protected void RegisterOutput(IUgenOutput output)
        {
            _outputs.Add(output);
        }
    }
}
