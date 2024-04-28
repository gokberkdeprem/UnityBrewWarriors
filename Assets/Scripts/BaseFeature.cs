using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseFeature : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [SerializeField] public float currentHealth;
    [SerializeField] public UnityEvent<GameObject> onBaseDeath;
    [SerializeField] private Slider healthBarSlider;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            onBaseDeath.Invoke(gameObject);
        }
    }

    public void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth / maxHealth;
    }
}