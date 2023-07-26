using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Germanenko.Framework
{

    public class Toolbox : Singleton<Toolbox>
    {

        private Dictionary<int, object> data = new Dictionary<int, object>();
        public static bool changingScene;
        public static bool applicationIsQuitting;


        public static void InitializeObject(object obj)
        {
            var awakeble = obj as IAwake;
            if(awakeble != null)
                awakeble.OnAwake();
            Updater.Add(obj);
        }



        public static T Add<T>(Type type = null) where T : new()
        {

            object o;
            var hash = type == null ? typeof(T).GetHashCode() : type.GetHashCode();
            if(Instance.data.TryGetValue(hash, out o))
            {

                InitializeObject(o);
                return (T)o;

            }

            var created = new T();

            InitializeObject(created);
            Instance.data.Add(hash, created);

            return created;

        }



        public static void Add(object obj)       
        {
                object possibleObj;

                if(Instance.data.TryGetValue(obj.GetType().GetHashCode(), out possibleObj))
                {

                    InitializeObject(possibleObj);

                }

                var add = obj;
                var scriptable = obj as ScriptableObject;

                if(scriptable)
                    add = Instantiate(scriptable);

                InitializeObject(obj);
                Instance.data.Add(obj.GetType().GetHashCode(), add);

        }



        public static void Remove(object obj)
        {
            if(applicationIsQuitting)
                return;
            Instance.data.Remove(obj.GetType().GetHashCode());
        }



        public static T Get<T>()
        {

            object resolve;
            Instance.data.TryGetValue(typeof(T).GetHashCode(), out resolve);
            return (T) resolve;

        }



        public void ClearScene()
        {
        }



        internal void ClearSessionData()
        {

            //if(applicationIsQuitting)
            //    return;

            //var toWipe = new List<int>();

            //foreach(var pair in data)
            //{
            //    if(!(pair.Value is IKernel))
            //        toWipe.Add(pair.Key);

            //    if(!(pair.Value is IDisposable needToBeCleaned))
            //        continue;

            //    needToBeCleaned.Dispose();
            //}

            //Pool.Dispose();
            //Storage.Dispose();
            //ProcessorGroups.Dispose();
            //ProcessorTimer.Default.Dispose();
            //ProcessorScene.Default.Dispose();
            //ProcessorUpdate.Default.Dispose();
            //Box.Default.Dispose();

            //for(var i = 0; i < toWipe.Count; i++)
            //{
            //    data.Remove(toWipe[i]);
            //}

        }

    }

}
