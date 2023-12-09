using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;

public class adController_menu_unityAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms

    public GameObject claimed, claim, countdown_text;
    float timeRemaining;

    public static bool freeTokenGiven;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
            _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#elif UNITY_EDITOR
        _adUnitId = _androidAdUnitId; //Only for testing the functionality in the Editor
#endif
    }

    void Start()
    {
        timeRemaining = PlayerPrefs.GetFloat("countdownVal", timeRemaining);
        if (PlayerPrefs.GetInt("tokenCoolDown") == 0) freeTokenGiven = false;
        if (timeRemaining > 0)
        {
            freeTokenGiven = true;
            Debug.Log("freeTokenGiven: " + freeTokenGiven);

            countdown_text.SetActive(true);
            claimed.SetActive(true);
            claim.SetActive(false);
        }

        LoadAd();
    }

    private void Update()
    {
        if (freeTokenGiven)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                int min = Mathf.FloorToInt(timeRemaining / 60);
                int sec = Mathf.FloorToInt(timeRemaining % 60);

                if (countdown_text != null)
                {
                    TextMeshProUGUI timer_text = countdown_text.GetComponent<TextMeshProUGUI>();
                    timer_text.text = min.ToString("00") + ":" + sec.ToString("00");
                }
                 //  Debug.Log(min.ToString("00") + ":" + sec.ToString("00"));
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                freeTokenGiven = false;
                PlayerPrefs.SetInt("tokenCoolDown", 0);
                LoadAd();
            }

            PlayerPrefs.SetFloat("countdownVal", timeRemaining);
        }
    }

    // Call this public method when you want to get an ad ready to show.
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);

        if (!freeTokenGiven)
        {
            if (claimed != null && claim != null)
            {
                claimed.SetActive(false);
                claim.SetActive(true);
                countdown_text.SetActive(false);
            }
        }
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {

        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        if (!freeTokenGiven)
        {
            Advertisement.Show(_adUnitId, this);
        }
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");

            int token = PlayerPrefs.GetInt("tokens", 0);
            token += 1;
            PlayerPrefs.SetInt("tokens", token);

            freeTokenGiven = true;
            timeRemaining = 90;

            countdown_text.SetActive(true);
            claimed.SetActive(true);
            claim.SetActive(false);
            // Grant a reward.
        }
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
