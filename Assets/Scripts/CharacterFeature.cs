using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterFeature : MonoBehaviour
{
    [SerializeField] public bool isEnemy;
    [SerializeField] public float maxHealth;
    [SerializeField] public float power;
    [SerializeField] public float attackRate;
    [SerializeField] public float currentHealth;
    [SerializeField] public float speed;
    [SerializeField] public UnityEvent<GameObject> onCharacterDeath;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        isEnemy = gameObject.CompareTag("Enemy");
        mainCamera = Camera.main;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        FixHealthBarRotation();
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            onCharacterDeath.Invoke(gameObject);
        }
    }

    private void FixHealthBarRotation()
    {
        healthBarSlider.transform.LookAt(mainCamera.transform);
    }

    public void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth / maxHealth;
    }
}