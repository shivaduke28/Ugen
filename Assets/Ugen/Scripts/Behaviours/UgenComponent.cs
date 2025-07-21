using UnityEngine;

namespace Ugen.Behaviours
{
    public abstract class UgenComponent : MonoBehaviour
    {
        public abstract string Name { get; }
        public abstract IInput[] Inputs { get; }
        public abstract IOutput[] Outputs { get; }
    }
}
