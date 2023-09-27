using System;
using UnityEngine;


public class ElementsContainer : MonoBehaviour
{
    public event Action OnChildrenChanged;

    private void OnTransformChildrenChanged()
    {
        OnChildrenChanged?.Invoke();
    }
}

