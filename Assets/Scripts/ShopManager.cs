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
    private Dictionary<WarriorType, Warrior> _characterFeatures;
    private GameManager _gameManager;
    private ShopHelper _shopHelper;

    private void Start()
    {
        playerGoldUI.text = playerGold.ToString();
        shopUI.SetActive(showShop);
        _shopHelper = GetComponent<ShopHelper>();
        _characterFeatures = _shopHelper.CharTypeToFeatureDict;
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _gameManager.onGameOver.AddListener(OnGameOver);
    }

    private void OnGameOver(Castle castle)
    {
        shopUI.SetActive(false);
        EarnGold(castle.destroyReward);
    }

    public void ToggleShopUI()
    {
        showShop = !showShop;
        shopUI.SetActive(showShop);
    }

    public void EarnGold(int earning)
    {
        playerGold += earning;
        playerGoldUI.text = playerGold.ToString();
    }

    private void PayGold(int payment)
    {
        playerGold -= payment;
        playerGoldUI.text = playerGold.ToString();
    }

    public bool CanInstantiate(WarriorType warriorType)
    {
        return playerGold >= _characterFeatures[warriorType].spawnPrice;
    }

    public void PayForInstantiate(WarriorType warriorType)
    {
        PayGold(_characterFeatures[warriorType].spawnPrice);
    }

    public void PurchaseCharacter(WarriorType warriorType)
    {
        var price = _characterFeatures[warriorType].purchasePrice;
        PayGold(price);
    }

    public bool CanPurchase(WarriorType warriorType)
    {
        return playerGold >= _characterFeatures[warriorType].purchasePrice;
    }

    public bool CanUpgrade(WarriorType type)
    {
        var isMaxed = _characterFeatures[type].spawnRate - 0.05f < 0;
        return playerGold >= _characterFeatures[type].upgradePrice && !isMaxed;
    }

    public void UpgradeCharacter(WarriorType type)
    {
        var price = _characterFeatures[type].upgradePrice;
        PayGold(price);
        _characterFeatures[type].spawnRate -= 0.05f;
        _characterFeatures[type].upgradePrice += 2;
    }
}