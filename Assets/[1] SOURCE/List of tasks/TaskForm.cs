using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Germanenko.Framework;
using UnityEngine.UI;
using System;
using Doozy.Runtime.Signals;
using DG.Tweening;
using UnityEngine.Events;

namespace Germanenko.Source
{

	public class TaskForm : MonoBehaviour, IReceive<SignalNewTaskData>, IReceive<SignalEndInput>
	{

		[SerializeField] private TaskFormFields _fields;
		[SerializeField] private Transform itemParent;


		private int itemPosition;

		private Tasks task = new Tasks();
		public Tasks Task { get => task; set => task = value; }

		[SerializeField] private bool _editTask;
		public bool EditTask => _editTask;

		public UnityEvent OnShowTask;

        public void Start()
        {

			itemPosition = Screen.height / 3;
			Framework.Signals.Add(this);
		}



		public void HandleSignal(SignalEndInput arg)
		{

			float newY = -(itemPosition + (arg.sender as RectTransform).anchoredPosition.y);

			_fields.ActivateNextField(arg.sender?.gameObject);

			if (newY < 0) return;

			(itemParent.transform as RectTransform).DOAnchorPosY(newY, 0.3f);

		}



		public void SaveTask()
        {

			Toolbox.Get<Tables>().AddTask();
			Toolbox.Get<ListOfTasks>().ReloadList();

        }


		
		public void CloseTask()
		{
			Signal.Send("TaskControl", "CloseTask");
		}



        public void HandleSignal(SignalNewTaskData arg)
        {

			switch(arg.field)
            {

				case "Type":
					SetType(arg.data);
					break;

				case "Color":
					SetColor(arg.data);
					break;

				default:
					break;

            }

        }




		private void SetName(string data)
        {
			task.Name = data;
        }



		private void SetType(string data)
        {
			task.Type = (int) Enum.Parse(typeof(TypeOfTasks), data);
        }



		private void SetColor(string data)
        {
			task.Color = data;

        }



		public void SendSignalName(TMP_InputField data)
        {
			SetName(data.text);
		}



		public void OnShow()
		{
			_fields.CreateFields(itemParent);
			OnShowTask?.Invoke();
        }

		public void SetEditTask(bool edit)
		{
			_editTask = edit;
		}

		public void OnHidden()
		{
			_fields.ClearFields();

			if(_editTask)
				_editTask = false;
		}


        private void OnDestroy()
        {
			 Framework.Signals.Remove(this);
		}

	}

}
