using Germanenko.Framework;
using Germanenko.Source;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDropdown : Dropdown<BaseDDItem>
{
    public override void SelectDDItem(string name)
    {
        SelectedItem.name = name;
        foreach (var item in listOfItems)
        {
            if (item.name == name)
            {
                SelectedItem.color = item.color;
                SelectedItem.sprite = item.sprite;

                _selectedItemView.color = item.color;
                _selectedItemView.sprite = item.sprite;
            }
        }
    }

    protected override void AddItemToList(BaseDDItem item)
    {
        var newDDItem = Pooler.Instance.Spawn(PoolType.Entities, itemPrefab, default, default, listFolder);

        newDDItem.GetComponent<DropDownItem>().Init(item.name, item.color, item.sprite);
    }
}



