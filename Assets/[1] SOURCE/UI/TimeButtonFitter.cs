using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeButtonFitter : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect screenRect;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
    }

    void Update()
    {
        ClampRectTransformToScreen(rectTransform, screenRect);
    }

    public static void ClampRectTransformToScreen(RectTransform rectTransform, Rect screenRect)
    {
        Vector2 position = rectTransform.position;
        position.x = Mathf.Clamp(position.x, screenRect.xMin, screenRect.xMax - rectTransform.rect.width);
        position.y = Mathf.Clamp(position.y, screenRect.yMin, screenRect.yMax - rectTransform.rect.height);
        rectTransform.position = position;
    }
}
