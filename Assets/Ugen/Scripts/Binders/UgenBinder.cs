using UnityEngine;

namespace Ugen.Binders
{
    /// <summary>
    /// Base class for all Ugen binders.
    /// Provides common functionality for GraphView integration.
    /// </summary>
    public abstract class UgenBinder : MonoBehaviour
    {
        /// <summary>
        /// Called when the binder is connected in the Graph View
        /// </summary>
        public virtual void OnGraphConnected()
        {
        }

        /// <summary>
        /// Called when the binder is disconnected in the Graph View
        /// </summary>
        public virtual void OnGraphDisconnected()
        {
        }

        /// <summary>
        /// Gets the display name for this binder in the Graph View
        /// </summary>
        public virtual string GetGraphNodeName()
        {
            return GetType().Name.Replace("Binder", "");
        }
    }
}