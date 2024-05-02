using Enums;
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
    [SerializeField] private Camera mainCamera;
    [SerializeField] public CharacterType characterType;
    [SerializeField] public float spawnDelay;
    [SerializeField] public int spawnPrice;
    [SerializeField] public int rewardPrice;
    [SerializeField] public int purchasePrice;

    //ShopManager
    private ShopManager _shopManager;
    private GameObject _shopManagerGameObject;

    private void Start()
    {
        isEnemy = gameObject.CompareTag("Enemy");
        mainCamera = Camera.main;
        currentHealth = maxHealth;
        _shopManagerGameObject = GameObject.FindWithTag("ShopManager");
        _shopManager = _shopManagerGameObject.GetComponent<ShopManager>();
    }

    private void Update()
    {
        FixHealthBarRotation();
        if (currentHealth <= 0)
        {
            if (isEnemy) _shopManager.EarnGold(rewardPrice);

            onCharacterDeath.Invoke(gameObject);
            Destroy(gameObject);
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