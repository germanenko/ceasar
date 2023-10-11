using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Doozy.Runtime.Colors.Models.HSL;

public class ListScaler : MonoBehaviour
{
    public bool Scaling;

    public SmoothGridLayoutUI LayoutUI;

    public GridLayoutGroup Group;

    public Rect Rect;

    private void Start()
    {
        LayoutUI = transform.parent.parent.GetComponent<SmoothGridLayoutUI>();
        Group = LayoutUI.gridLayoutGroup;
    }

    private void FixedUpdate()
    {
        if (Scaling)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(Group.cellSize.x, Group.cellSize.y);
        }
    }
}
