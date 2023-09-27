using UnityEngine;
using Germanenko.Source;
using Unity.VisualScripting;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class LerpToPlaceholder : MonoBehaviour
{
    private RectTransform _rectTransform;
    
    public SmoothGridLayoutUI smoothGridLayout;
    public Transform placeholderTransform;

    public ItemOfList Item;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        Item = GetComponent<ItemOfList>();
    }

    private void Update()
    {
        if (_rectTransform == null || placeholderTransform == null || smoothGridLayout == null) return;
        if (placeholderTransform.transform.position.sqrMagnitude < 1) { print("stop"); return; }
        if(Item && Item.isDragging) return;
        transform.position = Vector3.Lerp(transform.position, placeholderTransform.position, Time.deltaTime * smoothGridLayout.lerpSpeed);
    }
}
