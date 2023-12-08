using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class adController_admob : MonoBehaviour
{
    private InterstitialAd adInterstitial;
    private RewardedAd adRewarded;
    private BannerView bannerView;
    string idInterstitial, idRewarded, idBanner;
    string appId = "ca-app-pub-9421503984483424~7002227357";

    public static bool rewardedGiven, interstitialGiven, x2Reward, fuelOffer;

    void Start()
    {
        idInterstitial = "ca-app-pub-9421503984483424/9436819008";
        idRewarded = "ca-app-pub-9421503984483424/3742292473";
        idBanner = "ca-app-pub-9421503984483424/6639084408";

        MobileAds.Initialize(initStatus => 
        { 
            loadRewardedAd(); loadInterstitialAd();
        });

        this.bannerView = new BannerView(idBanner, AdSize.Banner, AdPosition.Top);

        var request = new AdRequest();

        if (gameController.bannerAd_randomNumb == 1 && PlayerPrefs.GetInt("adsRemoved", 0) == 0)
            this.bannerView.LoadAd(request);

        if (PlayerPrefs.GetInt("adsRemoved", 0) == 1)
            Debug.Log("ADS BLOCKED NO ADS");

    }

    private void Update()
    {
        if (missileController.targetHit && PlayerPrefs.GetInt("adsRemoved", 0) == 0)
        {
            if (gameController.interstitialAd_randomNumb == 1 && !interstitialGiven) showInterstitial();
        }
        if (missileController.crashed && PlayerPrefs.GetInt("adsRemoved", 0) == 0)
        {
            if (gameController.interstitialAd_randomNumb == 2 && !interstitialGiven) showInterstitial();
        }
    }

    private void loadRewardedAd()
    {
        if (adRewarded != null)
        {
            adRewarded.Destroy();
            adRewarded = null;
        }

        Debug.Log("Loading the rewarded ad.");

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(idRewarded, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Rewarded interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

              adRewarded = ad;
              RegisterEventHandlers(adRewarded);
          });
    }

    private void loadInterstitialAd()
    {
        if (adInterstitial != null)
        {
            adInterstitial.Destroy();
            adInterstitial = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        InterstitialAd.Load(idInterstitial, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                adInterstitial = ad;
            });
    }

    public void showInterstitial()
    {
        if (adInterstitial != null && adInterstitial.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialGiven = true;

            adInterstitial.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    public void x2RewardedAd()
    {   
        if (this.adRewarded.CanShowAd())
        {
            x2Reward = true;

            adRewarded.Show((Reward reward) =>
            {

            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
        }
    }

    public void refuelRewardedAd()
    {  
        if (this.adRewarded.CanShowAd())
        {
            fuelOffer = true;

            adRewarded.Show((Reward reward) =>
            {

            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
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
        };
    }
}
