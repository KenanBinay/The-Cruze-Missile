using UnityEngine;
using UnityEngine.Purchasing.Security;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IapManager : MonoBehaviour
{
    [SerializeField]
    Button removeAds_button;
    [SerializeField]
    GameObject noAds_icon;
    public string noAds_id;

    [SerializeField] 
    Button[] shopOffer_buttons;
    [SerializeField]
    string[] offers_id;

    public static bool noAds_bought;
    public static int token_value;

    IPurchaseReceipt productReceipt;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("adsRemoved", 0) == 1)
        {
            noAds_bought = true;

            removeAds_button.enabled = false;
            noAds_icon.SetActive(false);

            Debug.Log(noAds_id + " purchased");
        }
    }

    public void OnPurchaseComplete(UnityEngine.Purchasing.Product product)
    {
        int token = PlayerPrefs.GetInt("tokens", 0);

        if (product.definition.id == noAds_id)
        {
            PlayerPrefs.SetInt("adsRemoved", 1);

            removeAds_button.enabled = false;
            noAds_icon.SetActive(false);

            Debug.Log("noAds purchase complete");
        }
        if (product.definition.id == offers_id[0]) token += 20;
        if (product.definition.id == offers_id[1]) token += 50;
        if (product.definition.id == offers_id[2]) token += 80;
        if (product.definition.id == offers_id[3]) token += 120;
        if (product.definition.id == offers_id[4]) token += 200;
        if (product.definition.id == offers_id[5]) token += 350;
        if (product.definition.id == offers_id[6]) token += 500;

        PlayerPrefs.SetInt("tokens", token);

        Debug.Log(product.definition.id + " token purchased");
    }

    public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason reason)
    {
        Debug.Log(product + " || " + reason);
    }
}
