using System;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public abstract class UgenNodeData
    {
        [SerializeField] string id;
        [SerializeField] Vector2 position;

        public string Id => id;
        public Vector2 Position => position;

        public abstract Port[] InputPorts { get; }
        public abstract Port[] OutputPorts { get; }

        public UgenNodeData()
        {
            id = Guid.NewGuid().ToString();
            position = Vector2.zero;
        }

        protected UgenNodeData(string id, Vector2 position)
        {
            this.id = id;
            this.position = position;
        }
    }
}
