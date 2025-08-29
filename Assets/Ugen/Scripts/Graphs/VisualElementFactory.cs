using System;
using UnityEngine.UIElements;

namespace Ugen.Graphs
{
    public sealed class VisualElementFactory
    {
        readonly VisualTreeAssetSettings _settings;

        static VisualElementFactory _instance;

        public static VisualElementFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("VisualElementFactory is not initialized. ");
                }

                return _instance;
            }
        }

        public static void Initialize(VisualTreeAssetSettings settings)
        {
            _instance = new VisualElementFactory(settings);
        }

        VisualElementFactory(VisualTreeAssetSettings settings)
        {
            _settings = settings;
        }


        public VisualElement CreateNode()
        {
            var ve = new VisualElement();
            _settings.Node.CloneTree(ve);
            return ve;
        }

        public VisualElement CreateInputPort()
        {
            var ve = new VisualElement();
            _settings.InputPort.CloneTree(ve);
            return ve;
        }

        public VisualElement CreateOutputPort()
        {
            var ve = new VisualElement();
            _settings.OutputPort.CloneTree(ve);
            return ve;
        }
    }
}
