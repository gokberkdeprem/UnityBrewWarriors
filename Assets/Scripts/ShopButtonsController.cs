using System.Collections;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonsController : MonoBehaviour
{
    [SerializeField] private GameObject stickCharSpawnButton;
    [SerializeField] private GameObject spearCharSpawnButton;
    [SerializeField] private GameObject stoneCharSpawnButton;

    [SerializeField] private Button stickPurchaseButton;
    [SerializeField] private Button stickUpgradeButton;
    [SerializeField] private Button spearPurchaseButton;
    [SerializeField] private Button spearUpgradeButton;
    [SerializeField] private Button stonePurchaseButton;
    [SerializeField] private Button stoneUpgradeButton;

    [SerializeField] private GameObject insufficientBalanceAlertText;

    public List<GameObject> warriorsGameObjects;
    private Dictionary<CharacterType, CharacterFeature> _characterFeatures;
    private ShopHelper _shopHelper;
    private ShopManager _shopManager;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        stickCharSpawnButton.SetActive(true);
        spearCharSpawnButton.SetActive(false);
        stoneCharSpawnButton.SetActive(false);
        insufficientBalanceAlertText.SetActive(false);

        _shopManager = GetComponent<ShopManager>();
        _shopHelper = GetComponent<ShopHelper>();
        _characterFeatures = _shopHelper.CharTypeToFeatureDict;

        InitializeButtonsAndTexts();
    }

    private void UpdateButtonText(CharacterType type, Button button, bool isPurchaseButton)
    {
        var feature = _characterFeatures[type];

        if (isPurchaseButton)
            button.GetComponentInChildren<TMP_Text>().text = $"Purchase \n ({feature.purchasePrice:0.00} GOLD)";
        else
            button.GetComponentInChildren<TMP_Text>().text =
                $"Upgrade \n ({feature.spawnRate:0.00} seconds) \n ({feature.upgradePrice:0.00} GOLD)";
    }

    private void InitializeButtonsAndTexts()
    {
        stickPurchaseButton.onClick.AddListener(PurchaseStickChar);
        UpdateButtonText(CharacterType.StickCharacter, stickPurchaseButton, true);

        stickUpgradeButton.onClick.AddListener(UpgradeStickChar);
        UpdateButtonText(CharacterType.StickCharacter, stickUpgradeButton, false);

        spearPurchaseButton.onClick.AddListener(PurchaseSpareChar);
        UpdateButtonText(CharacterType.SpearCharacter, spearPurchaseButton, true);

        spearUpgradeButton.onClick.AddListener(UpgradeSpareChar);
        UpdateButtonText(CharacterType.SpearCharacter, spearUpgradeButton, false);

        stonePurchaseButton.onClick.AddListener(PurchaseStoneChar);
        UpdateButtonText(CharacterType.StoneCharacter, stonePurchaseButton, true);

        stoneUpgradeButton.onClick.AddListener(UpgradeStoneChar);
        UpdateButtonText(CharacterType.StoneCharacter, stoneUpgradeButton, false);
    }

    private void PurchaseStickChar()
    {
        PurchaseCharacter(CharacterType.StickCharacter, stickCharSpawnButton, stickPurchaseButton, stickUpgradeButton);
    }

    private void UpgradeStickChar()
    {
        UpgradeCharacter(CharacterType.StickCharacter, stickUpgradeButton);
    }

    private void PurchaseSpareChar()
    {
        PurchaseCharacter(CharacterType.SpearCharacter, spearCharSpawnButton, spearPurchaseButton, spearUpgradeButton);
    }

    private void UpgradeSpareChar()
    {
        UpgradeCharacter(CharacterType.SpearCharacter, spearUpgradeButton);
    }

    private void PurchaseStoneChar()
    {
        PurchaseCharacter(CharacterType.StoneCharacter, stoneCharSpawnButton, stonePurchaseButton, stoneUpgradeButton);
    }

    private void UpgradeStoneChar()
    {
        UpgradeCharacter(CharacterType.StoneCharacter, stoneUpgradeButton);
    }

    private void UpgradeCharacter(CharacterType type, Button upgradeButton)
    {
        if (_shopManager.CanUpgrade(type))
        {
            _shopManager.UpgradeCharacter(type);
            UpdateButtonText(type, upgradeButton, false);
        }
        else
        {
            StartCoroutine(AlertPurchaseFail(upgradeButton));
        }
    }

    private void PurchaseCharacter(CharacterType type, GameObject spawnButton, Button purchaseButton,
        Button upgradeButton)
    {
        if (_shopManager.CanPurchase(type))
        {
            _shopManager.PurchaseCharacter(type);
            spawnButton.SetActive(true);
            upgradeButton.interactable = true;

            purchaseButton.GetComponentInChildren<TMP_Text>().text = "Purchased!";
            purchaseButton.GetComponent<Image>().color = Color.green;
            purchaseButton.interactable = false;
        }
        else
        {
            StartCoroutine(AlertPurchaseFail(purchaseButton));
        }
    }

    private IEnumerator AlertPurchaseFail(Button button)
    {
        var buttonColor = button.GetComponent<Image>().color;
        insufficientBalanceAlertText.SetActive(true);
        button.interactable = false;
        button.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(1);
        button.GetComponent<Image>().color = buttonColor;
        button.interactable = true;
        insufficientBalanceAlertText.SetActive(false);
    }
}