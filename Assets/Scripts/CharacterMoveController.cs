using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [SerializeField] private SpawnManager _spawnManager;
    private GameObject _allyBase;
    private Animator _animator;
    private CharacterFeature _characterFeature;
    private GameObject _enemyBase;
    private GameManager _gameManager;
    private ShopHelper _shopHelper;

    [SerializeField] private bool CanMove { get; set; } = true;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (CanMove && !_gameManager.GameOver)
        {
            _animator.CrossFade("Walk", 0, 0);
            UpdateRotation();
            Move();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ally") || other.CompareTag("Enemy"))
            other.GetComponent<CharacterFeature>().onCharacterDeath.AddListener(x => { CanMove = true; });

        if (other.CompareTag("AllyBase") || other.CompareTag("EnemyBase"))
            other.GetComponent<BaseFeature>().onBaseDeath.AddListener(x => { });
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
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
    }


    private void UpdateRotation()
    {
        var target = _characterFeature.isEnemy ? FindClosestAlly() : FindClosestEnemy();
        if (target != null)
        {
            var targetDirection = target.transform.position - gameObject.transform.position;
            var targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                Time.deltaTime * _characterFeature.speed * 2);
        }
        else
        {
            target = _characterFeature.isEnemy ? _allyBase : _enemyBase;
            var targetDirection = target.transform.position - gameObject.transform.position;
            var targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                Time.deltaTime * _characterFeature.speed * 2);
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
        transform.position += transform.forward * (_characterFeature.speed * Time.deltaTime);
    }

    private bool ShouldStopMoving(Collider other)
    {
        return (_characterFeature.isEnemy && (other.CompareTag("Ally") || other.CompareTag("AllyBase")))
               || (!_characterFeature.isEnemy && (other.CompareTag("Enemy") || other.CompareTag("EnemyBase")));
    }
}