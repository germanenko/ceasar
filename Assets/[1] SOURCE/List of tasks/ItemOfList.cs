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

namespace Germanenko.Source
{

    public class ItemOfList : MonoBehaviour
    {
        [SerializeField] private int _id;
        public int ID => _id;
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
        [SerializeField] private ElementsContainer _parent;

        public void Init(Tasks _data, int priority)
        {
            _taskForm = FindObjectOfType<TaskForm>();

            Color currentColor;
            ColorUtility.TryParseHtmlString("#" + _data.Color, out currentColor);
            //TaskColor.color =  currentColor;

            _id = _data.ID;
            IDText.text = _data.ID.ToString();
            Title.text = _data.Name;
            _priority = priority;
            Priority.text = _priority.ToString();

            if (IsDraft)
                _icon.gameObject.SetActive(true);

            _lerpToPlaceholder = GetComponent<LerpToPlaceholder>();
            _layoutElement = GetComponent<LayoutElement>();
            _parent = transform.GetComponentInParent<ElementsContainer>();
        }



        public void SetPriority(int priority)
        {
            _priority = priority;
            Priority.text = _priority.ToString();
        }



        public void SendID()
        {
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
                        if(!_tasksToReplace.Contains(_spawnedReplaceTaskEmpty.transform))
                            _tasksToReplace.Add(_spawnedReplaceTaskEmpty.transform);
                    }
                }
            }
        }



        private void Update()
        {
            if (isDragging)
                _lerpToPlaceholder.placeholderTransform.GetComponent<LayoutElement>().ignoreLayout = _layoutElement.ignoreLayout;
        }



        public void OnBeginDrag()
        {
            _layoutElement.ignoreLayout = true;
            _spawnedReplaceTaskEmpty = Pooler.Instance.Spawn(PoolType.Entities, _taskEmpty, default(Vector3), default(Quaternion), ConstantSingleton.Instance.FolderListOfItems);
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

    }

}