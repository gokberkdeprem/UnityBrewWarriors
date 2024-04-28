using UnityEngine;
using UnityEngine.Events;
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

    private void Start()
    {
        isEnemy = gameObject.CompareTag("Enemy");
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            onCharacterDeath.Invoke(gameObject);
        }
    }

    public void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth / maxHealth;
    }
}