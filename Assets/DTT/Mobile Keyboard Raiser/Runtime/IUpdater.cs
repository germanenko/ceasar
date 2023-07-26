using System;

namespace DTT.KeyboardRaiser
{
    /// <summary>
    /// Should be implemented on objects that can expose an update loop.
    /// </summary>
    public interface IUpdater
    {
        /// <summary>
        /// Should be invoked whenever an update occurs.
        /// </summary>
        event Action Updated;
    }
}