/// Credit srinivas sunil 
/// sourced from: https://bitbucket.org/SimonDarksideJ/unity-ui-extensions/pull-requests/21/develop_53/diff
/// Updated by Hiep Eldest : https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/issues/300/scrollconflictmanager-not-working-if

using UnityEngine.EventSystems;

/// <summary>
/// This is the most efficient way to handle scroll conflicts when there are multiple scroll rects, this is useful when there is a vertical scrollrect in/on a horizontal scrollrect or vice versa
/// Attach the script to the  rect scroll and assign other rectscroll in the inspector (one is vertical and other is horizontal) gathered and modified from unity answers(delta snipper)
/// </summary>
namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("UI/Extensions/Scrollrect Conflict Manager")]
    public class ScrollConflictManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [Tooltip("The parent ScrollRect control hosting this ScrollSnap")]
        public ScrollRect ParentScrollRect;

        private IBeginDragHandler[] _beginDragHandlers;
        private IEndDragHandler[] _endDragHandlers;
        private IDragHandler[] _dragHandlers;
        //This tracks if the other one should be scrolling instead of the current one.
        public bool scrollOther;



        void Start()
        {
            if (ParentScrollRect)
            {
                AssignScrollRectHandlers();
            }
        }



        private void AssignScrollRectHandlers()
        {
            _beginDragHandlers = ParentScrollRect.GetComponents<IBeginDragHandler>();
            _dragHandlers = ParentScrollRect.GetComponents<IDragHandler>();
            _endDragHandlers = ParentScrollRect.GetComponents<IEndDragHandler>();
        }



        public void SetParentScrollRect(ScrollRect parentScrollRect)
        {
            ParentScrollRect = parentScrollRect;
            AssignScrollRectHandlers();
        }

        #region DragHandler



        public void OnBeginDrag(PointerEventData eventData)
        {
            scrollOther = true;
            //disable the current scroll rect so it does not move.
            for (int i = 0, length = _beginDragHandlers.Length; i < length; i++)
            {
                _beginDragHandlers[i].OnBeginDrag(eventData);
            }



            scrollOther = true;
            //disable the current scroll rect so it does not move.
            for (int i = 0, length = _beginDragHandlers.Length; i < length; i++)
            {
                _beginDragHandlers[i].OnBeginDrag(eventData);
            }
        }



        public void OnEndDrag(PointerEventData eventData)
        {
            if (scrollOther)
            {
                scrollOther = false;
                for (int i = 0, length = _endDragHandlers.Length; i < length; i++)
                {
                    _endDragHandlers[i].OnEndDrag(eventData);
                }
            }
        }



        public void OnDrag(PointerEventData eventData)
        {
            if (scrollOther)
            {
                for (int i = 0, length = _endDragHandlers.Length; i < length; i++)
                {
                    _dragHandlers[i].OnDrag(eventData);
                }
            }

            #endregion DragHandler
        }
    }
}