using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [SerializeField] private CharacterFeature _characterFeature;
    [SerializeField] private GameObject _allyBase;
    [SerializeField] private GameObject _enemyBase;
    [SerializeField] private SpawnManager _spawnManager;

    private bool CanMove { get; set; } = true;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (CanMove)
        {
            UpdateRotation();
            Move();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ally") || other.CompareTag("Enemy"))
            other.GetComponent<CharacterFeature>().onCharacterDeath.AddListener(x => CanMove = true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (ShouldStopMoving(other))
            CanMove = false;
    }

    private void Initialize()
    {
        _characterFeature = GetComponent<CharacterFeature>();
        _enemyBase = GameObject.FindWithTag("EnemyBaseFront");
        _allyBase = GameObject.FindWithTag("AllyBaseFront");
        _spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
    }

    private void UpdateRotation()
    {
        var target = _characterFeature.isEnemy ? FindClosestAlly() : FindClosestEnemy();
        if (target != null)
            transform.LookAt(target.transform);
        else
            transform.LookAt(_characterFeature.isEnemy ? _allyBase.transform : _enemyBase.transform);
    }

    private GameObject FindClosestAlly()
    {
        return _spawnManager.activeAllies.Find(ally => ally.CompareTag("Ally"));
    }

    private GameObject FindClosestEnemy()
    {
        return _spawnManager.activeEnemies.Find(enemy => enemy.CompareTag("Enemy"));
    }

    private void Move()
    {
        transform.position += transform.forward * (_characterFeature.speed * Time.deltaTime);
    }

    private bool ShouldStopMoving(Collider other)
    {
        return (_characterFeature.isEnemy && (other.CompareTag("Ally") || other.CompareTag("AllyBase")))
               || (!_characterFeature.isEnemy && (other.CompareTag("Enemy") || other.CompareTag("EnemyBase")));
    }

    // private void OnCharacterDeath(GameObject character)
    // {
    //     CanMove = true;
    //     character.GetComponent<CharacterFeature>().onCharacterDeath.RemoveListener();
    // }
}