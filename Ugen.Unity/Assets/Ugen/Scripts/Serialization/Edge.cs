using System;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public sealed class Edge
    {
        [SerializeField] string id;
        [SerializeField] string inputNodeId;
        [SerializeField] string inputPortId;
        [SerializeField] string outputNodeId;
        [SerializeField] string outputPortId;
    }
}