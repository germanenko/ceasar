using System;
using System.Collections.Generic;
using UnityEngine;


namespace Germanenko.Framework
{

	[Serializable]
	public class Signals : Singleton<Signals>, IComponent
	{

		public readonly Dictionary<int, List<IReceive>> signals = new Dictionary<int, List<IReceive>>();

		#region LOGIC



		public static void Send<T>(T val = default(T))
        {
			if (Instance != null) Instance.SendChecked(val);
		}


		public void SendChecked<T>(T val = default(T))
		{

			List<IReceive> cachedSignals;

			if(!signals.TryGetValue(typeof(T).GetHashCode(), out cachedSignals))
				return;

			var len = cachedSignals.Count;

			for(int i = 0; i < len; i++)
			{

				(cachedSignals[i] as IReceive<T>).HandleSignal(val);

			}

		}



		public void Add<T>(IReceive recieve)
		{

			List<IReceive> cachedSignals;
			if(signals.TryGetValue(typeof(T).GetHashCode(), out cachedSignals))
			{

				cachedSignals.Add(recieve);
				return;

			}

			signals.Add(typeof(T).GetHashCode(), new List<IReceive> { recieve });

		}



		public void Remove<T>(IReceive recieve)
		{

			List<IReceive> cachedSignals;

			//Timer.Add(Time.DeltaTime, () =>   // !!!!
			//{

				if(signals.TryGetValue(typeof(T).GetHashCode(), out cachedSignals))
				{
					cachedSignals.Remove(recieve);
				}

			//});

		}



		public void Add(IReceive recieve, Type type)
		{

			List<IReceive> cachedSignals;

			if(signals.TryGetValue(type.GetHashCode(), out cachedSignals))
			{
				cachedSignals.Add(recieve);
				return;
			}

			signals.Add(type.GetHashCode(), new List<IReceive> { recieve });

		}



		public void Remove(IReceive recieve, Type type)
		{

			List<IReceive> cachedSignals;

			//Timer.Add(Time.DeltaTime, () =>  // !!!!
			//{

				if(signals.TryGetValue(type.GetHashCode(), out cachedSignals))
				{

					cachedSignals.Remove(recieve);

				}

			//});

		}



		public static void Add(object obj)
		{
			if (Instance != null) Instance.AddChecked(obj);
		}



		public static void Remove(object obj)
		{
			if (Instance != null) Instance.RemoveChecked(obj);
		}



		public void AddChecked(object obj)
		{

			var reciever = obj as IReceive;
			if(reciever == null)
				return;

			var all = obj.GetType().GetInterfaces();

			foreach(var intType in all)
			{

				if(intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IReceiveGlobal<>))
				{

                    Instance.Add(reciever, intType.GetGenericArguments()[0]);

				}

				else if(intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IReceive<>))
				{

					Add(reciever, intType.GetGenericArguments()[0]);

				}

			}

		}



		public void RemoveChecked(object obj)
		{

			var reciever = obj as IReceive;
			if(reciever == null)
				return;

			var all = obj.GetType().GetInterfaces();

			foreach(Type intType in all)
			{

				if(intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IReceiveGlobal<>))
				{

                    Instance.Remove(reciever, intType.GetGenericArguments()[0]);

				}

				else if(intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IReceive<>))
				{

					Remove(reciever, intType.GetGenericArguments()[0]);

				}

			}

		}



		public void Dispose()
		{

			signals.Clear();

		}

		#endregion

	}

}