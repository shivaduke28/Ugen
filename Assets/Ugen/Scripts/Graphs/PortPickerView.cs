using R3;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    [UxmlElement("port-picker")]
    public sealed partial class PortPickerView : VisualElement
    {
        readonly Subject<EdgeCreationRequest> _onEdgeCreationRequested = new();
        public Observable<EdgeCreationRequest> OnEdgeCreationRequested() => _onEdgeCreationRequested;
        const string ClassName = "ugen-port-picker";

        public PortPickerView()
        {
            AddToClassList(ClassName);
        }

        public void TryCreateEdge(EdgeCreationRequest request)
        {
            _onEdgeCreationRequested.OnNext(request);
        }
    }
}
