using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform[] carrotSpawnPos;
    public GameObject cannonEffect;
    public GameObject[] weapons;
    public GameObject gameOverScreen;
    private List<int> availableIndices = new List<int>();
    public bool isGameOver;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI highScoreTxt;
    public TextMeshProUGUI coinTxt;
    public RectTransform parent;
    public RectTransform coinDestination;
    public GameObject coinPref;
    public GameObject arrowSpawner;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Shop.onGameBegin += GameBegin;
    }

    private void OnDisable()
    {
        Shop.onGameBegin -= GameBegin;
    }

    private void GameBegin()
    {
        SoundManager.Instance.PlaySoundLoop(SoundManager.Sounds.BGM);
        SpawnCarrots();
        StartCoroutine(ActivateRandomWeapons());

        for (int i = 0; i < weapons.Length; i++)
        {
            availableIndices.Add(i);
        }
        arrowSpawner.SetActive(true);
    }

    public void SpawnCarrots()
    {
        CarrotSpawner(ObjectPooler.Prefabs.carrot, carrotSpawnPos[Random.Range(0, carrotSpawnPos.Length)]);
    }

    private void CarrotSpawner(ObjectPooler.Prefabs name, Transform pos)
    {
        GameObject obj = ObjectPooler.instance.GetObject(name);
        obj.SetActive(true);
        obj.transform.position = pos.position;
    }

    public void CannonEffect(Transform pos)
    {
        Destroy(Instantiate(cannonEffect, pos.position-new Vector3(0,0.15f,0),Quaternion.identity,transform), 1f);
    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator ActivateRandomWeapons()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);

            if (availableIndices.Count == 0)
            {
                Debug.Log("No more weapons available.");
                yield break;
            }

            int randomIndex = Random.Range(0, availableIndices.Count);
            int index = availableIndices[randomIndex];
            availableIndices.RemoveAt(randomIndex);

            weapons[index].SetActive(true);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
        Pause.Instance.gamePanel.SetActive(false);
        SoundManager.Instance.PlaySound(SoundManager.Sounds.EndGame);
    }

    public void CoinAnimation(int count, Vector2 pos)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject coin = Instantiate(coinPref, parent);
            coin.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(pos);
            float delayTime = 0.1f;
            LeanTween.delayedCall(delayTime, () =>
            {
                LeanTween.move(coin, coinDestination.position, 0.5f)
                         .setEase(LeanTweenType.animationCurve)
                         .setOnComplete(() => { Destroy(coin); Bridge.GetInstance().UpdateCoins(1); });
            });
        }
    }

    public void UpdateCoinAndScore()
    {
        scoreTxt.text = CharacterController.Instance.GetScore().ToString();
        coinTxt.text = CharacterController.Instance.GetScore().ToString();
        highScoreTxt.text = Bridge.GetInstance().thisPlayerInfo.highScore.ToString();
    }
}
