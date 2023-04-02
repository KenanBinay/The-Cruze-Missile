using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEditor.PackageManager.Requests;

public class adController : MonoBehaviour
{
    private InterstitialAd adInterstitial;
    private RewardedAd adRewarded;
    private BannerView bannerView;
    string idInterstitial, idRewarded, idBanner;

    public static bool rewardedGiven;

    void Start()
    {
        idInterstitial = "ca-app-pub-9421503984483424~7002227357";
        idRewarded = "ca-app-pub-9421503984483424~7002227357";
        idBanner = "ca-app-pub-9421503984483424/6639084408";

        this.adRewarded = new RewardedAd(idRewarded);
        this.adInterstitial = new InterstitialAd(idInterstitial);
        this.bannerView = new BannerView(idBanner, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        this.adRewarded.LoadAd(request);
        this.adInterstitial.LoadAd(request);

        if (gameController.bannerAd_randomNumb == 1) 
        this.bannerView.LoadAd(request);

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
            adRewarded.OnAdLoaded += this.HandleOnRewardedAdLoaded;
            adRewarded.OnAdOpening += this.HandleOnRewardedAdOpening;
            adRewarded.OnAdClosed += this.HandleOnRewardedAdClosed;

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
        scoreManager_inGame.addScore(gameController.gainedScoreInLevel);
        Debug.Log("2X SCORE: " + gameController.gainedScoreInLevel);
        rewardedGiven = true;

        adRewarded.OnAdLoaded -= this.HandleOnRewardedAdLoaded;
        adRewarded.OnAdOpening -= this.HandleOnRewardedAdOpening;
        adRewarded.OnAdClosed -= this.HandleOnRewardedAdClosed;
    }
}
