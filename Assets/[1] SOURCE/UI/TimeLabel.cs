using DG.Tweening;
using HutongGames.PlayMaker.Ecosystem.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum TimeLabelType
{
    Hour,
    Minute
}

public class TimeLabel : MonoBehaviour
{
    public TimeLabelType LabelType;

    public TextMeshProUGUI LabelText;

    public int TimeValue;

    public Clock Clock;

    public Image SelectIndicator;

    public bool Selected;

    public bool IsStartTime;

    public UnityEvent OnEndIncrement;

    public void SetTime()
    {
        Clock.SetTime(TimeValue, LabelType);
    }



    public void UpdateTime()
    {
        Clock.SetTime(TimeValue, LabelType, true);
    }



    public void ChangePeriod()
    {
        Clock.ChangePeriod();
    }



    public void SelectTime(bool select, bool isStartTime = false, Color indicatorColor = new Color())
    {
        Selected = select;
        IsStartTime = isStartTime;

        if (select)
        {
            SelectIndicator.gameObject.SetActive(true);
            SelectIndicator.color = indicatorColor;
            LabelText.color = Color.white;
        }
        else
        {
            SelectIndicator.gameObject.SetActive(false);
            LabelText.color = Color.black;
        }
    }



    public void LerpValue(int start, int end, float duration)
    {
        DOTween.To(() => start, x => { TimeValue = x; LabelText.text = TimeValue.ToString(); }, end, duration).OnUpdate(() => 
        { 
            if (Selected) 
            {
                UpdateTime();
            } 
        }).OnComplete(() => { OnEndIncrement?.Invoke(); });
    }



    public void LerpValueAmerican(bool up, float duration)
    {
        StartCoroutine(CalculateTimeAmerican(up, duration));
    }



    public GameObject GetClockGameObject()
    {
        return Clock.GetClockGameObject();
    }



    public bool GetStartTime()
    {
        return Clock.GetStartTime();
    }



    private IEnumerator CalculateTimeAmerican(bool up, float duration)
    {
        if (up)
        {
            for (int i = 0; i < 12; i++)
            {
                TimeValue++;
                if(TimeValue == 13)
                {
                    TimeValue = 1;
                }
                LabelText.text = TimeValue.ToString();
                UpdateTime();
                yield return new WaitForSeconds(duration / 12);
            }
            OnEndIncrement?.Invoke();
            yield return null;
        }
        else
        {
            for (int i = 0; i < 12; i++)
            {
                TimeValue--;
                if (TimeValue == 0)
                {
                    TimeValue = 12;
                }
                LabelText.text = TimeValue.ToString();
                UpdateTime();
                yield return new WaitForSeconds(duration / 12);
            }
            OnEndIncrement?.Invoke();
            yield return null;
        }
    }
}
