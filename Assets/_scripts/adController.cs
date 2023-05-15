using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using Unity.VisualScripting;

public class adController : MonoBehaviour
{
    private InterstitialAd adInterstitial;
    private RewardedAd adRewarded;
    private BannerView bannerView;
    string idInterstitial, idRewarded, idBanner;
    string appId = "ca-app-pub-9421503984483424~7002227357";

    public static bool rewardedGiven, interstitialGiven, x2Reward, fuelOffer;

    void Start()
    {
        MobileAds.Initialize(initStatus => { });

        //  idInterstitial = "ca-app-pub-9421503984483424/9436819008";
        idInterstitial = "ca-app-pub-3940256099942544/1033173712";
        //    idRewarded = "ca-app-pub-9421503984483424/3742292473";
        idRewarded = "ca-app-pub-3940256099942544/5224354917";
        // idBanner = "ca-app-pub-9421503984483424/6639084408";
        idBanner = "ca-app-pub-3940256099942544/6300978111";

        this.adRewarded = new RewardedAd(idRewarded);
        this.adInterstitial = new InterstitialAd(idInterstitial);
        this.bannerView = new BannerView(idBanner, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        this.adRewarded.LoadAd(request);
        this.adInterstitial.LoadAd(request);

        if (gameController.bannerAd_randomNumb == 1 && PlayerPrefs.GetInt("adsRemoved", 0) == 0)
            this.bannerView.LoadAd(request);

        if (PlayerPrefs.GetInt("adsRemoved", 0) == 1)
            Debug.Log("ADS BLOCKED NO ADS");
    }

    private void Update()
    {
        if (missileController.targetHit && PlayerPrefs.GetInt("adsRemoved", 0) == 0)
        {
            if (gameController.interstitialAd_randomNumb == 2 && !interstitialGiven) interstitialAd();
        }
        if(missileController.crashed && PlayerPrefs.GetInt("adsRemoved", 0) == 0)
        {
            if (gameController.interstitialAd_randomNumb == 2 && !interstitialGiven) interstitialAd();
        }
    }

    public void interstitialAd()
    {
        if (this.adInterstitial.IsLoaded())
        {
            Debug.Log("interstitial ads");
            interstitialGiven = true;
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
            x2Reward = true;

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

    public void refuelRewardedAd()
    {
        if (this.adRewarded.IsLoaded())
        {
            fuelOffer = true;

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

        adRewarded.OnAdLoaded -= this.HandleOnRewardedAdLoaded;
        adRewarded.OnAdOpening -= this.HandleOnRewardedAdOpening;
        adRewarded.OnAdClosed -= this.HandleOnRewardedAdClosed;
    }
}
