using System;
using R3;
using Ugen.Behaviours;
using UnityEngine;

namespace Ugen.Graph.Nodes
{
    [Serializable]
    [UgenNode]
    public sealed partial class SliderNode : UgenBehaviourNode<UgenSlider>, IInitializable, IDisposable
    {
        public override string NodeName => "Slider";

        readonly Subject<float> subject = new();
        readonly UgenOutput<float> output;
        IDisposable disposable;

        public SliderNode()
        {
            output = new UgenOutput<float>("value", 0, subject);
            AddOutputPort(output);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }

        public void Initialize()
        {
            if (behaviour == null)
            {
                Debug.LogWarning("behaviour is null");
                return;
            }
            if (behaviour != null && behaviour.GetOutput(0) is UgenOutput<float> floatOutput)
            {
                disposable = floatOutput.Observable
                    .Do(x => Debug.Log($"slider value: {x}"))
                    .Subscribe(x => subject.OnNext(x));
            }
        }
    }
}
