using DG.Tweening;
using Germanenko.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace Germanenko.Source
{


    public class Dropdown : MonoBehaviour
    {

        [SerializeField] private List<DDItem> listOfItems = new List<DDItem>();
        protected List<GameObject> currentListOsItems = new List<GameObject>();

        [SerializeField] private RectTransform listFolder;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private int itemPosition;

        [SerializeField] public DDItem _selectedItem;
        [SerializeField] private Image _selectedItemView;

        void Start()
        {
            FillDD();
            itemPosition = Screen.height / 3;
        }



        public void FillDD()
        {

            DeleteItems();

            foreach (var item in listOfItems) 
                AddItemToList(item);

        }



        private void AddItemToList(DDItem item)
        {

            var newDDItem = Pooler.Instance.Spawn(PoolType.Entities, itemPrefab, default, default, listFolder);

            newDDItem.GetComponent<DropDownItem>().Init(item.name, item.color, item.sprite);
        }



        protected void DeleteItems()
        {

            foreach (var item in currentListOsItems)
            {
                Pooler.Instance.Despawn(PoolType.Entities, item);
            }

            currentListOsItems.Clear();

        }



        public void SelectDDItem(string name)
        {
            _selectedItem.name = name;
            foreach (var item in listOfItems)
            {
                if(item.name == name)
                {
                    _selectedItem.color = item.color;
                    _selectedItem.sprite = item.sprite;

                    _selectedItemView.color = item.color;
                    _selectedItemView.sprite = item.sprite;
                }    
            }
        }
    }



    [System.Serializable]
    public class DDItem
    {
        public Sprite sprite;
        public Color color;
        public string name;

    }


}