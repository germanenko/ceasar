using UnityEngine;
using Germanenko.Source;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class LerpToPlaceholder : MonoBehaviour
{
    private RectTransform _rectTransform;
    
    public SmoothGridLayoutUI smoothGridLayout;
    public Transform placeholderTransform;

    public ItemOfList Item;

    public bool BlockMoving;
    public bool InstantlyMove;
    public bool PositionReached;

    private void Start()
    {   
        _rectTransform = GetComponent<RectTransform>();
        Item = GetComponent<ItemOfList>();
    }



    private void OnDisable()
    {
        BlockMoving = false;
    }



    private void Update()
    {
        if (_rectTransform == null || placeholderTransform == null || smoothGridLayout == null) return;
        if (placeholderTransform.transform.position.sqrMagnitude < 1) return;
        if (gameObject.activeSelf && BlockMoving) return;
        if(Item && Item.isDragging) return;
        if (InstantlyMove)
        {
            transform.position = Vector3.Lerp(transform.position, placeholderTransform.position, Time.deltaTime * 300);
            InstantlyMove = false;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, placeholderTransform.position, Time.deltaTime * smoothGridLayout.lerpSpeed);

            if (Vector3.Distance(transform.position, placeholderTransform.position) > 0.1f)
            {
                PositionReached = false;
            }
            else
            {
                PositionReached = true;
            }
        }           
    }



    public void SetInstantlyMove(bool instantly)
    {
        InstantlyMove = instantly;
    }



    public void DelayMove()
    {
        StartCoroutine(DelayMoveCoroutine());
    }



    IEnumerator DelayMoveCoroutine()
    {
        BlockMoving = true;

        yield return new WaitForSeconds(.05f);

        BlockMoving = false;
    }
}
