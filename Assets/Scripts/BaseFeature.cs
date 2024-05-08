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
    [SerializeField] public int destroyPrice;
    private Cubifier _cubifier;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            _cubifier.InstantDivideIntoCuboids();
            onBaseDeath.Invoke(gameObject);
        }
    }

    public void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth / maxHealth;
    }

    private void Initialize()
    {
        currentHealth = maxHealth;
        isEnemy = CompareTag("EnemyBase");
        _cubifier = GetComponent<Cubifier>();
    }
}