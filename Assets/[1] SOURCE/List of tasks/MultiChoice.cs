using Doozy.Runtime.Reactor.Animators;
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

    [SerializeField] private UIAnimator _listOfTasks;

    [SerializeField] private bool _multiChoiceEnabled;

    private void Awake()
    {
        Instance = this;  
    }



    public void SetMultiChoice(bool multiChoice)
    {
        if (multiChoice && !_multiChoiceEnabled)
        {
            Signal.Send("Controls", "MultiChoiceActivate");

            _listOfTasks.Play();

            foreach (var task in Toolbox.Get<ListOfTasks>().Tasks)
            {
                task.SetCanSelect(multiChoice);
                task.CheckBox.OnToggleOnCallback.Event.AddListener(CountSelectedTasks);
                task.CheckBox.OnToggleOffCallback.Event.AddListener(CountSelectedTasks);
            }

            _multiChoiceEnabled = true;
        }
        else if(!multiChoice && _multiChoiceEnabled)
        {
            DeselectAllTasks();
            Signal.Send("Controls", "MainControlsActivate");

            _listOfTasks.Reverse();

            foreach (var task in Toolbox.Get<ListOfTasks>().Tasks)
            {
                task.SetCanSelect(multiChoice);
                task.CheckBox.OnToggleOnCallback.Event.RemoveListener(CountSelectedTasks);
                task.CheckBox.OnToggleOffCallback.Event.RemoveListener(CountSelectedTasks);
            }
            _multiChoiceEnabled = false;
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

        foreach (var task in _selectedTasks)
        {
            Pooler.Instance.Despawn(PoolType.Entities, task.gameObject);
        }

        SetMultiChoice(false);
    }
}
