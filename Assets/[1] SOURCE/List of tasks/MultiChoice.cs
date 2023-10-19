using Doozy.Runtime.Signals;
using Germanenko.Framework;
using Germanenko.Source;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiChoice : MonoBehaviour
{
    public static MultiChoice Instance;

    [SerializeField] private List<ItemOfList> _selectedTasks;
    public List<ItemOfList> SelectedTasks => _selectedTasks;

    [SerializeField] private List<int> _selectedTasksId;

    [SerializeField] private TextMeshProUGUI _countSelectedTasksText;

    private void Awake()
    {
        Instance = this;  
    }



    public void SetMultiChoice(bool multiChoice)
    {
        //ConstantSingleton.Instance.MultiChoiceScreen.SetActive(multiChoice);

        if (multiChoice)
        {
            Signal.Send("Controls", "MultiChoiceActivate");
        }
        else
        {
            DeselectAllTasks();
            Signal.Send("Controls", "MainControlsActivate");
        }

        foreach (var task in Toolbox.Get<ListOfTasks>().Tasks)
        {
            task.SetCanSelect(multiChoice);
            task.CheckBox.OnToggleOnCallback.Event.AddListener(CountSelectedTasks);
            task.CheckBox.OnToggleOffCallback.Event.AddListener(CountSelectedTasks);
        }
    }



    private void CountSelectedTasks()
    {
        _selectedTasks.Clear();

        foreach (var task in Toolbox.Get<ListOfTasks>().Tasks)
        {
            if (task.Selected) _selectedTasks.Add(task);
        }

        _countSelectedTasksText.text = _selectedTasks.Count.ToString();
    }



    public void SelectAllTasks()
    {
        foreach (var task in Toolbox.Get<ListOfTasks>().Tasks)
        {
            if(!task.Selected) 
                task.SetSelected();
        }
    }



    public void DeselectAllTasks()
    {
        foreach (var task in Toolbox.Get<ListOfTasks>().Tasks)
        {
            if (task.Selected)
                task.SetSelected();
        }
    }



    public void DeleteTasks()
    {
        _selectedTasksId.Clear();
        foreach (var task in _selectedTasks)
        {
            _selectedTasksId.Add(task.ID);
        }

        foreach (var id in _selectedTasksId)
        {
            Toolbox.Get<Tables>().DeleteTask(id);
        }

        Toolbox.Get<ListOfTasks>().ReloadList();
    }
}
