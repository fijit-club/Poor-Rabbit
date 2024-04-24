using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReviveController : MonoBehaviour
{
    public static ReviveController Instance;
    public GameObject reviveUI;
    public GameObject gameUI;
    public TextMeshProUGUI timerTxt;
    public TextMeshProUGUI coinTxt;
    public Image timerDisplay;
    private bool isSecondChanceBought;
    private bool isReviveCanceled;
    public Coroutine reviveCoroutine;
    private float reviveUiTime=5;
    private int numberOfChances=0;
    private int buyCost = 500;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        coinTxt.text = Bridge.GetInstance().thisPlayerInfo.coins.ToString();
    }

    public void StartCountdown(float duration)
    {
        StartCoroutine(DecreaseFillAmountOverTime(duration));
    }

    private IEnumerator DecreaseFillAmountOverTime(float duration)
    {
        float timer = duration;
        float startFillAmount = timerDisplay.fillAmount;

        while (timer > 0f)
        {
            timerTxt.text =((int) timer+1).ToString();
            timer -= Time.deltaTime;
            float fillAmount = Mathf.Lerp(0f, startFillAmount, timer / duration);

            timerDisplay.fillAmount = fillAmount;

            yield return null;
        }
    }

    public void GameEnd()
    {
        if (Bridge.GetInstance().thisPlayerInfo.coins < 500)
        {
            GameManager.Instance.GameOver();
            return;
        }

        reviveCoroutine = StartCoroutine(GameEndDelay());
        StartCountdown(reviveUiTime);
    }


    public IEnumerator GameEndDelay()
    {
        if (numberOfChances == 0)
        {
            reviveUI.SetActive(true);
        }
        else
        {
            GameManager.Instance.GameOver();
        }
        yield return new WaitForSeconds(5);

        if (!isSecondChanceBought && numberOfChances == 0 && !isReviveCanceled)
        {
            GameManager.Instance.GameOver();
            reviveUI.SetActive(false);
        }

    }

    public void BuySecondChance()
    {
        int totalCoin = Bridge.GetInstance().thisPlayerInfo.coins;
        if (totalCoin < buyCost)
        {
            return;
        }
        if (reviveCoroutine != null)
        {
            StopCoroutine(GameEndDelay());
            reviveCoroutine = null;
        }
        Bridge.GetInstance().UpdateCoins(-buyCost);
        numberOfChances++;
        isSecondChanceBought = true;
        reviveUI.SetActive(false);
        gameUI.SetActive(true);
        CharacterController.Instance.Revive();
    }

    public void SkipRevive()
    {
        isReviveCanceled = true;
        reviveUI.SetActive(false);
        if (reviveCoroutine != null)
        {
            StopCoroutine(GameEndDelay());
            reviveCoroutine = null;
        }
        GameManager.Instance.GameOver();
    }
}
