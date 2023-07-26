using UnityEngine;


namespace Germanenko.Source
{

	public struct SignalReloadList
	{
	}



	public struct SignalCameraShake
	{
		public int strength;
	}



	public struct SignalNewTaskData
	{
		public string field;
		public string data;
	}



	public struct SignalEndInput
	{
		public Transform sender;
	}

}