using R3;
using UnityEngine.UIElements;

namespace Ugen.Graphs.Ports
{
    [UxmlElement("port-picker")]
    public sealed partial class PortPickerView : VisualElement
    {
        readonly Subject<PortData> _onEdgeCreationRequested = new();
        public Observable<PortData> OnEdgeCreationRequested() => _onEdgeCreationRequested;
        const string ClassName = "ugen-port-picker";

        public PortPickerView()
        {
            AddToClassList(ClassName);
        }

        public void TryCreateEdge(PortData request)
        {
            _onEdgeCreationRequested.OnNext(request);
        }
    }
}
