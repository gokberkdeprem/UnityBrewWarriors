using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WarriorMoveController : MonoBehaviour
{
    [SerializeField] private SpawnManager _spawnManager;

    // [SerializeField] private bool _canMove = true;
    [SerializeField] private NavMeshAgent _agent;
    private Animator _animator;
    private GameManager _gameManager;
    private bool _moreWarriorsAround;
    private ShopHelper _shopHelper;
    private Warrior _warrior;
    [SerializeField] private float navmeshUpdateInterval = 2; 
    [SerializeField] private double _pathRecalculationTolerance = 1.0f;
    private Vector3 _lastTargetPosition;

    private void Start()
    {
        _warrior = GetComponent<Warrior>();
        _spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
        _gameManager.onGameOver.AddListener(x => OnGameOver());
        StartCoroutine(LateStart());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ShouldStopMoving(other)) _agent.speed = 0;

        var isAllyLayer = other.gameObject.layer == LayerMask.NameToLayer("Ally");
        var isEnemyLayer = other.gameObject.layer == LayerMask.NameToLayer("Enemy");

        if ((_warrior.isEnemy && isAllyLayer) || (!_warrior.isEnemy && isEnemyLayer))
            other.GetComponent<BattleEntity>().onDestroy.AddListener(x =>
            {
                if (!AnyOpponentAround()) Move();
            });
    }

    private void OnTriggerStay(Collider other)
    {
        var isAllyLayer = other.gameObject.layer == LayerMask.NameToLayer("Ally");
        var isEnemyLayer = other.gameObject.layer == LayerMask.NameToLayer("Enemy");

        if ((_warrior.isEnemy && isAllyLayer) || (!_warrior.isEnemy && isEnemyLayer)) _agent.speed = 0;
    }

    private void OnGameOver()
    {
        _agent.speed = 0;
        _animator.CrossFade("Idle", 0.1f, 0);
    }

    private void Move()
    {
        if (!_gameManager.GameOver && _warrior.currentHealth > 0)
        {
            _agent.speed = _warrior.speed;
            _animator.CrossFadeInFixedTime("Walk", 0.1f, 0);
        }
    }

    private bool AnyOpponentAround()
    {
        Collider[] results = { };
        var layer = _warrior.isEnemy ? "Ally" : "Enemy";
        var opponentCount =
            Physics.OverlapSphereNonAlloc(transform.position, 0.1f, results, LayerMask.NameToLayer(layer));

        if (opponentCount > 0)
            _warrior.SelectTarget(results[0].gameObject);
        else
            _warrior.SelectTarget();

        if (opponentCount > 0)
            return true;

        return false;
    }

    private bool ShouldStopMoving(Collider other)
    {
        return (_warrior.isEnemy && (other.CompareTag("Ally") || other.CompareTag("AllyBase")))
               || (!_warrior.isEnemy && (other.CompareTag("Enemy") || other.CompareTag("EnemyBase")));
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(UpdateDestination());
        Move();
    }
    
    IEnumerator UpdateDestination()
    {
        
        // while (true)
        // {
        //     if(_gameManager.GameOver)
        //         break;
        //     
        //     if (_agent.enabled && gameObject && _warrior.Target)
        //     {
        //         _agent.SetDestination(_warrior.Target.transform.position);
        //     }
        //     yield return new WaitForSeconds(navmeshUpdateInterval);
        // }
        
        while (true)
        {
            var target = _warrior.Target.transform;
            if(_gameManager.GameOver)
                break;
            
            if (_agent.enabled && gameObject && _warrior.Target)
            {
                if (Vector3.Distance(target.position, _lastTargetPosition) > _pathRecalculationTolerance)
                {
                    _agent.SetDestination(target.position);
                    _lastTargetPosition = target.position;
                }
            }
            yield return new WaitForSeconds(navmeshUpdateInterval);
        }
        
        
        
    }
    
}