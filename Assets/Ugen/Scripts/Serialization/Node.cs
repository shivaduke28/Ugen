using System;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public abstract class Node
    {
        [SerializeField] string id;
        [SerializeField] Vector2 position;

        public string Id => id;
        public Vector2 Position => position;

        public abstract Port[] InputPorts { get; }
        public abstract Port[] OutputPorts { get; }

        public Node(string id, Vector2 position)
        {
            this.id = id;
            this.position = position;
        }
    }
}
