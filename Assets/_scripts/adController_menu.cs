using System;
using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;

public class adController_menu : MonoBehaviour
{
    private RewardedAd adRewarded;
    string idRewarded;

    public GameObject claimed, claim, countdown_text;
    float timeRemaining;

    public static bool freeTokenGiven;

    void Start()
    {    
        idRewarded = "ca-app-pub-9421503984483424/3742292473";

        timeRemaining = PlayerPrefs.GetFloat("countdownVal", timeRemaining);

        if (PlayerPrefs.GetInt("tokenCoolDown") == 0) freeTokenGiven = false; 
        else freeTokenGiven = true;

        requestAd(); 
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
             //   Debug.Log(min.ToString("00") + ":" + sec.ToString("00"));
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                freeTokenGiven = false;
                PlayerPrefs.SetInt("tokenCoolDown", 0);
                requestAd();
            }

            PlayerPrefs.SetFloat("countdownVal", timeRemaining);
        }
    }

    private void requestAd()
    {
        this.adRewarded = new RewardedAd(idRewarded);

        AdRequest request = new AdRequest.Builder().Build();

        this.adRewarded.LoadAd(request);

        MobileAds.Initialize(initStatus => { });

        if (this.adRewarded.IsLoaded() && !freeTokenGiven)
        {
            if (claimed != null && claim != null)
            {
                claimed.SetActive(false);
                claim.SetActive(true);
                countdown_text.SetActive(false);
            }         
        }
        if (freeTokenGiven)
        {
            if (claimed != null && claim != null)
            {
                claimed.SetActive(true);
                claim.SetActive(false);
                countdown_text.SetActive(true);
            }
        }
    }

    public void freeTokenClaim()
    {
        if (!this.adRewarded.IsLoaded() || freeTokenGiven)
        {
            Debug.Log("no rewarded ads");
        }
        if (this.adRewarded.IsLoaded() && !freeTokenGiven)
        {
            adRewarded.OnAdLoaded += this.HandleOnRewardedAdLoaded;
            adRewarded.OnAdOpening += this.HandleOnRewardedAdOpening;
            adRewarded.OnAdClosed += this.HandleOnRewardedAdClosed;

            Debug.Log("rewarded ad taken");
            this.adRewarded.Show();
        }
    }

    public void HandleOnRewardedAdLoaded(object sender, EventArgs args) { }
    public void HandleOnRewardedAdOpening(object sender, EventArgs args) { }
    public void HandleOnRewardedAdClosed(object sender, EventArgs args)
    {
        int token = PlayerPrefs.GetInt("tokens", 0);
        token += 1;
        PlayerPrefs.SetInt("tokens", token);

        freeTokenGiven = true;
        timeRemaining = 90;

        countdown_text.SetActive(true);
        claimed.SetActive(true);
        claim.SetActive(false);

        Debug.Log("token claimed");

        adRewarded.OnAdLoaded -= this.HandleOnRewardedAdLoaded;
        adRewarded.OnAdOpening -= this.HandleOnRewardedAdOpening;
        adRewarded.OnAdClosed -= this.HandleOnRewardedAdClosed;
    }
}
