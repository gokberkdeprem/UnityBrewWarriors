using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithSliderManager : MonoBehaviour
{
    [SerializeField] private GameObject warrior;
    [SerializeField] private Slider spawnSlider;
    [SerializeField] private Button spawnButton;
    [SerializeField] private float loadingSpeed;

    private void Start()
    {
        loadingSpeed = warrior.GetComponent<CharacterFeature>().spawnDelay;
        spawnSlider.value = 0f;
        spawnButton.onClick.AddListener(StartLoading);
    }

    private void StartLoading()
    {
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
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