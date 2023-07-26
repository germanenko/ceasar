namespace Germanenko.Framework
{

	public interface IReceive<T> : IReceive
	{

		void HandleSignal(T arg);

	}



	public interface IReceiveGlobal<T> : IReceive<T>
	{
	}



	public interface IReceive
	{
	}

}
