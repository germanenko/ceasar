using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using UnityEngine.EventSystems;
using HutongGames.PlayMaker;
using Germanenko.Framework;

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

        [SerializeField] private Vector3 _defaultScale;
        [SerializeField] private Vector3 _replaceScale;

        [SerializeField] private GameObject _taskEmpty;
        [SerializeField] private GameObject _spawnedBaseTaskEmpty;
        [SerializeField] private GameObject _spawnedReplaceTaskEmpty;

        public void Init(Tasks _data, int priority)
        {
            _taskForm = FindObjectOfType<TaskForm>();

            _defaultScale = transform.localScale;
            _replaceScale = _defaultScale * 1.1f;

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



        public void OnTriggerStay2D(Collider2D col)
        {
            if(col.TryGetComponent(out ItemOfList iol))
            {
                if (!col.GetComponent<ItemOfList>().isDragging && isDragging == true)
                {
                    if(!_spawnedReplaceTaskEmpty)
                    {
                        _spawnedReplaceTaskEmpty = Pooler.Instance.Spawn(PoolType.Entities, _taskEmpty, default(Vector3), default(Quaternion), ConstantSingleton.Instance.FolderListOfItems);
                        _spawnedReplaceTaskEmpty.transform.SetSiblingIndex(col.transform.GetSiblingIndex());
                    }
                    //_spawnedTaskEmpty = Pooler.Instance.Spawn(PoolType.Entities, _taskEmpty, default(Vector3), default(Quaternion), ConstantSingleton.Instance.FolderListOfItems);
                    //_spawnedTaskEmpty.transform.SetSiblingIndex(col.transform.GetSiblingIndex());
                    //_tasksToReplace.Add(col.transform);
                    //col.transform.localScale = _replaceScale;
                }
            }
        }   



        public void OnTriggerExit2D(Collider2D col)
        {
            //Destroy(_spawnedTaskEmpty);
            if(col.gameObject == _spawnedReplaceTaskEmpty)
            {
                print("delete");
                Pooler.Instance.Despawn(PoolType.Entities, _spawnedReplaceTaskEmpty);
                _spawnedReplaceTaskEmpty = null;
            }

            //_tasksToReplace.Remove(col.transform);
            //col.transform.localScale = _defaultScale;
        }



        public void OnBeginDrag()
        {
            _spawnedBaseTaskEmpty = Pooler.Instance.Spawn(PoolType.Entities, _taskEmpty, default(Vector3), default(Quaternion), ConstantSingleton.Instance.FolderListOfItems);
            _spawnedBaseTaskEmpty.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }



        public void OnEndDrag()
        {
            Pooler.Instance.Despawn(PoolType.Entities, _spawnedBaseTaskEmpty);
        }



        public void ReplaceTask()
        {
            if (_tasksToReplace.Count > 0)
            {
                print("replace");
                transform.SetSiblingIndex(_tasksToReplace[0].GetSiblingIndex());
                Toolbox.Get<Tables>().UpdatePriority();
            }
        }

    }

}