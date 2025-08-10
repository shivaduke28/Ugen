using System;
using UnityEngine;
using Ugen.Graph.Nodes;

namespace Ugen.Graph
{
    [Serializable]
    public sealed class SerializedUgenNode
    {
        [SerializeField] string nodeId;
        [SerializeField] string nodeTypeName;
        [SerializeField] Vector2 position;

        public string NodeId
        {
            get => nodeId;
            set => nodeId = value;
        }

        public string NodeTypeName
        {
            get => nodeTypeName;
            set => nodeTypeName = value;
        }

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public SerializedUgenNode()
        {
            nodeId = Guid.NewGuid().ToString();
        }

        public SerializedUgenNode(UgenNode node)
        {
            nodeId = node.NodeId;
            nodeTypeName = node.GetType().Name;
            position = node.Position;
        }

        public UgenNode ToUgenNode()
        {
            UgenNode node = nodeTypeName switch
            {
                nameof(SliderNode) => new SliderNode(),
                nameof(YawRotatorNode) => new YawRotatorNode(),
                _ => null
            };

            if (node != null)
            {
                node.NodeId = nodeId;
                node.Position = position;
            }

            return node;
        }
    }
}
