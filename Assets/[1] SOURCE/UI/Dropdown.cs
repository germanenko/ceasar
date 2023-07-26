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
            var img = newDDItem.GetComponent<Image>();

            img.sprite = item.sprite;
            img.color = item.color;

        }



        protected void DeleteItems()
        {

            foreach (var item in currentListOsItems)
            {
                Pooler.Instance.Despawn(PoolType.Entities, item);
            }

            currentListOsItems.Clear();

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