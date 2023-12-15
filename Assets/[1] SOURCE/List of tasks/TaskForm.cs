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
using System.Globalization;

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

		[SerializeField] private Clock _clocks;

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

		[SerializeField] private Image _blur;

		public ScrollRect ScrollRect;

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

            if (Toolbox.Get<Tables>().GetArchiveTask(_id) != null)
                _archiveVersionButton.SetActive(true);
            else
                _archiveVersionButton.SetActive(false);
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
                Toolbox.Get<Tables>().EditTask(_nameField.text, _colorField._selectedItem.name, _clocks.GetStartPeriod(), _clocks.GetEndPeriod(), _id);
            }
			else
				Toolbox.Get<Tables>().AddTask(_nameField.text, _colorField._selectedItem.name, _clocks.GetStartPeriod(), _clocks.GetEndPeriod());


            Toolbox.Get<ListOfTasks>().ReloadList();

        }



		public void CloseTask()
		{
            _blur.material.SetFloat("_Alpha", 0f);
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
                Toolbox.Get<Tables>().AddDraft(_nameField.text, _colorField._selectedItem.name, _clocks.GetStartPeriod(), _clocks.GetEndPeriod());
            }

            if (isDraft)
            {
                Toolbox.Get<Tables>().EditDraft(_nameField.text, _colorField._selectedItem.name);
            }

            Toolbox.Get<ListOfTasks>().ReloadList();
        }



		public void SaveArchive()
		{
            Toolbox.Get<Tables>().AddArchiveTask(_nameField.text, _colorField._selectedItem.name, _id);
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
            _colorField.SelectDDItem("FF0000");

            _fields.CreateFields(itemParent);

			if (_editTask)
				SetFields();

			if (isDraft)
				SetDraftFields();

            _blur.material.SetFloat("_Alpha", 4f);

            OnShowTask?.Invoke();

            
        }



		private void SetFields()
		{
			print("setfields");

			string sql = $"SELECT * FROM Tasks WHERE ID = {_id}";

            List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

			_idField.text = taskList[0].ID.ToString();
			SetValuesFromTask(taskList[0]);
        }



        private void SetDraftFields()
        {
            string sql = $"SELECT * FROM SavesAndDrafts WHERE Draft = 1 AND Reference = 0";

            List<SavesAndDrafts> draft = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(sql);

			string sqlTask = $"SELECT * FROM Tasks WHERE ID = {draft[0].TaskID}";

            List<Tasks> task = ConstantSingleton.Instance.DbManager.Query<Tasks>(sqlTask);

            SetTaskID(task[0].ID, true);
            _idField.text = task[0].ID.ToString();

			SetValuesFromTask(task[0]);
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
            string returnSql = $"SELECT * FROM SavesAndDrafts WHERE Reference = {_id} AND Draft = 0";
            List<SavesAndDrafts> save = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(returnSql);

            string taskSql = $"SELECT * FROM Tasks WHERE ID = {save[0].TaskID}";
            List<Tasks> task = ConstantSingleton.Instance.DbManager.Query<Tasks>(taskSql);

            //SetTaskID(save[0].Reference, true);
            //_idField.text = save[0].Reference.ToString();

            SetValuesFromTask(task[0]);

            //_nameField.text = task[0].Name;
            //_colorField.SelectDDItem(task[0].Color);
            //_clocks.SetPeriod(task[0].StartTime, task[0].EndTime);
        }



        public void ReturnArchive()
        {
            string archiveSql = $"SELECT * FROM SavesAndDrafts WHERE Reference = {_id} AND Draft = 1";
            List<SavesAndDrafts> archive = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(archiveSql);

            string taskSql = $"SELECT * FROM Tasks WHERE ID = {archive[0].TaskID}";
            List<Tasks> task = ConstantSingleton.Instance.DbManager.Query<Tasks>(taskSql);

			//SetTaskID(archive[0].Reference, true);
			//_idField.text = archive[0].Reference.ToString();

			SetValuesFromTask(task[0]);

            //_nameField.text = task[0].Name;
            //_colorField.SelectDDItem(task[0].Color);
            //_clocks.SetPeriod(task[0].StartTime, task[0].EndTime);
        }



		private void SetValuesFromTask(Tasks task)
		{
            _nameField.text = task.Name;
            _colorField.SelectDDItem(task.Color);
            _clocks.SetPeriod(task.StartTime, task.EndTime);

			if(Localization.Instance.Language == LocalizationLanguage.Russia)
			{
                _clocks.SetPreviewPeriodText(task.StartTime == task.EndTime ?
					string.Format("{0:00}:{1:00}", task.StartTime.Hour, task.StartTime.Minute) :
					$"{string.Format("{0:00}:{1:00}", task.StartTime.Hour, task.StartTime.Minute)} - {string.Format("{0:00}:{1:00}", task.EndTime.Hour, task.EndTime.Minute)}");
            }
			else if(Localization.Instance.Language == LocalizationLanguage.USA)
			{
                _clocks.SetPreviewPeriodText(task.StartTime == task.EndTime ?
					task.StartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture) :
					 $"{task.StartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)} - {task.EndTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)}");
            }
        }



        public void OnHidden()
		{
			_fields.ClearFields();

            _idField.text = "";
            _nameField.text = "";
            _colorField.SelectDDItem("FF0000");

			_archiveVersionButton.SetActive(false);
			_previousVersionButton.SetActive(false);

			_clocks.ClearTime();
			_clocks.SetActiveTimeSelector(false);

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
