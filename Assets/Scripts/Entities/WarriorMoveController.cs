using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WarriorMoveController : MonoBehaviour
{
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float navmeshUpdateInterval = 1;
    [SerializeField] private double _pathRecalculationTolerance = 1.0f;
    [SerializeField] private float _walkAnimationMultiplier;
    private Animator _animator;
    private GameManager _gameManager;
    private Helper _helper;
    private Vector3 _lastTargetPosition;
    private bool _moreWarriorsAround;
    private Warrior _warrior;

    private void Start()
    {
        _warrior = GetComponent<Warrior>();
        _spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
        _gameManager.onGameOver.AddListener(x => OnGameOver());
        StartCoroutine(LateStart());
    }

    // TODO: Will be deleted
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ShouldStopMoving(other)) _agent.speed = 0;

        var isAllyLayer = other.gameObject.layer == LayerMask.NameToLayer("Ally");
        var isEnemyLayer = other.gameObject.layer == LayerMask.NameToLayer("Enemy");

        if ((_warrior.isEnemy && isAllyLayer) || (!_warrior.isEnemy && isEnemyLayer))
        {
            other.GetComponent<BattleEntity>().onDestroy.AddListener(x =>
            {
                if (!AnyOpponentAround()) Move();
            });
            _warrior.SelectTarget(other.gameObject);
            StartCoroutine(RotateTowardsTarget());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var isAllyLayer = other.gameObject.layer == LayerMask.NameToLayer("Ally");
        var isEnemyLayer = other.gameObject.layer == LayerMask.NameToLayer("Enemy");

        if ((_warrior.isEnemy && isAllyLayer) || (!_warrior.isEnemy && isEnemyLayer)) _agent.speed = 0;
    }

    private IEnumerator RotateTowardsTarget()
    {
        var rotationSpeed = _warrior.speed * 100;
        var target = _warrior.Target;
        var rotationDuration = 2;
        while (rotationDuration > 0)
            if (target && _warrior.TargetBattleEntity.currentHealth > 0)
            {
                var targetDirection = (target.transform.position - transform.position).normalized;
                var targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                rotationDuration -= 1;
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield break;
            }
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
        // var isAllyLayer = other.gameObject.layer == LayerMask.NameToLayer("Ally");
        // var isEnemyLayer = other.gameObject.layer == LayerMask.NameToLayer("Enemy");
        //
        // return (_warrior.isEnemy && isAllyLayer) || (!_warrior.isEnemy && isEnemyLayer);

        return _warrior.Target == other.gameObject;
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(UpdateDestination());
        SetWalkAnimationSpeed();
        Move();
    }

    private IEnumerator UpdateDestination()
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
            var target = _warrior?.Target?.transform;
            if (_gameManager.GameOver && !target)
                break;

            if (_agent.enabled && gameObject && _warrior.Target)
                if (Vector3.Distance(target.position, _lastTargetPosition) > _pathRecalculationTolerance)
                {
                    _agent.SetDestination(target.position);
                    _lastTargetPosition = target.position;
                }

            yield return new WaitForSeconds(navmeshUpdateInterval);
        }
    }

    private void SetWalkAnimationSpeed()
    {
        _animator.SetFloat("AnimMultiplier", _walkAnimationMultiplier * _warrior.speed);
    }
}