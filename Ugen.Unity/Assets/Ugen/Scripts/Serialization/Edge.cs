using System;
using UnityEngine;

public class ExampleAttribute : Attribute
{
}

namespace Ugen.Serialization
{
    [Serializable, Example]
    public sealed partial class Edge
    {
        [SerializeField] string id;
        [SerializeField] string inputNodeId;
        [SerializeField] string inputPortId;
        [SerializeField] string outputNodeId;
        [SerializeField] string outputPortId;
    }
}
