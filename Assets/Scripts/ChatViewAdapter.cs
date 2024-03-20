/*
 * * * * This bare-bones script was auto-generated * * * *
 * The code commented with "/ * * /" demonstrates how data is retrieved and passed to the adapter, plus other common commands. You can remove/replace it once you've got the idea
 * Complete it according to your specific use-case
 * Consult the Example scripts if you get stuck, as they provide solutions to most common scenarios
 * 
 * Main terms to understand:
 *		Model = class that contains the data associated with an item (title, content, icon etc.)
 *		Views Holder = class that contains references to your views (Text, Image, MonoBehavior, etc.)
 * 
 * Default expected UI hiererchy:
 *	  ...
 *		-Canvas
 *		  ...
 *			-MyScrollViewAdapter
 *				-Viewport
 *					-Content
 *				-Scrollbar (Optional)
 *				-ItemPrefab (Optional)
 * 
 * Note: If using Visual Studio and opening generated scripts for the first time, sometimes Intellisense (autocompletion)
 * won't work. This is a well-known bug and the solution is here: https://developercommunity.visualstudio.com/content/problem/130597/unity-intellisense-not-working-after-creating-new-1.html (or google "unity intellisense not working new script")
 * 
 * 
 * Please read the manual under "/Docs", as it contains everything you need to know in order to get started, including FAQ
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Other.Extensions;
using Com.ForbiddenByte.OSA.Core;
using Com.ForbiddenByte.OSA.CustomParams;
using Com.ForbiddenByte.OSA.DataHelpers;
using TMPro;

// There are 2 important callbacks you need to implement, apart from Start(): CreateViewsHolder() and UpdateViewsHolder()
// See explanations below
public class ChatViewAdapter : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
    public SimpleDataHelper<ChatMessageItemModel> Data { get; private set; }


    #region OSA implementation
    protected override void Awake()
    {
        base.Awake();

        Data = new SimpleDataHelper<ChatMessageItemModel>(this);
    }

    /// <inheritdoc/>
    protected override void Update()
    {
        base.Update();

        if (!IsInitialized)
            return;

        for (int i = 0; i < VisibleItemsCount; i++)
        {
            var visibleVH = GetItemViewsHolder(i);
            if (visibleVH.IsPopupAnimationActive)
                visibleVH.UpdatePopupAnimation(Time);
        }
    }

    /// <inheritdoc/>
    protected override MyListItemViewsHolder CreateViewsHolder(int itemIndex)
    {
        var instance = new MyListItemViewsHolder();
        instance.Init(_Params.ItemPrefab, _Params.Content, itemIndex);

        return instance;
    }

    /// <inheritdoc/>
    protected override void OnItemHeightChangedPreTwinPass(MyListItemViewsHolder vh)
    {
        base.OnItemHeightChangedPreTwinPass(vh);

        Data[vh.ItemIndex].HasPendingVisualSizeChange = false;
    }

    /// <inheritdoc/>
    protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
    {
        // Initialize the views from the associated model
        ChatMessageItemModel model = Data[newOrRecycled.ItemIndex];

        newOrRecycled.UpdateFromModel(model);

        if (model.HasPendingVisualSizeChange)
        {
            // Height will be available before the next 'twin' pass, inside OnItemHeightChangedPreTwinPass() callback (see above)
            newOrRecycled.MarkForRebuild(); // will enable the content size fitter
                                            //newOrRecycled.contentSizeFitter.enabled = true;
            ScheduleComputeVisibilityTwinPass(true);
        }
        if (!newOrRecycled.IsPopupAnimationActive && newOrRecycled.itemIndexInView == GetItemsCount() - 1) // only animating the last one
            newOrRecycled.ActivatePopulAnimation(Time);
    }

    /// <inheritdoc/>
    protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
    {
        inRecycleBinOrVisible.DeactivatePopupAnimation();

        base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
    }

    /// <inheritdoc/>
    protected override void RebuildLayoutDueToScrollViewSizeChange()
    {
        // Invalidate the last sizes so that they'll be re-calculated
        SetAllModelsHavePendingSizeChange();

        base.RebuildLayoutDueToScrollViewSizeChange();
    }

    /// <summary>
    /// When the user resets the count or refreshes, the OSA's cached sizes are cleared so we can recalculate them. 
    /// This is provided here for new users that just want to call Refresh() and have everything updated instead of telling OSA exactly what has updated.
    /// But, in most cases you shouldn't need to ResetItems() or Refresh() because of performace reasons:
    /// - If you add/remove items, InsertItems()/RemoveItems() is preferred if you know exactly which items will be added/removed;
    /// - When just one item has changed externally and you need to force-update its size, you'd call ForceRebuildViewsHolderAndUpdateSize() on it;
    /// - When the layout is rebuilt (when you change the size of the viewport or call ScheduleForceRebuildLayout()), that's already handled
    /// So the only case when you'll need to call Refresh() (and override ChangeItemsCount()) is if your models can be changed externally and you'll only know that they've changed, but won't know which ones exactly.
    /// </summary>
    public override void ChangeItemsCount(ItemCountChangeMode changeMode, int itemsCount, int indexIfInsertingOrRemoving = -1, bool contentPanelEndEdgeStationary = false, bool keepVelocity = false)
    {
        if (changeMode == ItemCountChangeMode.RESET)
            SetAllModelsHavePendingSizeChange();

        base.ChangeItemsCount(changeMode, itemsCount, indexIfInsertingOrRemoving, contentPanelEndEdgeStationary, keepVelocity);
    }
    #endregion

    void SetAllModelsHavePendingSizeChange()
    {
        foreach (var model in Data)
            model.HasPendingVisualSizeChange = true;
    }
}

// Class containing the data associated with an item
public class ChatMessageItemModel
{
    public MessageBody Message;
    public bool IsMine;
    public bool HasPendingVisualSizeChange = true;

}


// This class keeps references to an item's views.
// Your views holder should extend BaseItemViewsHolder for ListViews and CellViewsHolder for GridViews
public class MyListItemViewsHolder : BaseItemViewsHolder
{
    public TextMeshProUGUI timeText, text;
    public Image leftIcon, rightIcon;
    public Image image;
    public Image messageContentPanelImage;

    UnityEngine.UI.ContentSizeFitter ContentSizeFitter { get; set; }
    public float PopupAnimationStartTime { get; private set; }
    public bool IsPopupAnimationActive
    {
        get { return _IsAnimating; }
    }

    const float POPUP_ANIMATION_TIME = .2f;

    bool _IsAnimating;
    VerticalLayoutGroup _RootLayoutGroup, _MessageContentLayoutGroup;
    int paddingAtIconSide, paddingAtOtherSide;
    Color colorAtInit;


    public override void CollectViews()
    {
        base.CollectViews();

        _RootLayoutGroup = root.GetComponent<VerticalLayoutGroup>();
        paddingAtIconSide = _RootLayoutGroup.padding.right;
        paddingAtOtherSide = _RootLayoutGroup.padding.left;

        ContentSizeFitter = root.GetComponent<UnityEngine.UI.ContentSizeFitter>();
        ContentSizeFitter.enabled = false; // the content size fitter should not be enabled during normal lifecycle, only in the "Twin" pass frame
        root.GetComponentAtPath("MessageContentPanel", out _MessageContentLayoutGroup);
        messageContentPanelImage = _MessageContentLayoutGroup.GetComponent<Image>();
        messageContentPanelImage.transform.GetComponentAtPath("Image", out image);
        messageContentPanelImage.transform.GetComponentAtPath("TimeText", out timeText);
        messageContentPanelImage.transform.GetComponentAtPath("Text", out text);
        root.GetComponentAtPath("LeftIconImage", out leftIcon);
        root.GetComponentAtPath("RightIconImage", out rightIcon);
        colorAtInit = messageContentPanelImage.color;
    }

    public override void MarkForRebuild()
    {
        base.MarkForRebuild();
        if (ContentSizeFitter)
            ContentSizeFitter.enabled = true;
    }

    public override void UnmarkForRebuild()
    {
        if (ContentSizeFitter)
            ContentSizeFitter.enabled = false;
        base.UnmarkForRebuild();
    }

    /// <summary>Utility getting rid of the need of manually writing assignments</summary>
    public void UpdateFromModel(ChatMessageItemModel model)
    {
        string messageText = model.Message.Content;
        if (text.text != messageText)
            text.text = messageText;

        timeText.text = model.Message.Date.ToLocalTime().ToString("HH:mm");

        //leftIcon.gameObject.SetActive(!model.IsMine);
        //rightIcon.gameObject.SetActive(model.IsMine);

        if (model.IsMine)
        {
            messageContentPanelImage.rectTransform.pivot = new Vector2(1.4f, .5f);
            messageContentPanelImage.color = colorAtInit;
            _RootLayoutGroup.childAlignment = _MessageContentLayoutGroup.childAlignment = TextAnchor.MiddleRight;
            //text.alignment = TextAlignmentOptions.MidlineRight;
            _RootLayoutGroup.padding.right = paddingAtIconSide;
            _RootLayoutGroup.padding.left = paddingAtOtherSide;
        }
        else
        {
            messageContentPanelImage.rectTransform.pivot = new Vector2(-.4f, .5f);
            messageContentPanelImage.color = new Color(.75f, 1f, 1f, colorAtInit.a);
            _RootLayoutGroup.childAlignment = _MessageContentLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
            //text.alignment = TextAlignmentOptions.MidlineLeft;
            _RootLayoutGroup.padding.right = paddingAtOtherSide;
            _RootLayoutGroup.padding.left = paddingAtIconSide;
        }
    }

    public void DeactivatePopupAnimation()
    {
        messageContentPanelImage.transform.localScale = Vector3.one;
        _IsAnimating = false;
    }

    public void ActivatePopulAnimation(float unityTime)
    {
        var s = messageContentPanelImage.transform.localScale;
        s.x = 0;
        messageContentPanelImage.transform.localScale = s;
        PopupAnimationStartTime = unityTime;
        _IsAnimating = true;
    }

    internal void UpdatePopupAnimation(float unityTime)
    {
        float elapsed = unityTime - PopupAnimationStartTime;
        float t01;
        if (elapsed > POPUP_ANIMATION_TIME)
            t01 = 1f;
        else
            // Normal in, sin slow out
            t01 = Mathf.Sin((elapsed / POPUP_ANIMATION_TIME) * Mathf.PI / 2);

        var s = messageContentPanelImage.transform.localScale;
        s.x = t01;
        messageContentPanelImage.transform.localScale = s;

        if (t01 == 1f)
            DeactivatePopupAnimation();

        //Debug.Log("Updating: " + itemIndexInView + ", t01=" + t01 + ", elapsed=" + elapsed);
    }
    //public TextMeshProUGUI titleText;
    //public Image backgroundImage;



    // Retrieving the views from the item's root GameObject
    //public override void CollectViews()
    //{
    //	base.CollectViews();

    //	// GetComponentAtPath is a handy extension method from frame8.Logic.Misc.Other.Extensions
    //	// which infers the variable's component from its type, so you won't need to specify it yourself

    //	root.GetComponentAtPath("TitleText", out titleText);
    //	root.GetComponentAtPath("BackgroundImage", out backgroundImage);

    //}

    // Override this if you have children layout groups or a ContentSizeFitter on root that you'll use. 
    // They need to be marked for rebuild when this callback is fired
    /*
	public override void MarkForRebuild()
	{
		base.MarkForRebuild();

		LayoutRebuilder.MarkLayoutForRebuild(yourChildLayout1);
		LayoutRebuilder.MarkLayoutForRebuild(yourChildLayout2);
		YourSizeFitterOnRoot.enabled = true;
	}
	*/

    // Override this if you've also overridden MarkForRebuild() and you have enabled size fitters there (like a ContentSizeFitter)
    /*
	public override void UnmarkForRebuild()
	{
		YourSizeFitterOnRoot.enabled = false;

		base.UnmarkForRebuild();
	}
	*/
}

