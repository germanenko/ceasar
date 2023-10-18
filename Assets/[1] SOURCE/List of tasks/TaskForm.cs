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

		public bool isDraft;
		[SerializeField] private Question _draftPopup;

        public UnityEvent OnShowTask;

		[SerializeField] private GameObject _previousVersionButton;
		[SerializeField] private GameObject _archiveVersionButton;

        public void Start()
        {
            itemPosition = Screen.height / 3;
			Framework.Signals.Add(this);
		}



		public void CreateTask()
		{
			if(Toolbox.Get<ListOfTasks>().CountOfDrafts() > 0)
			{
				_draftPopup.Show();
            }
			else
			{
                Signal.Send("TaskControl", "OpenTask");
            }
		}



		public void SetTaskID(int id, bool draft)
		{
			_id = id;
			isDraft = draft;

            if (Toolbox.Get<Tables>().GetSaveTask(_id) != null)
				_previousVersionButton.SetActive(true);
			else
                _previousVersionButton.SetActive(false);
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
			{
                Toolbox.Get<Tables>().EditTask(_nameField.text, _colorField._selectedItem.name, _id);
            }
			else
				Toolbox.Get<Tables>().AddTask(_nameField.text, _colorField._selectedItem.name);


            Toolbox.Get<ListOfTasks>().ReloadList();

        }



		public void CloseTask()
		{
			Signal.Send("TaskControl", "CloseTask");
        }



		public int DraftCount()
		{
			return Toolbox.Get<ListOfTasks>().CountOfDrafts();
        }



		public void ReplaceDraftNewTask()
		{
            //Toolbox.Get<Tables>().AddTask(_nameField.text, _colorField._selectedItem.name);
			Toolbox.Get<Tables>().DraftToTask(_id);

            Toolbox.Get<ListOfTasks>().ReloadList();
        }



		public void SaveDraft()
		{
            if (Toolbox.Get<ListOfTasks>().CountOfDrafts() == 0)
            {
                Toolbox.Get<Tables>().AddDraft(_nameField.text, _colorField._selectedItem.name);
            }

            if (isDraft)
            {
                Toolbox.Get<Tables>().EditTask(_nameField.text, _colorField._selectedItem.name, _id);
            }

            Toolbox.Get<ListOfTasks>().ReloadList();
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

			if (isDraft)
				SetDraftFields();

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



        private void SetDraftFields()
        {
            string sql = $"SELECT * FROM Tasks WHERE Draft = 1";

            List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

			SetTaskID(taskList[0].ID, true);
            _idField.text = taskList[0].ID.ToString();
            _nameField.text = taskList[0].Name;
            _colorField.SelectDDItem(taskList[0].Color);
        }



        public void SetEditTask(bool edit)
		{
			_editTask = edit;
		}



        public void SetDraftTask(bool draft)
        {
            isDraft = draft;
        }



		public void ReturnTask()
		{
            string sql = $"SELECT * FROM Tasks WHERE Reference = {_id}";

            List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

            SetTaskID(taskList[0].Reference, true);
            _idField.text = taskList[0].Reference.ToString();
            _nameField.text = taskList[0].Name;
            _colorField.SelectDDItem(taskList[0].Color);
        }



        public void OnHidden()
		{
			_fields.ClearFields();

            _idField.text = "";
            _nameField.text = "";
            _colorField.SelectDDItem("");

            if (_editTask)
				_editTask = false;

            isDraft = false;
        }



        private void OnDestroy()
        {
			 Framework.Signals.Remove(this);
		}

	}

}
