using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameObject gameController;

    [Header("Win Lose Panel")]
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;

    [Header("List change buttons")]
    [SerializeField] List<Button> LoadCurrentButtons;
    [SerializeField] List<Button> LoadNextButtons;
    [SerializeField] List<Button> LoadSkipButtons;

    [Header("Level text")]
    [SerializeField] Text LvText;

    [Header("Settings")]
    [SerializeField] GameObject settingPanel;
    [SerializeField] CanvasGroup settingsCanvas;
    [SerializeField] RectTransform settingsTransform;
    [SerializeField] Button SettingButton;
    [SerializeField] Button CloseButton;

    [Header("List Button Control Background music")]
    [SerializeField] Button StartgameBtn;
    [SerializeField] Button ReturnHome;

    [Header("Game Objects check")]
    [SerializeField] EventsManager eventsManager;
    [SerializeField] GameObject gameplay;

    public bool isSkipLevel = false;

    [Header("Gameplay Buttons")]
    [SerializeField] Button SkipButton;
    [SerializeField] Button HomeButton;
    [SerializeField] Button HintButton;
    public bool isDisableSkipBUtton = false;
    
    [Header("Public bool check")]
    public bool isReturnHome = false;
    
    private void Start()
    {
        foreach (var button in LoadCurrentButtons)
        {
            button.onClick.AddListener(() =>
            {
                eventsManager.enabled = false;
                SoundManager.Instance.PlaySound("14. Click Bubble New");
                ClickChangeLevel(PlayerDataManager.Instance.GetCurrentLevel());
                isDisableSkipBUtton = false;
            });
        }
        foreach (var button in LoadNextButtons)
        {
            button.onClick.AddListener(() =>
            {
                eventsManager.enabled = false;
                SoundManager.Instance.PlaySound("14. Click Bubble New");
                if (FirebaseManager.Instance.listLevelShowInterAds.Contains(PlayerDataManager.Instance.GetCurrentLevel() + 1))
                    AdsManager.Instance.ShowInterstitial();
                ClickChangeLevel(PlayerDataManager.Instance.GetCurrentLevel() + 1);
                FirebaseManager.Instance.Pass_NormalLevel(PlayerDataManager.Instance.GetCurrentLevel() + 1);
                isDisableSkipBUtton = false;
            });
        }
        foreach (var button in LoadSkipButtons)
        {
            button.onClick.AddListener(() =>
            {
                eventsManager.enabled = false;
                SoundManager.Instance.PlaySound("14. Click Bubble New");
                AdsManager.Instance.ShowRewardedAd();
                isSkipLevel = true;
                FirebaseManager.Instance.Skip_NormalLevel(PlayerDataManager.Instance.GetCurrentLevel() + 1);
            });
        }
        SettingButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound("14. Click Bubble New");
            UIManager.Instance.PanelFadeIn(settingsCanvas, settingsTransform);
            settingPanel.SetActive(true);
        });
        CloseButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound("14. Click Bubble New");
            UIManager.Instance.PanelFadeOut(settingsCanvas, settingsTransform, settingPanel);
        });
        StartgameBtn.onClick.AddListener(() =>
        {
            if(FirebaseManager.Instance.isShowInterAdsFromHome)
                AdsManager.Instance.ShowInterstitialFromHome();
            isReturnHome = false;
            gameplay.SetActive(true);
            SoundManager.Instance.SoundAction("1. Background 1");
            SoundManager.Instance.StopAllSoundExcept();
            SoundManager.Instance.PlaySound("1. Background 2", true);

        });
        ReturnHome.onClick.AddListener(() =>
        {
            if (FirebaseManager.Instance.isShowInterAdsFromGameplay)
                AdsManager.Instance.ShowInterstitialFromGameplay();
            isReturnHome = true;
            gameplay.SetActive(false);
            gameController.SetActive(false);
            SoundManager.Instance.PlaySound("14. Click Bubble New");
            SoundManager.Instance.StopAllSoundExcept();
            SoundManager.Instance.PlaySound("1. Background 1", true);
        });
    }
    private void Update()
    {
        if (!isDisableSkipBUtton)
        {
            if (GameController.Instance.isStartGame)
            {
                SkipButton.interactable = false;
                HomeButton.interactable = false;
                HintButton.interactable = false;
                isDisableSkipBUtton = true;
            }
            else
            {
                SkipButton.interactable = true;
                HomeButton.interactable = true;
                HintButton.interactable = true;
            }
        }
    }
    public void ClickChangeLevel(int level)
    {
        eventsManager.enabled = true;
        SoundManager.Instance.StopAllSoundExcept("1. Background 2");
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        gameController.SetActive(false);
        LevelManager.Instance.LoadLevel(level);
        PlayerDataManager.Instance.SetCurrentLevel(level);
        UpdateLevelText();
    }
    void UpdateLevelText()
    {
        LvText.text = "Level " + ((PlayerDataManager.Instance.GetCurrentLevel() < 9) ? "0" : null) + (PlayerDataManager.Instance.GetCurrentLevel() + 1);
    }
}
