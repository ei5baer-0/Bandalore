using UnityEngine;
using TMPro;
using System.Collections;

public class Countdown : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text countdownTMP;
    [SerializeField] private LevelLoader loader;

    [Header("Variables")]
    [SerializeField] private float countdownTime = 3f;

    private int currentSeconds;
    private string formattedTime;
    private float totalTime;
    public bool gameStart;

    void Start()
    {
        totalTime = loader.transitionTime + countdownTime;
        countdownTMP.enabled = true;
        gameStart = false;
    }

    void Update()
    {
        if (totalTime > 0)
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
        totalTime -= Time.deltaTime;
        totalTime = Mathf.Max(totalTime, 0f);
    }

    private void TimeCalculation()
    {
        currentSeconds = Mathf.FloorToInt(totalTime % 60f);
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
