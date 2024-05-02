using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int playerGold;
    public TMP_Text playerGoldUI;
    public bool showShop;
    public GameObject shopUI;
    private Dictionary<CharacterType, CharacterFeature> _characterFeatures;
    private ShopHelper _shopHelper;


    private void Start()
    {
        playerGoldUI.text = "GOLD: " + playerGold;
        shopUI.SetActive(showShop);
        _shopHelper = GetComponent<ShopHelper>();
        _characterFeatures = _shopHelper.CharTypeToFeatureDict;
    }

    public void ToggleShopUI()
    {
        showShop = !showShop;
        shopUI.SetActive(showShop);
    }

    public void EarnGold(int earning)
    {
        playerGold += earning;
        playerGoldUI.text = "GOLD: " + playerGold;
    }

    private void PayGold(int payment)
    {
        playerGold -= payment;
        playerGoldUI.text = "GOLD: " + playerGold;
    }

    public bool CanInstantiate(CharacterType characterType)
    {
        return playerGold >= _characterFeatures[characterType].spawnPrice;
    }

    public void PayForInstantiate(CharacterType characterType)
    {
        PayGold(_characterFeatures[characterType].spawnPrice);
    }

    public void PurchaseCharacter(CharacterType characterType)
    {
        var price = _characterFeatures[characterType].purchasePrice;
        PayGold(price);
    }

    public bool CanPurchase(CharacterType characterType)
    {
        return playerGold >= _characterFeatures[characterType].purchasePrice;
    }


    public bool CanUpgrade(CharacterType type)
    {
        return playerGold >= _characterFeatures[type].upgradePrice;
    }

    public void UpgradeCharacter(CharacterType type)
    {
        var price = _characterFeatures[type].upgradePrice;
        PayGold(price);
        _characterFeatures[type].spawnRate -= 0.05f;
        _characterFeatures[type].upgradePrice += 2;
    }
}