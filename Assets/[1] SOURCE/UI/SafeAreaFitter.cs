using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaFitter : MonoBehaviour
{
    private void Awake()
    {
        ApplicationChrome.statusBarState = ApplicationChrome.States.VisibleOverContent;
        ApplicationChrome.statusBarColor = 0x00000000;
        //ApplicationChrome.navigationBarState = ApplicationChrome.States.TranslucentOverContent;

        var rectTransform = GetComponent<RectTransform>();
        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
