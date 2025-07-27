using System;
using UnityEngine;

namespace Ugen.Graphs
{
    [Serializable]
    public class UgenGraph
    {
        [SerializeField] UgenBehaviourRef[] behaviours;
        
        public UgenBehaviourRef[] Behaviours => behaviours;
        
        public UgenGraph()
        {
            behaviours = new UgenBehaviourRef[0];
        }
        
        public UgenGraph(UgenBehaviourRef[] behaviours)
        {
            this.behaviours = behaviours;
        }
    }
}
