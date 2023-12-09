using System;
using UnityEngine;
using GoogleMobileAds;
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

        MobileAds.Initialize((InitializationStatus initStatus) => { requestAd(); });

        timeRemaining = PlayerPrefs.GetFloat("countdownVal", timeRemaining);

        if (PlayerPrefs.GetInt("tokenCoolDown") == 0) freeTokenGiven = false;
        if (PlayerPrefs.GetFloat("countdownVal", timeRemaining) > 0)
        {
            freeTokenGiven = true;

            countdown_text.SetActive(true);
            claimed.SetActive(true);
            claim.SetActive(false);
        }
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

    public void ShowRewardedAd()
    {
        if (adRewarded != null && adRewarded.CanShowAd() && !freeTokenGiven)
        {
            adRewarded.Show((Reward reward) =>
            {
                // TODO: Reward the user.

            });
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            int token = PlayerPrefs.GetInt("tokens", 0);
            token += 1;
            PlayerPrefs.SetInt("tokens", token);

            freeTokenGiven = true;
            timeRemaining = 90;

            countdown_text.SetActive(true);
            claimed.SetActive(true);
            claim.SetActive(false);
        };
    }
}
