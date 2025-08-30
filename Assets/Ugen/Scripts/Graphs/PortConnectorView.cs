using R3;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    [UxmlElement("port-connector")]
    public sealed partial class PortConnectorView : VisualElement
    {
        readonly Subject<EdgeCreationRequest> _onEdgeCreationRequested = new();
        public Observable<EdgeCreationRequest> OnEdgeCreationRequested() => _onEdgeCreationRequested;
        const string ClassName = "ugen-port-connector";

        public PortConnectorView()
        {
            AddToClassList(ClassName);
        }

        public void TryCreateEdge(EdgeCreationRequest request)
        {
            _onEdgeCreationRequested.OnNext(request);
        }
    }
}
