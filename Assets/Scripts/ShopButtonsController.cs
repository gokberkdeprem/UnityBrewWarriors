using System.Collections;
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
    [SerializeField] private Button spareUpgradeButton;
    [SerializeField] private Button stonePurchaseButton;
    [SerializeField] private Button stoneUpgradeButton;

    [SerializeField] private GameObject insufficientBalanceAlertText;

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

        stickPurchaseButton.onClick.AddListener(PurchaseStickChar);
        stickUpgradeButton.onClick.AddListener(UpgradeStickChar);
        spearPurchaseButton.onClick.AddListener(PurchaseSpareChar);
        spareUpgradeButton.onClick.AddListener(UpgradeSpareChar);
        stonePurchaseButton.onClick.AddListener(PurchaseStoneChar);
        stoneUpgradeButton.onClick.AddListener(UpgradeStoneChar);

        _shopManager = GetComponent<ShopManager>();
    }

    private void PurchaseStickChar()
    {
        PurchaseCharacter(CharacterType.StickCharacter, stickCharSpawnButton, stickPurchaseButton);
    }

    private void UpgradeStickChar()
    {
    }

    private void PurchaseSpareChar()
    {
        PurchaseCharacter(CharacterType.SpearCharacter, spearCharSpawnButton, spearPurchaseButton);
    }

    private void UpgradeSpareChar()
    {
    }

    private void PurchaseStoneChar()
    {
        PurchaseCharacter(CharacterType.StoneCharacter, stoneCharSpawnButton, stonePurchaseButton);
    }

    private void UpgradeStoneChar()
    {
    }

    private void UpgradeCharacter(CharacterType type, Button upgradeButton)
    {
        // if (_shopManager.CanUpgrade(type)) _shopManager.UpgradeCharacter();
    }

    private void PurchaseCharacter(CharacterType type, GameObject spawnButton, Button purchaseButton)
    {
        if (_shopManager.CanPurchase(type))
        {
            _shopManager.PurchaseCharacter(type);
            spawnButton.SetActive(true);
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