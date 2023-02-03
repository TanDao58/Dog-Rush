using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AdsManager : Singleton<AdsManager>
{
    [SerializeField] ButtonManager buttonManager;
    string adUnitKey = "enmXpFsmcX7SfpGFuvnLm5aTuScxPJoH35SlgABnd6u0BfCk-RZqpcJiLHEP86Sev9NTM6u9P5aHtI09K7t7Cw";
    string rewardAdUnitId = "6d3ead074c6502a8";


    string InterstitialAdUnitId = "c92a48e817030247";
    string bannerAdUnitId = "57116f1fce9e31f4";
    string AppOpenAdUnitId = "0b72c70f8731f1cd";

    string DogRushAdsFromHome = "9d6bbbce36cf03e2";
    string DogRushAdsFromGameplay = "1ca7fe139535f77d";


    int retryAttempt;
    public bool gameIsOn, hasInternet;
    bool ShowOpenAds = false;

    private void OnEnable()
    {
        gameIsOn = true;

#if UNITY_EDITOR
#else
            Debug.unityLogger.filterLogType = LogType.Warning;
            Debug.unityLogger.filterLogType = LogType.Error;
#endif
    }

    [Obsolete]
    private void Start()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            if (!PlayerDataManager.Instance.GetRemoveAdsStatus())
            {
                // AppLovin SDK is initialized, start loading ads
                InitializeBannerAds();
                InitializeInterstitialAds();
            }
            InitializeRewardedAds();
        };
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            if (!PlayerDataManager.Instance.GetRemoveAdsStatus())
            {
                MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
                ShowAdIfReady();
            }
        };
        MaxSdk.SetSdkKey(adUnitKey);
        //MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        StartCoroutine(CheckInternetConnection());
    }
    [Obsolete]
    IEnumerator CheckInternetConnection()
    {
        while (gameIsOn)
        {
            yield return new WaitForSeconds(1f);
            const string echoServer = "http://www.example.com";
            bool result;
            using (var request = UnityWebRequest.Head(echoServer))
            {
                request.timeout = 3;
                yield return request.SendWebRequest();
                result = !request.isNetworkError && !request.isHttpError && request.responseCode == 200;
            }
            if (result)
                hasInternet = true;
            else
                hasInternet = false;
        }
    }
    private void Update()
    {
        if (FirebaseManager.Instance.isShowBannerAds)
            ShowBanner();
    }
    #region Banner
    public void InitializeBannerAds()
    {
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerExtraParameter(bannerAdUnitId, "adaptive_banner", "false");
        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.white);
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
    }
    public void ShowBanner()
    {
        MaxSdk.ShowBanner(bannerAdUnitId);
    }
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    #endregion
    #region Interstitials
    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        // Load the first interstitial
        LoadInterstitial();
    }
    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
        MaxSdk.LoadInterstitial(DogRushAdsFromHome);
        MaxSdk.LoadInterstitial(DogRushAdsFromGameplay);
    }

    public void ShowInterstitial()
    {
        if (FirebaseManager.Instance.isShowInterAds)
        {
            if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
            {
                MaxSdk.ShowInterstitial(InterstitialAdUnitId);
            }
        }
    }
    public void ShowInterstitialFromHome()
    {
        if (MaxSdk.IsInterstitialReady(DogRushAdsFromHome))
        {
            MaxSdk.ShowInterstitial(DogRushAdsFromHome);
        }
    }

    public void ShowInterstitialFromGameplay()
    {
        if (MaxSdk.IsInterstitialReady(DogRushAdsFromGameplay))
        {
            MaxSdk.ShowInterstitial(DogRushAdsFromGameplay);
        }
    }
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
    }
    #endregion
    #region Reward
    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }
    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(rewardAdUnitId);
    }
    public void ShowRewardedAd()
    {
        if (FirebaseManager.Instance.isShowRewardAds)
        {
            if (MaxSdk.IsRewardedAdReady(rewardAdUnitId))
            {
                MaxSdk.ShowRewardedAd(rewardAdUnitId);
            }
        }
    }
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }
    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        if (buttonManager.isSkipLevel)
        {
            buttonManager.ClickChangeLevel(PlayerDataManager.Instance.GetCurrentLevel() + 1);
            buttonManager.isDisableSkipBUtton = false;
        }
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }
    #endregion
    #region Open Ads
    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            ShowAdIfReady();
        }
    }
    public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
    }
    public void ShowAdIfReady()
    {
        if (FirebaseManager.Instance.isShowOpenAds)
        {
            if (MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId))
            {
                MaxSdk.ShowAppOpenAd(AppOpenAdUnitId);
            }
            else
            {
                MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
            }
        }
    }
    #endregion
}
