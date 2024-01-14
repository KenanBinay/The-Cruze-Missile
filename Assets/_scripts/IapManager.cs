using UnityEngine;
using UnityEngine.Purchasing.Security;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IapManager : MonoBehaviour//, IStoreListener
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

    private IStoreController m_StoreController;

    private void Awake()
    {
        //  InitializePurchasing();
        RestoreVariable();
    }

    private void Start()
    {

    }

  /*  void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(noAds_id, ProductType.NonConsumable);
        builder.AddProduct(offers_id[0], ProductType.Consumable);
        builder.AddProduct(offers_id[1], ProductType.Consumable);
        builder.AddProduct(offers_id[2], ProductType.Consumable);
        builder.AddProduct(offers_id[3], ProductType.Consumable);
        builder.AddProduct(offers_id[4], ProductType.Consumable);
        builder.AddProduct(offers_id[5], ProductType.Consumable);
        builder.AddProduct(offers_id[6], ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    } */

    void RestoreVariable()
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
        if (product.definition.id == offers_id[7]) token += 15000;
        if (product.definition.id == offers_id[8]) token += 50000;

        PlayerPrefs.SetInt("tokens", token);

        Debug.Log(product.definition.id + " token purchased");
    }

    public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason reason)
    {
        Debug.Log(product + " || " + reason);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;

        CheckNonConsumable(noAds_id);
    }

    void CheckNonConsumable(string id)
    {
        if (m_StoreController != null)
        {
            var product = m_StoreController.products.WithID(id);
            if (product != null)
            {
                if (product.hasReceipt)
                {
                    PlayerPrefs.SetInt("adsRemoved", 1);

                    removeAds_button.enabled = false;
                    noAds_icon.SetActive(false);
                }
                else
                {
                    Debug.Log("yok");
                }
            }
        }
    }

    public void OnTransactionsRestored(bool success, string? error)
    {
        Debug.Log($"TransactionsRestored: {success} {error}");
    }
}
