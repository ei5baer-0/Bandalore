using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Countdown : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text countdownTMP;

    [Header("Variables")]
    [SerializeField] private float countdownTime = 3f;

    private int currentSeconds;
    private string formattedTime;
    public bool gameStart;

    void Start()
    {
        countdownTMP.enabled = true;
        gameStart = false;
    }

    void Update()
    {
        if (countdownTime > 0)
        {
            Counter();
            TimeCalculation();
            TimeOutput();
        }
        else
        {
            CountdownExpired();
        }
    }

    private void Counter()
    {
        countdownTime -= Time.deltaTime;
        countdownTime = Mathf.Max(countdownTime, 0f);
    }

    private void TimeCalculation()
    {
        currentSeconds = Mathf.FloorToInt(countdownTime % 60f);
        formattedTime = $"{currentSeconds:0}";
    }

    private void TimeOutput()
    {
        countdownTMP.text = formattedTime;

    }
    private void CountdownExpired()
    {
        countdownTMP.enabled = false;
        gameStart = true;
    }
}
