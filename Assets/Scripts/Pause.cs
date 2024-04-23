using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pause : MonoBehaviour
{
    public static Pause Instance;
    public GameObject muteImage;
    public GameObject unMuteImage;
    public GameObject vibrateImage;
    public GameObject vibrateOffImage;
    public GameObject pausePanel;
    public GameObject gamePanel;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI highScoreTxt;
    public TextMeshProUGUI coinTxt;
    public List<GameObject> lifeSymbols;
    public bool isVibrating;


    private void Awake()
    {
        Instance = this;
    }

    public void UpdatePauseMenu()
    {
        Score();
        Coin();
        HighScore();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        gamePanel.SetActive(false);
        SoundManager.Instance.PlaySound(SoundManager.Sounds.ButtonClick);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        gamePanel.SetActive(true);
        SoundManager.Instance.PlaySound(SoundManager.Sounds.ButtonClick);
    }

    public void Reload()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.Reload();
        Time.timeScale = 1;
        SoundManager.Instance.PlaySound(SoundManager.Sounds.ButtonClick);
    }

    public void SaveAndExit()
    {
        pausePanel.SetActive(false);
        Bridge.GetInstance().SendScore(CharacterController.Instance.GetScore());
        SoundManager.Instance.PlaySound(SoundManager.Sounds.ButtonClick);
    }

    public void Mute()
    {
        SoundManager.Instance.Mute();
        muteImage.SetActive(true);
        unMuteImage.SetActive(false);
        PlayerPrefs.SetInt("SoundMuted", 0);
        SoundManager.Instance.PlaySound(SoundManager.Sounds.ButtonClick);
    }

    public void UnMute()
    {
        SoundManager.Instance.Unmute();
        unMuteImage.SetActive(true);
        muteImage.SetActive(false);
        PlayerPrefs.SetInt("SoundMuted", 1);
        SoundManager.Instance.PlaySound(SoundManager.Sounds.ButtonClick);
    }

    public void VibrateOn()
    {
        vibrateImage.SetActive(true);
        vibrateOffImage.SetActive(false);
        Bridge.GetInstance().VibrateBridge(true);
        isVibrating = true;
        SoundManager.Instance.PlaySound(SoundManager.Sounds.ButtonClick);
    }

    public void VibrateOff()
    {
        vibrateOffImage.SetActive(true);
        vibrateImage.SetActive(false);
        Bridge.GetInstance().VibrateBridge(false);
        isVibrating = false;
        SoundManager.Instance.PlaySound(SoundManager.Sounds.ButtonClick);
    }

    private void Score()
    {
        scoreTxt.text = CharacterController.Instance.GetScore().ToString();
    }

    private void Coin()
    {
        coinTxt.text = Bridge.GetInstance().thisPlayerInfo.coins.ToString();
    }

    private void HighScore()
    {
        highScoreTxt.text = Bridge.GetInstance().thisPlayerInfo.highScore.ToString();
    }
}
