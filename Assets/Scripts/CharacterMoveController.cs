using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private bool _canMove = true;
    private GameObject _allyBase;
    private Animator _animator;
    private GameObject _enemyBase;
    private GameManager _gameManager;
    private ShopHelper _shopHelper;
    private Warrior _warrior;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (_canMove && !_gameManager.GameOver && _warrior.currentHealth > 0)
        {
            _animator.CrossFade("Walk", 0, 0);
            UpdateRotation();
            Move();
        }
        else if (_gameManager.GameOver)
        {
            _animator.CrossFade("Idle", 0.1f, 0);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (ShouldStopMoving(other))
            _canMove = false;

        if (other.CompareTag("Ally") || other.CompareTag("Enemy"))
            other.GetComponent<Warrior>().onCharacterDeath.AddListener(x => { _canMove = true; });

        if (other.CompareTag("AllyBase") || other.CompareTag("EnemyBase"))
            other.GetComponent<BaseFeature>().onBaseDeath.AddListener(x => { });
    }


    private void Initialize()
    {
        _warrior = GetComponent<Warrior>();
        _enemyBase = GameObject.FindWithTag("EnemyBaseFront");
        _allyBase = GameObject.FindWithTag("AllyBaseFront");
        _spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
    }


    private void UpdateRotation()
    {
        var target = _warrior.isEnemy ? FindClosestAlly() : FindClosestEnemy();
        if (target != null)
        {
            var targetDirection = target.transform.position - gameObject.transform.position;
            var targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                Time.deltaTime * _warrior.speed * 2);
        }
        else
        {
            target = _warrior.isEnemy ? _allyBase : _enemyBase;
            var targetDirection = target.transform.position - gameObject.transform.position;
            var targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                Time.deltaTime * _warrior.speed * 2);
        }
    }

    private GameObject FindClosestAlly()
    {
        return _spawnManager.ActiveAllies.Find(ally => ally.CompareTag("Ally"));
    }

    private GameObject FindClosestEnemy()
    {
        return _spawnManager.ActiveEnemies.Find(enemy => enemy.CompareTag("Enemy"));
    }

    private void Move()
    {
        transform.position += transform.forward * (_warrior.speed * Time.deltaTime);
    }

    private bool ShouldStopMoving(Collider other)
    {
        return (_warrior.isEnemy && (other.CompareTag("Ally") || other.CompareTag("AllyBase")))
               || (!_warrior.isEnemy && (other.CompareTag("Enemy") || other.CompareTag("EnemyBase")));
    }
}