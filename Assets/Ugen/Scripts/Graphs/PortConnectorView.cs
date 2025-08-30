using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    [UxmlElement("port-connector")]
    public sealed partial class PortConnectorView : VisualElement
    {
        const string ClassName = "ugen-port-connector";

        public Observable<Vector2> OnCenterWorldPositionChanged()
            => Observable.EveryValueChanged(this, x => x.worldBound.center);

        public PortConnectorView()
        {
            AddToClassList(ClassName);
        }
    }
}
