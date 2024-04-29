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

    // //StoneCharacter options
    // [SerializeField] public GameObject closestAlly;
    // [SerializeField] public GameObject closestEnemy;
    // [SerializeField] private float attackRange;
    // [SerializeField] public bool canAttack = true;
    // private List<GameObject> _activeAllies;
    // private List<GameObject> _activeEnemies;

    //SpawnManager
    // private SpawnManager _spawnManager;
    // private GameObject _spawnManagerGameObject;

    private void Start()
    {
        isEnemy = gameObject.CompareTag("Enemy");
        mainCamera = Camera.main;
        currentHealth = maxHealth;
        // _spawnManagerGameObject = GameObject.FindWithTag("SpawnManager");
        // _spawnManager = _spawnManagerGameObject.GetComponent<SpawnManager>();
        // _activeEnemies = _spawnManager.activeEnemies;
        // _activeAllies = _spawnManager.activeAllies;
        // closestAlly = _activeAllies.First();
        // closestEnemy = _activeEnemies.First();
    }

    private void Update()
    {
        // if (characterType == CharacterType.StoneCharacter)
        //     FindClosestOpponent();

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

    // private void FindClosestOpponent()
    // {
    //     if (isEnemy)
    //     {
    //         var minDistance = 20f;
    //
    //         var x = closestAlly?.transform.position.x - gameObject.transform.position.x;
    //         minDistance = x ?? 20;
    //
    //
    //         foreach (var ally in _activeAllies)
    //         {
    //             var distance = Math.Abs(ally.transform.position.x - gameObject.transform.position.x);
    //             if (distance < minDistance)
    //                 closestAlly = ally;
    //         }
    //
    //         canAttack = minDistance <= attackRange;
    //     }
    //     else
    //     {
    //         var minDistance = 20f;
    //
    //         var x = closestEnemy?.transform.position.x - gameObject.transform.position.x;
    //         if (x != null) minDistance = Math.Abs((float)x);
    //
    //         foreach (var enemy in _activeEnemies)
    //         {
    //             var y = enemy?.transform.position.x - gameObject.transform.position.x;
    //             if (y != null)
    //             {
    //                 var distance = Math.Abs((float)y);
    //                 if (distance < minDistance)
    //                     closestEnemy = enemy;
    //             }
    //         }
    //
    //         canAttack = minDistance <= attackRange;
    //     }
    // }
}