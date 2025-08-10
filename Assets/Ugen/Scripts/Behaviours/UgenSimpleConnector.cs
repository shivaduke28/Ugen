using System;
using UnityEngine;

namespace Ugen.Behaviours
{
    [Serializable]
    public class UgenConnection
    {
        public UgenBehaviour source;
        public int outputIndex;
        public UgenBehaviour target;
        public int inputIndex;
    }

    public sealed class UgenSimpleConnector : MonoBehaviour
    {
        [SerializeField] UgenConnection[] connections;

        void Start()
        {
            foreach (var connection in connections)
            {
                if (connection.source != null && connection.target != null)
                {
                    connection.source.ConnectTo(
                        connection.outputIndex,
                        connection.target,
                        connection.inputIndex
                    );

                    Debug.Log($"Connected {connection.source.name}[{connection.outputIndex}] -> {connection.target.name}[{connection.inputIndex}]");
                }
            }
        }
    }
}
