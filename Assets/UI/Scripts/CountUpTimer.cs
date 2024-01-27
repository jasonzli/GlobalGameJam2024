using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountUpTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerTextTwo;

    private float secondsCount;
    private int minuteCount;
    private int hourCount;

    void Update()
    {
        UpdateTimerUI();
    }

    //call this on update
    public void UpdateTimerUI()
    {
        //set timer UI
        secondsCount += Time.deltaTime;
        timerText.text = minuteCount.ToString("00") + ":" + ((int)secondsCount).ToString("00");
        timerTextTwo.text = minuteCount.ToString("00") + ":" + ((int)secondsCount).ToString("00");
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        else if (minuteCount >= 60)
        {
            hourCount++;
            minuteCount = 0;
        }
    }

}
