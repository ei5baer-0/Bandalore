using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animatorUI;

    private Animator animator;

    private float totalTime;
    private float randomCounter;

    private bool collected = false;
    private bool collectedUI = false;

    void Start()
    {
        randomCounter = Random.Range(4, 8);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update ()
    {   
        Counter();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected) return;

        if (collision.CompareTag("Player"))
        {
            collected = true;
            animator.Play("CollectedCoin");
            animatorUI.Play("CollectedCoinUI");

            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(UIGlowWait());
        }
    }

    private void Counter()
    {
        totalTime += Time.deltaTime;
        totalTime = Mathf.Max(totalTime, 0f);

        if (randomCounter <= totalTime)
        {
            totalTime = 0f;
            randomCounter = Random.Range(4, 8);
            CoinGlow();
        }
    }

    private void CoinGlow()
    {
        if (collectedUI) animatorUI.Play("CoinGlowUI");
        if (collected == false) animator.Play("CoinGlow");
    }

    private IEnumerator UIGlowWait()
    {
        yield return new WaitForSeconds(2f);
        collectedUI = true;
    }
}