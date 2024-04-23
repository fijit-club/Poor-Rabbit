using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Action onGameBegin;
    public static Action onTutorialBegin;

    public static Shop Instance;
    public GameObject shopUI;
    public GameObject gameUI;
    public int selectedPlayer;
    public Image playerImage;
    public Sprite[] itemSprites;
    private int itemPrice;
    public string[] itemNames;
    public GameObject buyBtn;
    public GameObject useBtn;
    public TextMeshProUGUI totalCoins;
    private string itemName;
    public TextMeshProUGUI itemNameTxt;
    public int[] prices;
    public GameObject[] playerPrefabs;
    public GameObject[] playerPowers;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowSaveData(Bridge.GetInstance().thisPlayerInfo.data.saveData.value);
    }

    public void ShowSaveData(int value)
    {
        itemName = itemNames[value];
        //itemNameTxt.text = itemNames[value];
        playerImage.sprite= itemSprites[value];
        selectedPlayer = value;
        useBtn.SetActive(true);
        buyBtn.SetActive(false);

        ActivatePowerStatus(value);
    }

    public void Play()
    {
        GameBegin();
        Bridge.GetInstance().SaveData(selectedPlayer);
    }

    private void ActivatePowerStatus(int value)
    {
        for (int i = 0; i < playerPowers.Length; i++)
        {
            if (i == value)
            {
                playerPowers[i].SetActive(true);
            }
            else
            {
                playerPowers[i].SetActive(false);
            }
        }
    }

    public void GameBegin()
    {
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            if (i == Bridge.GetInstance().thisPlayerInfo.data.saveData.value)
            {
                playerPrefabs[i].SetActive(true);
            }
            else
            {
                playerPrefabs[i].SetActive(false);
            }
        }
        shopUI.SetActive(false);
        gameUI.SetActive(true);
        onGameBegin?.Invoke();
        GameManager.Instance.isGameOver = false;
    }


    public void Next()
    {
        selectedPlayer++;
        if (selectedPlayer == itemSprites.Length)
        {
            selectedPlayer = 0;
        }
       
        ActivateItems();
    }

    public void Previous()
    {
        selectedPlayer--;
        if (selectedPlayer == -1)
        {
            selectedPlayer = itemSprites.Length - 1;
        }
       
        ActivateItems();
    }


    public void BuyItem()
    {
        int totalCoin = Bridge.GetInstance().thisPlayerInfo.coins;
        if (itemPrice > totalCoin)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sounds.PurchaseFail);
            return;
        }
        SoundManager.Instance.PlaySound(SoundManager.Sounds.PurchaseSuccess);
        Bridge.GetInstance().UpdateCoins(-itemPrice);
        totalCoins.text = Bridge.GetInstance().thisPlayerInfo.coins.ToString();
        useBtn.SetActive(true);
        buyBtn.SetActive(false);
        Bridge.GetInstance().BuyPete("poor-bunny-"+itemName);
    }


    public void ActivateItems()
    {
        playerImage.sprite = itemSprites[selectedPlayer];
        //itemNameTxt.text = itemNames[selectedPlayer];
        itemName = itemNames[selectedPlayer];
        SoundManager.Instance.PlaySound(SoundManager.Sounds.ButtonClick);
        bool itemFound = false;
        foreach (Asset asset in Bridge.GetInstance().thisPlayerInfo.data.assets)
        {
            if (asset.id == "poor-bunny-" + itemName)
            {
                itemFound = true;
                break;
            }
        }

        if (itemFound)
        {
            useBtn.SetActive(true);
            buyBtn.SetActive(false);
        }
        else
        {
            if (selectedPlayer > 0)
            {
                itemPrice = prices[selectedPlayer];
                useBtn.SetActive(false);
                buyBtn.SetActive(true);
                buyBtn.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = itemPrice.ToString();
            }
            if (selectedPlayer == 0)
            {
                useBtn.SetActive(true);
                buyBtn.SetActive(false);
            }
        }
        ActivatePowerStatus(selectedPlayer);
    }
}
