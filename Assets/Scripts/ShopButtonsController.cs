using System.Collections;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonsController : MonoBehaviour
{
    [SerializeField] private GameObject shopManagerObject;

    [SerializeField] private Button stickPurchaseButton;
    [SerializeField] private Button stickUpgradeButton;
    [SerializeField] private Button sparePurchaseButton;
    [SerializeField] private Button spareUpgradeButton;
    [SerializeField] private Button stonePurchaseButton;
    [SerializeField] private Button stoneUpgradeButton;

    [SerializeField] private GameObject insufficientBalanceAlertText;

    public GameObject stickCharSpawnButton;
    public GameObject spareCharSpawnButton;
    public GameObject stoneCharSpawnButton;

    public GameObject stickChar;
    public GameObject spearChar;
    public GameObject stoneChar;

    private ShopManager _shopManager;

    private CharacterFeature _spearCharFeature;
    private CharacterFeature _stickCharFeature;
    private CharacterFeature _stoneCharFeature;

    private void Start()
    {
        spareCharSpawnButton.SetActive(false);
        stoneCharSpawnButton.SetActive(false);
        insufficientBalanceAlertText.SetActive(false);

        stickPurchaseButton.onClick.AddListener(PurchaseStickChar);
        stickUpgradeButton.onClick.AddListener(UpgradeStickChar);
        sparePurchaseButton.onClick.AddListener(PurchaseSpareChar);
        spareUpgradeButton.onClick.AddListener(UpgradeSpareChar);
        stonePurchaseButton.onClick.AddListener(PurchaseStoneChar);
        stoneUpgradeButton.onClick.AddListener(UpgradeStoneChar);
        _shopManager = shopManagerObject.GetComponent<ShopManager>();
        InitializeShopButtonTexts();
    }

    private void InitializeShopButtonTexts()
    {
    }


    private void PurchaseStickChar()
    {
        if (_shopManager.CanPurchase(CharacterType.StickCharacter))
        {
            _shopManager.PurchaseCharacter(CharacterType.StickCharacter);
            stickCharSpawnButton.SetActive(true);
            stickPurchaseButton.GetComponentInChildren<TMP_Text>().text = "Purchased!";
            stickPurchaseButton.GetComponent<Image>().color = Color.green;
            stickPurchaseButton.interactable = false;
        }
        else
        {
            StartCoroutine(AlertPurchaseFail(stickPurchaseButton));
        }
    }

    private void UpgradeStickChar()
    {
    }

    private void PurchaseSpareChar()
    {
        if (_shopManager.CanPurchase(CharacterType.SpearCharacter))
        {
            _shopManager.PurchaseCharacter(CharacterType.SpearCharacter);
            spareCharSpawnButton.SetActive(true);
            sparePurchaseButton.GetComponentInChildren<TMP_Text>().text = "Purchased!";
            sparePurchaseButton.GetComponent<Image>().color = Color.green;
            sparePurchaseButton.interactable = false;
        }
        else
        {
            StartCoroutine(AlertPurchaseFail(sparePurchaseButton));
        }
    }

    private void UpgradeSpareChar()
    {
    }

    private void PurchaseStoneChar()
    {
        if (_shopManager.CanPurchase(CharacterType.SpearCharacter))
        {
            _shopManager.PurchaseCharacter(CharacterType.SpearCharacter);
            stoneCharSpawnButton.SetActive(true);
            stonePurchaseButton.GetComponentInChildren<TMP_Text>().text = "Purchased!";
            stonePurchaseButton.GetComponent<Image>().color = Color.green;
            stonePurchaseButton.interactable = false;
        }
        else
        {
            StartCoroutine(AlertPurchaseFail(stonePurchaseButton));
        }
    }

    private void UpgradeStoneChar()
    {
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