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
    public List<GameObject> warriorsGameObjects;
    public Dictionary<CharacterType, CharacterFeature> CharTypeToFeature = new();

    private void Start()
    {
        playerGoldUI.text = "GOLD: " + playerGold;
        shopUI.SetActive(showShop);
        PopulateCharShopModel();
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
        return playerGold >= CharTypeToFeature[characterType].spawnPrice;
    }

    public void PayForInstantiate(CharacterType characterType)
    {
        PayGold(CharTypeToFeature[characterType].spawnPrice);
    }

    public void PurchaseCharacter(CharacterType characterType)
    {
        var price = CharTypeToFeature[characterType].purchasePrice;
        PayGold(price);
    }

    public bool CanPurchase(CharacterType characterType)
    {
        return playerGold >= CharTypeToFeature[characterType].purchasePrice;
    }

    private void PopulateCharShopModel()
    {
        foreach (var warrior in warriorsGameObjects)
        {
            var charFeature = warrior.GetComponent<CharacterFeature>();
            CharTypeToFeature.Add(charFeature.characterType, charFeature);
        }
    }
}