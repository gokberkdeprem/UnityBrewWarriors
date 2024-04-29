using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseFeature : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [SerializeField] public float currentHealth;
    [SerializeField] public UnityEvent<GameObject> onBaseDeath;
    [SerializeField] private Slider healthBarSlider;
    public bool isEnemy;
    private Cubifier _cubifier;

    private void Start()
    {
        currentHealth = maxHealth;
        if (gameObject.CompareTag("AllyBase"))
            isEnemy = false;
        else if (gameObject.CompareTag("EnemyBase"))
            isEnemy = true;

        _cubifier = GetComponent<Cubifier>();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            // Destroy(gameObject);
            _cubifier.InstantDivideIntoCuboids();
            onBaseDeath.Invoke(gameObject);
        }
    }

    public void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth / maxHealth;
    }
}