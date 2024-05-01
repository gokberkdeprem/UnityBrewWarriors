using System.Collections.Generic;
using System.Linq;
using Enums;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] public int playerGold;
    [SerializeField] private TMP_Text playerGoldUI;
    [SerializeField] private bool showShop;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private List<GameObject> warriorsGameObjects;
    private readonly List<CharPriceModel> _charPriceModels = new();

    // Start is called before the first frame update
    private void Start()
    {
        playerGoldUI.text = "GOLD: " + playerGold;
        shopUI.SetActive(showShop);
        PopulateCharShopModel(warriorsGameObjects);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void PopulateCharShopModel(List<GameObject> warriors)
    {
        foreach (var warrior in warriors)
        {
            var charFeature = warrior.GetComponent<CharacterFeature>();
            _charPriceModels.Add(new CharPriceModel
            {
                CharacterType = charFeature.characterType,
                SpawnPrice = charFeature.spawnPrice,
                PurchasePrice = charFeature.purchasePrice
            });
        }
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

    public void PayGold(int payment)
    {
        playerGold -= payment;
        playerGoldUI.text = "GOLD: " + playerGold;
    }

    public bool CanInstantiate(CharacterType characterType)
    {
        return playerGold >= _charPriceModels.First(x => x.CharacterType == characterType).SpawnPrice;
    }

    public void PurchaseCharacter(CharacterType characterType)
    {
        var price = _charPriceModels.First(x => x.CharacterType == characterType).PurchasePrice;
        PayGold(price);
    }

    public bool CanPurchase(CharacterType characterType)
    {
        return playerGold >= _charPriceModels.First(x => x.CharacterType == characterType).PurchasePrice;
    }

    private class CharPriceModel
    {
        public CharacterType CharacterType;
        public int PurchasePrice;
        public int SpawnPrice;
    }
}