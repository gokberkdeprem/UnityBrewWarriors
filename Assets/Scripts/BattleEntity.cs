using Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BattleEntity : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [SerializeField] public float currentHealth;

    public UnityEvent<GameObject> onDestroy;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] public bool isEnemy;
    [SerializeField] public int destroyReward;
    [SerializeField] public WarriorType warriorType;
    [SerializeField] public EntityType EntityType;
    private ShopManager _shopManager;
    private GameObject _shopManagerGameObject;
    protected SpawnManager SpawnManager;

    protected virtual void Start()
    {
        isEnemy = gameObject.CompareTag("Enemy");
        currentHealth = maxHealth;
        _shopManagerGameObject = GameObject.FindWithTag("ShopManager");
        _shopManager = _shopManagerGameObject.GetComponent<ShopManager>();
        var spawnManagerGameObject = GameObject.FindWithTag("SpawnManager");
        SpawnManager = spawnManagerGameObject.GetComponent<SpawnManager>();
    }

    protected void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth / maxHealth;
    }

    public virtual void GetDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            onDestroy.Invoke(gameObject);
            if (isEnemy) _shopManager.EarnGold(destroyReward);
        }
    }
}