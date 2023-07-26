using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Germanenko.Framework
{

    public class Pool
    {

        private Transform parentPool;
        private Dictionary<int, Stack<GameObject>> cachedObjects = new Dictionary<int, Stack<GameObject>>();
        private Dictionary<int, int> CachedIds = new Dictionary<int, int>();


        public Pool PopulateWith(GameObject prefab, int amount, int amountPerTick, int tickSize = 1)
        {
            int key = prefab.GetInstanceID();
            var stack = new Stack<GameObject>(amount);
            cachedObjects.Add(key, stack);

            Observable.IntervalFrame(tickSize, FrameCountType.EndOfFrame).Where(val => amount > 0).Subscribe(_loop =>
            {

                Observable.Range(0, amountPerTick).Where(check => amount > 0).Subscribe(_pop =>
                {

                    var go = Populate(prefab, Vector3.zero, Quaternion.identity, parentPool);
                    go.SetActive(false);
                    CachedIds.Add(go.GetInstanceID(), key);
                    cachedObjects[key].Push(go);
                    amount--;

                });

            });

            return this;
        }


        public void SetParent(Transform parentPool)
        {
            this.parentPool = parentPool;
        }


        public GameObject Spawn(GameObject prefab, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform parentPool = null)
        {
            int key = prefab.GetInstanceID();

            bool stacked = cachedObjects.TryGetValue(key, out Stack<GameObject> stack);

            if (stacked && stack.Count > 0)
            {

                Transform transform = stack.Pop().transform;
                transform.SetParent(parentPool);

                transform.rotation = rotation;

                if ((object)parentPool == null) transform.position = position;
                else transform.localPosition = position;

                IPoolChild poolChild = transform.GetComponent<IPoolChild>();
                if (poolChild != null) poolChild.OnSpawnForChildren(prefab);

                IPoolable poolable = transform.GetComponent<IPoolable>();
                if (poolable != null) poolable.OnSpawn();

                transform.gameObject.SetActive(true);

                return transform.gameObject;

            }

            if (!stacked) cachedObjects.Add(key, new Stack<GameObject>());
            GameObject createdPrefab = Populate(prefab, position, rotation, parentPool);
            CachedIds.Add(createdPrefab.GetInstanceID(), key);

            return createdPrefab;

        }


        public void Despawn(GameObject go)
        {

            if (go == null) return;
            if (!go.activeSelf) return;

            go.SetActive(false);

            cachedObjects[CachedIds[go.GetInstanceID()]].Push(go);

            IPoolable poolable = go.GetComponent<IPoolable>();
            if (poolable != null) poolable.OnDespawn();

            if (parentPool != null) go.transform.SetParent(parentPool);

        }


        public void Dispose()
        {

            parentPool = null;
            cachedObjects.Clear();
            CachedIds.Clear();

        }


        public GameObject Populate(GameObject prefab, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform parentPool = null)
        {

            var go = Object.Instantiate(prefab, position, rotation, parentPool).transform;
            if ((object)parentPool == null) go.position = position;
            else go.localPosition = position;

            return go.gameObject;

        }
    }

}