using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithSliderManager : MonoBehaviour
{
    [SerializeField] private GameObject warrior;
    [SerializeField] private Slider spawnSlider;
    [SerializeField] private Button spawnButton;
    [SerializeField] private bool isInteractable;
    [SerializeField] private float loadingSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        loadingSpeed = warrior.GetComponent<CharacterFeature>().spawnDelay;
        spawnSlider.value = 0f;
        spawnButton.onClick.AddListener(StartLoading);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void StartLoading()
    {
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        spawnButton.interactable = false; // Disable the button during loading

        // Simulate loading progress
        var progress = 1f;
        while (progress > 0f)
        {
            progress -= 1 / loadingSpeed * Time.deltaTime;
            spawnSlider.value = progress;
            yield return null; // Wait for the next frame
        }

        // Loading complete
        spawnButton.interactable = true; // Enable the button
    }
}