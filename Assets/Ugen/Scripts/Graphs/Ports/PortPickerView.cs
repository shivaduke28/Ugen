using UnityEngine.UIElements;

namespace Ugen.Graphs.Ports
{
    [UxmlElement("port-picker")]
    public sealed partial class PortPickerView : VisualElement
    {
        const string ClassName = "ugen-port-picker";
        public PortData? PortData { get; set; }

        public PortPickerView()
        {
            AddToClassList(ClassName);
        }
    }
}
