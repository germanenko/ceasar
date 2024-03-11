using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Germanenko.Source;



namespace Germanenko.Framework
{

    public class Pooler : Singleton<Pooler>
    {

        private Dictionary<int, Pool> pools = new Dictionary<int, Pool>();


        public Pool AddPool(PoolType id, bool reparent = true)
        {
            if (pools.TryGetValue((int)id, out Pool pool) == false)
            {

                pool = new Pool();
                pools.Add((int)id, pool);

                if (reparent)
                {

                    var poolDynamic = ConstantSingleton.Instance.FolderDinamicElements;
                    var poolGO = new GameObject("Pool:" + id);
                    poolGO.transform.SetParent(poolDynamic);
                    pool.SetParent(poolGO.transform);

                }

            }

            return pool;

        }



        public GameObject Spawn(PoolType id, GameObject prefab, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform parentPool = null)
        {

            if(!pools.ContainsKey((int)id)) Instance.AddPool(id);
            return pools[(int)id].Spawn(prefab, position, rotation, parentPool);

        }



        public T Spawn<T>(PoolType id, GameObject prefab, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform parentPool = null)
        {

            if(!pools.ContainsKey((int)id)) Instance.AddPool(id);
            GameObject go = pools[(int)id].Spawn(prefab, position, rotation, parentPool);
            return go.GetComponent<T>();

        }



        public async void Despawn(PoolType id, GameObject obj, int timerDelay = 0)
        {
            await System.Threading.Tasks.Task.Delay(timerDelay);
            pools[(int)id].Despawn(obj);

        }



        public void Dispose()
        {

            foreach (var poolValues in pools.Values)
            {

                poolValues.Dispose();

            }

            pools.Clear();

        }

    }

}