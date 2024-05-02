using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithSliderManager : MonoBehaviour
{
    [SerializeField] private GameObject warrior;
    [SerializeField] private Slider spawnSlider;
    [SerializeField] private Button spawnButton;
    private CharacterFeature _characterFeature;
    private ShopManager _shopManager;
    private GameObject _shopManagerGameObject;

    private void Start()
    {
        _characterFeature = warrior.GetComponent<CharacterFeature>();
        spawnSlider.value = 0f;
        spawnButton.onClick.AddListener(StartLoading);
        _shopManagerGameObject = GameObject.Find("ShopManager");
        _shopManager = _shopManagerGameObject.GetComponent<ShopManager>();
    }

    private void StartLoading()
    {
        if (_shopManager.CanInstantiate(_characterFeature.characterType))
            StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        var loadingSpeed = _characterFeature.spawnRate;
        spawnButton.interactable = false;

        var progress = 1f;
        while (progress > 0f)
        {
            progress -= 1 / loadingSpeed * Time.deltaTime;
            spawnSlider.value = progress;
            yield return null;
        }

        spawnButton.interactable = true;
    }
}