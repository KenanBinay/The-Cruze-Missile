using UnityEngine;
using UnityEngine.Advertisements;

public class adController_unityAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId_rewarded = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId_rewarded = "Rewarded_iOS";
    string _adUnitId_rewarded = null; // This will remain null for unsupported platforms

    [SerializeField] string _androidAdUnitId_interstitial = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId_interstitial = "Interstitial_iOS";
    string _adUnitId_interstitial;

    [SerializeField] string _androidAdUnitId_banner = "Banner_Android";
    [SerializeField] string _iOSAdUnitId_banner = "Banner_iOS";
    string _adUnitId_banner = null;

    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    public static bool rewardedGiven, interstitialGiven, x2Reward, fuelOffer;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
            _adUnitId_rewarded = _iOSAdUnitId_rewarded;
            _adUnitId_interstitial = _iOsAdUnitId_interstitial;
            _adUnitId_banner = _iOSAdUnitId_banner;
#elif UNITY_ANDROID
            _adUnitId_rewarded = _androidAdUnitId_rewarded;
            _adUnitId_interstitial = _androidAdUnitId_interstitial;
            _adUnitId_banner = _androidAdUnitId_banner;

#elif UNITY_EDITOR  //Only for testing the functionality in the Editor
        _adUnitId_rewarded = _androidAdUnitId_rewarded;
        _adUnitId_interstitial = _androidAdUnitId_interstitial;
        _adUnitId_banner = _androidAdUnitId_banner;
#endif
    }

    void Start()
    {
        LoadRewardedAd();
        LoadInterstitialAd();
        LoadBanner();

        if (gameController.bannerAd_randomNumb == 1 && PlayerPrefs.GetInt("adsRemoved", 0) == 0)
            ShowBannerAd();

        if (PlayerPrefs.GetInt("adsRemoved", 0) == 1)
            Debug.Log("ADS BLOCKED NO ADS");
    }

    private void Update()
    {
        if (missileController.targetHit && PlayerPrefs.GetInt("adsRemoved", 0) == 0)
        {
            if (gameController.interstitialAd_randomNumb == 1 && !interstitialGiven)
                ShowInterstitialAd();
        }
        if (missileController.crashed && PlayerPrefs.GetInt("adsRemoved", 0) == 0)
        {
            if (gameController.interstitialAd_randomNumb == 2 && !interstitialGiven)
                ShowInterstitialAd();
        }
    }

    public void LoadBanner()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_adUnitId_banner, options);
        Advertisement.Banner.SetPosition(_bannerPosition);
        ShowBannerAd();
    }

    // Call this public method when you want to get an ad ready to show.
    public void LoadRewardedAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId_rewarded);
        Advertisement.Load(_adUnitId_rewarded, this);
    }

    public void LoadInterstitialAd()
    {
        Debug.Log("Loading Ad: " + _adUnitId_interstitial);
        Advertisement.Load(_adUnitId_interstitial, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId_rewarded))
        {

        }
    }

    // Implement a method to execute when the user clicks the button:
    public void refuelRewardedAd()
    {
        fuelOffer = true;

        Advertisement.Show(_adUnitId_rewarded, this);
    }

    public void x2RewardedAd()
    {
        x2Reward = true;

        Advertisement.Show(_adUnitId_rewarded, this);
    }

    // Implement a method to call when the Show Banner button is clicked:
    void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {

        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_adUnitId_banner, options);
    }
    void ShowInterstitialAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + _adUnitId_interstitial);
        interstitialGiven = true;
        Advertisement.Show(_adUnitId_interstitial, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId_rewarded) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            if (x2Reward)
            {
                scoreManager_inGame.addScore(gameController.gainedScoreInLevel);
                Debug.Log("2X SCORE: " + gameController.gainedScoreInLevel);
                rewardedGiven = true;
                x2Reward = false;
            }

            if (fuelOffer)
            {
                fuelManager.refuel = true;
                Debug.Log("fuelGiven");
            }
            // Grant a reward.
        }
    }
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
    }

    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}
