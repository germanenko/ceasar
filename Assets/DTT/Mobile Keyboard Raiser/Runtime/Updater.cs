using System;
using UnityEngine;

namespace DTT.KeyboardRaiser
{
    /// <summary>
    /// Exposes the <see cref="MonoBehaviour"/> update loop through the <see cref="IUpdater"/> interface.
    /// </summary>
    internal class Updater : MonoBehaviour, IUpdater
    {
        /// <summary>
        /// Is invoked every Unity update loop.
        /// </summary>
        public event Action Updated;

        /// <summary>
        /// Public singleton instance.
        /// </summary>
        public static Updater Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("Updater").AddComponent<Updater>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static Updater _instance;

        /// <summary>
        /// Singleton integrity check.
        /// </summary>
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this);
        }

        /// <summary>
        /// Invokes <see cref="Updated"/>.
        /// </summary>
        private void Update() => Updated?.Invoke();
    }
}