using System;
using UnityEngine;

namespace Ugen.Serialization
{
    [Serializable]
    public abstract class UgenNodeData
    {
        [SerializeField] string _id;
        [SerializeField] Vector2 _position;

        public string Id => _id;

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public abstract string Name { get; }
        public abstract PortData[] InputPorts { get; }
        public abstract PortData[] OutputPorts { get; }

        public UgenNodeData()
        {
            _id = Guid.NewGuid().ToString();
            _position = Vector2.zero;
        }

        protected UgenNodeData(string id, Vector2 position)
        {
            _id = id;
            _position = position;
        }
    }
}
