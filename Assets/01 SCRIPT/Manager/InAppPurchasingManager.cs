using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public enum ItemsType
{
    RemoveAds
}

[System.Serializable]
public class Products
{
    public string id;
    public string price;
    public ItemsType type;
}


public class InAppPurchasingManager : MonoBehaviour, IStoreListener
{
    private IStoreController controller;
    private IExtensionProvider extensions;
    private bool isInit;
    public Products[] products;

    [SerializeField] Button removeAdsButton;
    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        removeAdsButton.onClick.RemoveAllListeners();
        removeAdsButton.onClick.AddListener(() =>
        {
            Buy(0);
        });
    }
    private void Init()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        for (int i = 0; i < products.Length; i++)
        {
            builder.AddProduct(products[i].id,
                (UnityEngine.Purchasing.ProductType)ProductType.Consumable, new IDs()
                {
                    {products[i].id, GooglePlay.Name},
                    {products[i].id, AppleAppStore.Name}
                });
        }
        UnityPurchasing.Initialize(this, builder);
    }
    private void OnReward(string id)
    {
        for (int i = 0; i < products.Length; i++)
        {
            if (!products[i].id.Equals(id))
                continue;
            switch (products[i].type)
            {
                case ItemsType.RemoveAds:
                    {
                        PlayerDataManager.Instance.SetRemoveAdsStatus(true);
                        break;
                    }
            }
        }
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
#if UNITY_EDITOR
        Debug.Log("OnInitialized: pass");
#endif
        this.controller = controller;
        this.extensions = extensions;
        isInit = true;
    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {
#if UNITY_EDITOR
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
#endif
        isInit = false;
    }
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
#if UNITY_EDITOR
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", i.definition.storeSpecificId, p));
#endif
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
#if UNITY_EDITOR
        Debug.Log("OnPurchaseReward: Success. Product:" + e.purchasedProduct);
#endif

        OnReward(e.purchasedProduct.definition.id);
        return PurchaseProcessingResult.Complete;
    }
    public bool IsInit()
    {
        return isInit;
    }
    public int GetLenght()
    {
        int count = 0;
        if (products.Length > 0)
            count = products.Length;
        return count;
    }
    public string GetLocalizedPrice(int index)
    {
        string priceString = "In Coming";
        if (products.Length > 0 &&
            products[index].id != null)
        {
            Product product = controller.products.WithID(products[index].id);
            if (product != null)
                priceString = product.metadata.localizedPriceString;
        }
        return priceString;
    }
    public void Buy(int index)
    {
        if (products.Length <= 0 || products[index].id == null)
            return;
        try
        {
            controller.InitiatePurchase(products[index].id);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
