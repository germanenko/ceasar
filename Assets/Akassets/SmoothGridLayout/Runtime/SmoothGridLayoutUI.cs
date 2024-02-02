using Germanenko.Framework;
using Germanenko.Source;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class SmoothGridLayoutUI : MonoBehaviour
{
    [Range(0f, 1000f)]
    public float lerpSpeed = 10;
    public RectTransform placeholdersTransform;
    public RectTransform elementsTransform;
    public ElementsContainer elementsContainer;
    public GridLayoutGroup gridLayoutGroup;

    public UnityEvent OnRebuilded;

    private void Awake()
    {
        if(placeholdersTransform == null)
            Debug.LogError("<color=lightblue><b>Smooth Grid Layout</b></color> ► <color=red>\"Placeholders Transform\" is null.</color> Assign field in the inspector or use autofix.", this);
        if(elementsTransform == null)
            Debug.LogError("<color=lightblue><b>Smooth Grid Layout</b></color> ► <color=red>\"Elements Transform\" is null.</color> Assign field in the inspector or use autofix.", this);
        if(elementsContainer == null)
            Debug.LogError("<color=lightblue><b>Smooth Grid Layout</b></color> ► <color=red>\"Elements Container\" is null.</color> Assign field in the inspector or use autofix.", this);
        if(gridLayoutGroup == null)
            Debug.LogError("<color=lightblue><b>Smooth Grid Layout</b></color> ► <color=red>\"Elements Transform\" is null.</color> Assign field in the inspector or use autofix.", this);

        elementsContainer.OnChildrenChanged += RebuildChildren;
    }

    private void Start()
    {
        RebuildChildren();
    }

    private void OnDestroy()
    {
        elementsContainer.OnChildrenChanged -= RebuildChildren;
    }

    private void RebuildChildren()
    {
        var childTransforms = placeholdersTransform.Cast<Transform>().ToList();
        foreach (var child in childTransforms)
            Destroy(child.gameObject);

        //for (int i = 0; i < placeholdersTransform.childCount; i++) 
        //{
        //    placeholdersTransform.GetChild(i).gameObject.SetActive(false);
        //}


        foreach (Transform element in elementsTransform)
        {
            if (element.gameObject.activeSelf && element.TryGetComponent(out LerpToPlaceholder lerpToPlaceholder)) 
                lerpToPlaceholder.DelayMove();
            if (element.TryGetComponent(out RectTransform rectTransform))
            {
                AddElement(rectTransform);
            }
            else
            {
                var rect = element.gameObject.AddComponent<RectTransform>();
                AddElement(rect);
            }
        }

        OnRebuilded?.Invoke();
    }

    private void AddElement(RectTransform element)
    {
        //try
        //{
        //    if (placeholdersTransform.GetChild(element.GetSiblingIndex()))
        //    {
        //        var p = placeholdersTransform.GetChild(element.GetSiblingIndex());
        //        p.gameObject.SetActive(true);

        //        if (element.gameObject.TryGetComponent(out LerpToPlaceholder lerpToPlaceholderr))
        //        {
        //            lerpToPlaceholderr.placeholderTransform = p.GetComponent<RectTransform>();
        //            lerpToPlaceholderr.smoothGridLayout = this;
        //            return;
        //        }

        //        lerpToPlaceholderr = element.gameObject.AddComponent<LerpToPlaceholder>();
        //        lerpToPlaceholderr.placeholderTransform = p.GetComponent<RectTransform>();
        //        lerpToPlaceholderr.smoothGridLayout = this;

        //        return;
        //    }
        //}
        //catch(Exception ex)
        //{
            
        //}

        var placeholder = new GameObject($"{element.name} placeholder");
        var placeholderRect = placeholder.AddComponent<RectTransform>();
        placeholder.AddComponent<LayoutElement>();
        //element.anchorMax = new Vector2(0.5f, 0.5f);
        //element.anchorMin = new Vector2(0.5f, 0.5f);
        element.sizeDelta = gridLayoutGroup.cellSize;
        placeholderRect.sizeDelta = gridLayoutGroup.cellSize;
        placeholder.transform.SetParent(placeholdersTransform);
        placeholder.transform.localPosition = new Vector3(placeholder.transform.localPosition.x, placeholder.transform.localPosition.y, 0);

        if (element.gameObject.TryGetComponent(out LerpToPlaceholder lerpToPlaceholder))
        {
            lerpToPlaceholder.placeholderTransform = placeholderRect;
            lerpToPlaceholder.smoothGridLayout = this;
            return;
        }

        lerpToPlaceholder = element.gameObject.AddComponent<LerpToPlaceholder>();
        lerpToPlaceholder.placeholderTransform = placeholderRect;
        lerpToPlaceholder.smoothGridLayout = this;
    }
}
