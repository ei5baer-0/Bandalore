using UnityEngine;
using System.Collections;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text clockTMP;
    [SerializeField] private Countdown Countdown;

    [Header("Variables")]
    [SerializeField] private float penaltySeconds = 5f;
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeMagnitude = 5f;
    [SerializeField] private Color penaltyColor = Color.red;

    private float currentTime;
    private int currentMinutes;
    private int currentSeconds;
    private int currentMilliseconds;
    private string formattedTime;
    private Color originalColor;
    private Vector3 originalPosition;
    private Vector3 originalScale;

    private bool runFinished;

    void Start()
    {
        runFinished = false;
        originalColor = clockTMP.color;
        originalPosition = clockTMP.rectTransform.localPosition;
        originalScale = clockTMP.rectTransform.localScale;
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

    public void AddPenalty()
    {
        currentTime += penaltySeconds;

        if (clockTMP != null)
            StartCoroutine(PenaltyFeedbackRoutine());
    }

    private IEnumerator PenaltyFeedbackRoutine()
    {
        clockTMP.color = penaltyColor;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            float yOffset = Random.Range(-shakeMagnitude / 2f, shakeMagnitude / 2f);
            clockTMP.rectTransform.localPosition = originalPosition + new Vector3(xOffset, yOffset, 0f);
            clockTMP.rectTransform.localScale = originalScale + new Vector3(0.1f, 0.1f, 0f);

            yield return null;
        }

        clockTMP.rectTransform.localScale = originalScale;
        clockTMP.rectTransform.localPosition = originalPosition;
        clockTMP.color = originalColor;
    }
}