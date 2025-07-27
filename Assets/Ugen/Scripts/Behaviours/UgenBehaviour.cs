using System;
using UnityEngine;

namespace Ugen.Behaviours
{
    public class UgenBehaviour : MonoBehaviour
    {
        [SerializeField] string id;
        [SerializeField] UgenComponent[] components;
        public string Id => id;
        public UgenComponent[] Components => components;
        public string Name => gameObject.name;

        void Reset()
        {
            id = Guid.NewGuid().ToString();
        }
    }
}
