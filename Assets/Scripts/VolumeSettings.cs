using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Slider linkedSlider;

    [Header("Variables")]
    [SerializeField] private float lowThreshold = 35f;
    [SerializeField] private float midThreshold = 75f;

    void Update()
    {
        if (linkedSlider == null) return;

        float soundValue = linkedSlider.value;

        if (soundValue < 1)
            animator.Play("Off");

        else if (soundValue < lowThreshold)

            animator.Play("Low");
        else if (soundValue < midThreshold)

            animator.Play("Mid");
        else
            animator.Play("High");
    }
}
