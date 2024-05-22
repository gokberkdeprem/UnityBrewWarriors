using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseFeature : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public UnityEvent<GameObject> onBaseDeath;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] public bool isEnemy;
    [SerializeField] public int destroyReward;

    private void Start()
    {
        Initialize();
    }

    private void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth / maxHealth;
    }

    public void GetDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0) onBaseDeath.Invoke(gameObject);
    }


    private void Initialize()
    {
        currentHealth = maxHealth;
        isEnemy = CompareTag("EnemyBase");
    }
}