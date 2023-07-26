using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Germanenko.Framework
{

    public sealed class Updater : MonoBehaviour
    {

        public static Updater Default;

        private List<ITick> ticks = new List<ITick>();
        private List<ITickFixed> ticksFixed = new List<ITickFixed>();
        private List<ITickLate> ticksLate = new List<ITickLate>();

        internal static List<Time> times = new List<Time>();
        internal static int timesLen = 0;

        private int countTicks;
        private int countTicksFixed;
        private int countTicksLate;


        void Awake()
        {

            Default = this;

        }



        public static void Add(object obj)
        {

            if(obj is ITick)
            {

                Default.ticks.Add(obj as ITick);
                Default.countTicks = Default.ticks.Count;

            }

            if (obj is ITickFixed)
            {

                Default.ticksFixed.Add(obj as ITickFixed);
                Default.countTicksFixed = Default.ticksFixed.Count;

            }

            if (obj is ITickLate)
            {

                Default.ticksLate.Add(obj as ITickLate);
                Default.countTicksLate = Default.ticksLate.Count;

            }

        }



        public static void Remove(object obj)
        {

            if (obj is ITick)
            {

                Default.ticks.Remove(obj as ITick);
                Default.countTicks = Default.ticks.Count;

            }

            if (obj is ITickFixed)
            {

                Default.ticksFixed.Remove(obj as ITickFixed);
                Default.countTicksFixed = Default.ticksFixed.Count;

            }

            if (obj is ITickLate)
            {

                Default.ticksLate.Remove(obj as ITickLate);
                Default.countTicksLate = Default.ticksLate.Count;

            }

        }



        public void Tick(float delta)
        {

            for (int i = 0; i < timesLen; i++)
            {

                times[i].Tick();

            }

            for (int i = 0; i < countTicks; i++)
            {

                ticks[i].Tick(delta);

            }

        }



        public void TickFixed(float delta)
        {

            for (int i = 0; i < countTicksFixed; i++)
            {

                ticksFixed[i].TickFixed(delta);

            }

        }



        public void TickLate(float delta)
        {

            for (int i = 0; i < countTicksLate; i++)
            {

                ticksLate[i].TickLate(delta);

            }

        }



        void Update()
        {

            if(Toolbox.changingScene)
                return;


            var delta = Time.delta * Time.Default.timeScale;

            for(int i = 0; i < timesLen; i++)
            {

                times[i].Tick();

            }

            for(var i = 0; i < countTicks; i++)
            {

                ticks[i].Tick(delta);

            }

        }



        void FixedUpdate()
        {

            if(Toolbox.changingScene)
                return;

            var delta = Time.deltaFixed;

            for(var i = 0; i < countTicksFixed; i++)
                ticksFixed[i].TickFixed(delta);

        }



        void LateUpdate()
        {

            if(Toolbox.changingScene)
                return;

            var delta = Time.delta;

            for(var i = 0; i < countTicksLate; i++)
                ticksLate[i].TickLate(delta);

        }



        public static void Create()
        {

            var obj = new GameObject("ActorsUpdate");
            DontDestroyOnLoad(obj);
            Default = obj.AddComponent<Updater>();

        }

    }

}
