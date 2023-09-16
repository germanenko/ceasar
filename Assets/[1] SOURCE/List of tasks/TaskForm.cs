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

		[SerializeField] private int _id;
		[SerializeField] private TMP_InputField _idField;
		[SerializeField] private TMP_InputField _nameField;
		[SerializeField] private Dropdown _colorField;

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



		public void SetTaskID(int id)
		{
			_id = id;
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
			if (_editTask)
                Toolbox.Get<Tables>().EditTask(_nameField.text, _colorField._selectedItem.name, _id);
			else
				Toolbox.Get<Tables>().AddTask(_nameField.text, _colorField._selectedItem.name);


            Toolbox.Get<ListOfTasks>().ReloadList();

        }



		public void Save()
		{

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

			if (_editTask)
				SetFields();

            OnShowTask?.Invoke();
        }



		private void SetFields()
		{
			string sql = $"SELECT * FROM Tasks WHERE ID = {_id}";

            List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

			_idField.text = taskList[0].ID.ToString();
			_nameField.text = taskList[0].Name;
			_colorField.SelectDDItem(taskList[0].Color);
        }



		public void LoadDraftFields()
		{

		}



		public void SetEditTask(bool edit)
		{
			_editTask = edit;
		}



		public void OnHidden()
		{
			_fields.ClearFields();

            _idField.text = "";
            _nameField.text = "";
            _colorField.SelectDDItem("");

            if (_editTask)
				_editTask = false;
		}



        private void OnDestroy()
        {
			 Framework.Signals.Remove(this);
		}

	}

}
