using System.Collections.Generic;
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
    private readonly Dictionary<CharacterType, int> _charTypePriceInfo = new();

    // Start is called before the first frame update
    private void Start()
    {
        playerGoldUI.text = "GOLD: " + playerGold;
        shopUI.SetActive(showShop);
        PopulateCharPriceInfo(warriorsGameObjects);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void PopulateCharPriceInfo(List<GameObject> warriors)
    {
        foreach (var warrior in warriors)
        {
            var charFeature = warrior.GetComponent<CharacterFeature>();
            _charTypePriceInfo.Add(charFeature.characterType, charFeature.spawnPrice);
        }
    }

    public void ToggleShopUI()
    {
        showShop = !showShop;
        shopUI.SetActive(showShop);
    }

    public void EarnPlayerGold(int earning)
    {
        playerGold += earning;
        playerGoldUI.text = "GOLD: " + playerGold;
    }

    public void PayPlayerGold(int payment)
    {
        playerGold -= payment;
        playerGoldUI.text = "GOLD: " + playerGold;
    }

    public bool CanPay(CharacterType characterType)
    {
        return playerGold >= _charTypePriceInfo[characterType];
    }
}