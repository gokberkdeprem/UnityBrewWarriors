using UnityEngine;

public class WarriorMoveController : MonoBehaviour
{
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private bool _canMove = true;
    private GameObject _allyBase;
    private Animator _animator;
    private GameObject _enemyBase;
    private GameManager _gameManager;
    private bool _moreWarriorsAround;
    private ShopHelper _shopHelper;
    private Warrior _warrior;

    private void Start()
    {
        _warrior = GetComponent<Warrior>();
        _enemyBase = GameObject.FindWithTag("EnemyBaseFront");
        _allyBase = GameObject.FindWithTag("AllyBaseFront");
        _spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
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
            other.GetComponent<Warrior>().onDestroy.AddListener(x => _canMove = true);

        if (other.CompareTag("AllyBase") || other.CompareTag("EnemyBase"))
            other.GetComponent<Castle>().onDestroy.AddListener(x => { });
    }

    private void OnTriggerStay(Collider other)
    {
        var isAllyLayer = other.gameObject.layer == LayerMask.NameToLayer("Ally");
        var isEnemyLayer = other.gameObject.layer == LayerMask.NameToLayer("Enemy");

        if ((_warrior.isEnemy && isAllyLayer) || (!_warrior.isEnemy && isEnemyLayer)) _canMove = false;
    }

    private void UpdateRotation()
    {
        var target = _warrior.Target;

        if (target != null)
        {
            var lookPos = target.gameObject.transform.position;
            lookPos.y = transform.position.y; // Lock the Y-axis
            var rotation = Quaternion.LookRotation(lookPos - transform.position);
            var damping = 5f; // Adjust as needed
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
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