using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class adController : MonoBehaviour
{
    private InterstitialAd adInterstitial;
    private RewardedAd adRewarded;
    string idInterstitial, idRewarded;

    public static bool rewardedGiven;

    void Start()
    {
        idInterstitial = "ca-app-pub-9421503984483424~7002227357";
        idRewarded = "ca-app-pub-9421503984483424~7002227357";

        this.adRewarded = new RewardedAd(idRewarded);
        this.adInterstitial = new InterstitialAd(idInterstitial);

        AdRequest request = new AdRequest.Builder().Build();

        this.adRewarded.LoadAd(request);
        this.adInterstitial.LoadAd(request);

        MobileAds.Initialize(initStatus => { });
    }

    public void interstitialAd()
    {
        if (this.adInterstitial.IsLoaded())
        {     
            this.adInterstitial.Show();
        }
        else
        {
            Debug.Log("no interstitial ads");
        }
    }

    public void x2RewardedAd()
    {
        if (this.adRewarded.IsLoaded())
        {
            scoreManager_inGame.addScore(gameController.gainedScoreInLevel);
            Debug.Log("2X SCORE: " + gameController.gainedScoreInLevel);
            rewardedGiven = true;

            this.adRewarded.Show();
        }
        else
        {
            Debug.Log("no rewarded ads");
        }
    }

    public void HandleOnRewardedAdLoaded(object sender, EventArgs args) { }
    public void HandleOnRewardedAdOpening(object sender, EventArgs args) { }
    public void HandleOnRewardedAdClosed(object sender, EventArgs args)
    {  
        adInterstitial.OnAdLoaded -= this.HandleOnRewardedAdLoaded;
        adInterstitial.OnAdOpening -= this.HandleOnRewardedAdOpening;
        adInterstitial.OnAdClosed -= this.HandleOnRewardedAdClosed;
    }
}
