using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using UnityEngine.EventSystems;
using HutongGames.PlayMaker;
using Germanenko.Framework;
using System.Linq;
using UnityEngine.UI.Extensions;

namespace Germanenko.Source
{

    public class ItemOfList : MonoBehaviour
    {
        [SerializeField] private string _id;
        public string ID => _id;
        [SerializeField] private int _priority;

        [SerializeField] private TextMeshProUGUI IDText;
        [SerializeField] private TextMeshProUGUI Title;
        [SerializeField] private TextMeshProUGUI Times;
        [SerializeField] private TextMeshProUGUI Priority;

        [SerializeField] private Image TaskColor;
        [SerializeField] private Image _icon;

        public bool IsDraft;
        public bool isDragging;

        [SerializeField] private TaskForm _taskForm;

        [SerializeField] private List<Transform> _tasksToReplace;

        [SerializeField] private LayoutElement _layoutElement;

        [SerializeField] private GameObject _taskEmpty;
        [SerializeField] private GameObject _spawnedReplaceTaskEmpty;

        [SerializeField] private LerpToPlaceholder _lerpToPlaceholder;

        [SerializeField, HideInInspector] private ScrollRect _scroll;
        [SerializeField, HideInInspector] private ScrollConflictManager _conflictManager;

        [SerializeField] private bool _canSelect;
        public bool CanSelect => _canSelect;

        [SerializeField] private bool _selected;
        public bool Selected => _selected;

        [SerializeField] private UIToggle _checkBox;
        public UIToggle CheckBox => _checkBox;

        public Canvas ParentCanvas;

        public void Init(Tasks _data, int priority)
        {
            _taskForm = FindObjectOfType<TaskForm>();

            Color currentColor;
            ColorUtility.TryParseHtmlString("#" + _data.HexColor, out currentColor);
            TaskColor.color =  currentColor;

            _id = _data.Id;
            IDText.text = _data.Id;
            Title.text = _data.Title;
            _priority = priority;
            Priority.text = _priority.ToString();

            if (IsDraft)
                _icon.gameObject.SetActive(true);

            _lerpToPlaceholder = GetComponent<LerpToPlaceholder>();
            _layoutElement = GetComponent<LayoutElement>();

            _scroll = transform.parent.parent.parent.GetComponent<ScrollRect>();
            _conflictManager = GetComponent<ScrollConflictManager>();
            _conflictManager.ParentScrollRect = _scroll;

            ParentCanvas = transform.parent.parent.parent.parent.GetComponent<Canvas>();
        }



        public void SetPriority(int priority)
        {
            _priority = priority;
            Priority.text = _priority.ToString();
        }



        public void SendID()
        {
            _taskForm.SetOpenPositionAndColor(transform.localPosition, TaskColor.color);

            _taskForm.SetTaskID(_id, IsDraft);
            
            if(!IsDraft)
                _taskForm.SetEditTask(true);
        }



        public void SetDraft(bool draft)
        {
            IsDraft = draft;
            _icon.gameObject.SetActive(draft);
        }



        private void OnDisable()
        {
            SetDraft(false);
        }



        public void OnTriggerEnter2D(Collider2D col)
        {    
            if (col.TryGetComponent(out ItemOfList iol))
            {
                if (!col.GetComponent<ItemOfList>().isDragging && isDragging == true)
                {
                    if(_spawnedReplaceTaskEmpty)
                    {
                        _spawnedReplaceTaskEmpty.transform.SetSiblingIndex(col.transform.GetSiblingIndex());
                        SetPreviewMultiChoice(false);

                        if (!_tasksToReplace.Contains(_spawnedReplaceTaskEmpty.transform))
                            _tasksToReplace.Add(_spawnedReplaceTaskEmpty.transform);
                    }
                }
            }
        }



        //public void OnTriggerStay2D(Collider2D col)
        //{
        //    if (col.TryGetComponent(out ItemOfList iol))
        //    {
        //        if (isDragging == true && !col.GetComponent<ItemOfList>().isDragging)
        //        {
        //            if (_spawnedReplaceTaskEmpty)
        //            {
        //                _spawnedReplaceTaskEmpty.transform.SetSiblingIndex(col.transform.GetSiblingIndex());
        //                SetPreviewMultiChoice(false);

        //                if (!_tasksToReplace.Contains(_spawnedReplaceTaskEmpty.transform))
        //                    _tasksToReplace.Add(_spawnedReplaceTaskEmpty.transform);
        //            }
        //        }
        //    }
        //}



        private void Update()
        {
            if (isDragging)
                _lerpToPlaceholder.placeholderTransform.GetComponent<LayoutElement>().ignoreLayout = _layoutElement.ignoreLayout;
        }



        public void OnBeginDrag()
        {
            _layoutElement.ignoreLayout = true;
            _spawnedReplaceTaskEmpty = Pooler.Instance.Spawn(PoolType.Entities, _taskEmpty, transform.localPosition, default(Quaternion), ConstantSingleton.Instance.FolderListOfItems);
            _spawnedReplaceTaskEmpty.transform.localPosition = transform.localPosition;
            _spawnedReplaceTaskEmpty.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }



        public void ReplaceTask()
        {
            if (_tasksToReplace.Count > 0)
            {
                print("replace");
                transform.SetSiblingIndex(_tasksToReplace[0].GetSiblingIndex());

                Pooler.Instance.Despawn(PoolType.Entities, _spawnedReplaceTaskEmpty);
                Toolbox.Get<Tables>().UpdatePriority();
            }
            else
            {
                Pooler.Instance.Despawn(PoolType.Entities, _spawnedReplaceTaskEmpty);
            }
        }



        public void EnableHorizontalSwipe(bool enable)
        {
            TouchInArea.HorizontalTouch.gameObject.SetActive(enable);
        }



        public void SetCanSelect(bool canSelect)
        {
            _canSelect = canSelect;
            _checkBox.gameObject.SetActive(canSelect);

            if(!canSelect)
                _checkBox.isOn = false;
        }



        public void SetPreviewCheckBox(bool previewCheckBox)
        {
            _checkBox.gameObject.SetActive(previewCheckBox);

            _checkBox.GetComponent<CanvasGroup>().alpha = previewCheckBox ? .5f : 1f;
        }



        public void SetSelected()
        {
            _selected = !_selected;
            _checkBox.SetIsOn(_selected);
        }



        public void SetMultiChoice(bool multiChoice)
        {
            MultiChoice.Instance.SetMultiChoice(multiChoice);
        }



        public void SetPreviewMultiChoice(bool previewMultiChoice)
        {
            MultiChoice.Instance.SetPreviewMultiChoice(previewMultiChoice);
        }



        public bool GetPreviewMultiChoiceEnabled()
        {
            return MultiChoice.Instance.PreviewMultiChoiceEnabled;
        }
    }

}