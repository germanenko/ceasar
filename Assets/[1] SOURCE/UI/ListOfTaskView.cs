using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Animators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfTaskView : MonoBehaviour
{
    [SerializeField] private UIContainerUIAnimator _viewAnimator;


    public void SignalListen(Signal signal)
    {
        if (signal.stream.category == "Swipe")
        {
            if (signal.stream.name == "Left")
            {
                _viewAnimator.showAnimation.Move.fromDirection = Doozy.Runtime.Reactor.MoveDirection.Right;
                _viewAnimator.hideAnimation.Move.toDirection = Doozy.Runtime.Reactor.MoveDirection.Left;
            }
            else if (signal.stream.name == "Right")
            {
                _viewAnimator.showAnimation.Move.fromDirection = Doozy.Runtime.Reactor.MoveDirection.Left;
                _viewAnimator.hideAnimation.Move.toDirection = Doozy.Runtime.Reactor.MoveDirection.Right;
            }
        }
    }
}
