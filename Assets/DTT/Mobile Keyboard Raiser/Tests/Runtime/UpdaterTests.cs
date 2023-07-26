using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DTT.KeyboardRaiser.Tests
{
    /// <summary>
    /// Tests the <see cref="Updater"/> singleton.
    /// </summary>
    public class UpdaterTests
    {
        /// <summary>
        /// Tests whether retrieving the instance doesn't return a null object.
        /// Expects not a null object.
        /// </summary>
        [Test]
        public void Instance_Retrieved_NotNull()
        {
            // Assert.
            Assert.IsNotNull(Updater.Instance);
        }
        
        /// <summary>
        /// Tests whether the instance is the same everytime.
        /// Expects the same instance on the second retrieval.
        /// </summary>
        [Test]
        public void Instance_Retrieved_NotRecreated()
        {
            // Act.
            Updater instance = Updater.Instance;
            
            // Assert.
            Assert.AreEqual(instance, Updater.Instance);
        }

        /// <summary>
        /// Tests that the update event is invoked every frame.
        /// Expects a callback every frame.
        /// </summary>
        /// <param name="framesToWait">The amount of frames to wait for and see whether the update event matches this.</param>
        [UnityTest]
        public IEnumerator UpdatedEvent_CalledEveryFrame([Values(0U, 1U, 10U)] uint framesToWait)
        {
            // Arrange.
            int counter = 0;
            Updater.Instance.Updated += OnUpdate;

            for (int i = 0; i < framesToWait; i++)
                yield return null;
            
            Assert.AreEqual(framesToWait, counter); 

            void OnUpdate() => counter++;
        }
    }
}