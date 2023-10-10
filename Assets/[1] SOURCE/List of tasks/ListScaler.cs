using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListScaler : MonoBehaviour
{
    public float OldDistance;
    public float NewDistance;

    public GridLayoutGroup GridLayout;

    void Update()
    {
        if(Input.touches.Length == 2)
        {
            NewDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

            if (Mathf.Abs(NewDistance - OldDistance) >= 2) 
            {
                print("da");
                
                if (NewDistance < OldDistance)
                {
                    GridLayout.cellSize = new Vector2(GridLayout.cellSize.x + Mathf.Abs(NewDistance - OldDistance) / 50, GridLayout.cellSize.y + Mathf.Abs(NewDistance - OldDistance) / 50);
                    print(GridLayout.cellSize.x);
                }
                else if(NewDistance > OldDistance)
                {
                    GridLayout.cellSize = new Vector2(GridLayout.cellSize.x - Mathf.Abs(NewDistance - OldDistance) / 50, GridLayout.cellSize.y - Mathf.Abs(NewDistance - OldDistance) / 50);
                    print(GridLayout.cellSize.x);
                }
                OldDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

            }
        }
    }
}
