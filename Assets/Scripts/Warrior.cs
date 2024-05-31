using Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Warrior : MonoBehaviour
{
    //Animator

    [SerializeField] public bool isEnemy;
    [SerializeField] public float maxHealth;
    [SerializeField] public float power;
    [SerializeField] public float attackRate;
    [SerializeField] public float currentHealth;
    [SerializeField] public float speed;
    [SerializeField] public CharacterType characterType;
    [SerializeField] public float spawnRate;
    [SerializeField] public int spawnPrice;
    [SerializeField] public int rewardPrice;
    [SerializeField] public int purchasePrice;
    [SerializeField] public int upgradePrice;

    [SerializeField] public UnityEvent<GameObject> onCharacterDeath;
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Camera mainCamera;
    private Animator _animator;

    //ShopManager
    private ShopManager _shopManager;
    private GameObject _shopManagerGameObject;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        isEnemy = gameObject.CompareTag("Enemy");
        currentHealth = maxHealth;
        _shopManagerGameObject = GameObject.FindWithTag("ShopManager");
        _shopManager = _shopManagerGameObject.GetComponent<ShopManager>();
    }

    private void OnDestroy()
    {
        if (isEnemy) _shopManager.EarnGold(rewardPrice);
    }

    private void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth / maxHealth;
    }

    public void GetDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            onCharacterDeath.Invoke(gameObject);
            _animator.CrossFade("Death", 0, 0);
            Destroy(gameObject, 3);
        }
    }
}