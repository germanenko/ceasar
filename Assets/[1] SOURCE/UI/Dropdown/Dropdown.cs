using DG.Tweening;
using Doozy._Examples.Common.Runtime;
using Doozy.Runtime.UIManager.Components;
using Germanenko.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace Germanenko.Source
{


    public abstract class Dropdown<T> : MonoBehaviour
    {
        [SerializeField] protected List<T> listOfItems = new List<T>();
        protected List<GameObject> currentListOsItems = new List<GameObject>();

        [SerializeField] protected RectTransform listFolder;
        [SerializeField] protected GameObject itemPrefab;
        [SerializeField] protected int itemPosition;

        [SerializeField] public T SelectedItem;
        [SerializeField] protected Image _selectedItemView;

        [SerializeField] protected PlayMakerFSM _fsm;
        [SerializeField] protected UIButton _button;

        void Start()
        {
            FillDD();
            itemPosition = Screen.height / 3;


            _button.onDeselectedEvent.AddListener(CloseDropdown);
        }



        private void CloseDropdown()
        {
            _fsm.SendEvent("ClickOut");
        }



        public void FillDD()
        {

            DeleteItems();

            foreach (var item in listOfItems) 
                AddItemToList(item);

        }



        protected abstract void AddItemToList(T item);



        protected void DeleteItems()
        {

            foreach (var item in currentListOsItems)
            {
                Pooler.Instance.Despawn(PoolType.Entities, item);
            }

            currentListOsItems.Clear();

        }



        public abstract void SelectDDItem(string name);
            
    }
}

[System.Serializable]
public class BaseDDItem
{
    public Sprite sprite;
    public Color color;
    public string name;
}