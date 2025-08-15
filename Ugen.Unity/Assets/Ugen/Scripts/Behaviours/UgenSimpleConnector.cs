using System;
using R3;
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

        readonly CompositeDisposable disposable = new();

        void Start()
        {
            foreach (var connection in connections)
                if (connection.source != null && connection.target != null)
                    connection.source.GetOutput(connection.outputIndex)
                        .ConnectTo(connection.target.GetInput(connection.inputIndex), disposable);
        }
    }
}