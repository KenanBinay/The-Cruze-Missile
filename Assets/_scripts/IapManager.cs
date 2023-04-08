using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing.Security;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
    [SerializeField]
    private Button PurchaseButton;
    [SerializeField]
    private GameObject Purchase_icon;

    public string noAds_id;
    public static bool noAds_bought;

    IPurchaseReceipt productReceipt;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("adsRemoved", 0) == 1)
        {
            noAds_bought = true;

            PurchaseButton.enabled = false;
            Purchase_icon.SetActive(false);

            Debug.Log(noAds_id + " purchased");
        }
    }

    public void OnPurchaseComplete(UnityEngine.Purchasing.Product product)
    {
        if (product.definition.id == noAds_id)
        {
            PlayerPrefs.SetInt("adsRemoved", 1);

            PurchaseButton.enabled = false;
            Purchase_icon.SetActive(false);

            Debug.Log("noAds purchase complete");
        }
    }

    public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason reason)
    {
        Debug.Log(product + " || " + reason);
    }
}
