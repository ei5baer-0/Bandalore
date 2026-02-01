using UnityEngine;
using System.Collections;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text clockTMP;
    [SerializeField] private Countdown Countdown;

    [Header("Variables")]
    private float currentTime;
    private int currentMinutes;
    private int currentSeconds;
    private int currentMilliseconds;
    public string formattedTime;

    private bool runFinished;

    void Start()
    {
        runFinished = false;
    }

    void Update()
    {
        if (Countdown.gameStart == true && runFinished == false)
        {
            Counter();
            TimeCalculation();
            TimeOutput();
        }
    }

    private void Counter()
    {
        currentTime += Time.deltaTime;
        currentTime = Mathf.Max(currentTime, 0f);
    }

    private void TimeCalculation()
    {
        currentMinutes = Mathf.FloorToInt(currentTime / 60f);
        currentSeconds = Mathf.FloorToInt(currentTime % 60f);
        currentMilliseconds = Mathf.FloorToInt((currentTime * 1000f) % 1000f);

        formattedTime = $"{currentMinutes:00}:{currentSeconds:00}.{currentMilliseconds:000}";
    }

    private void TimeOutput()
    {
        clockTMP.text = formattedTime;
    }

    private void TimeStop()
    {
        if (!runFinished)
        {
            runFinished = true;
        }
        else
        {
            runFinished = false;
        }
    }
}