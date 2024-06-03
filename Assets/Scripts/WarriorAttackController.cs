using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttackController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _activeAllies;
    [SerializeField] private List<GameObject> _activeEnemies;
    private Animator _animator;
    private float _attackRate;
    private bool _canAttack = true;
    private GameManager _gameManager;

    private ShopHelper _shopHelper;
    private SpawnManager _spawnManager;
    private BattleEntity _target;
    private Warrior _warrior;

    private void Start()
    {
        _warrior = GetComponent<Warrior>();
        _attackRate = _warrior.attackRate;
        _spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        _activeEnemies = _spawnManager.ActiveEnemies;
        _activeAllies = _spawnManager.ActiveAllies;
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var opponentFeature = other.gameObject.GetComponent<Warrior>();
        if (opponentFeature) opponentFeature.onDestroy.AddListener(RemoveFromActiveOpponents);
    }

    public void OnTriggerStay(Collider other)
    {
        var isAllyLayer = other.gameObject.layer == LayerMask.NameToLayer("Ally");
        var isEnemyLayer = other.gameObject.layer == LayerMask.NameToLayer("Enemy");

        if (_warrior.isEnemy && isAllyLayer)
            Attack();
        else if (!_warrior.isEnemy && isEnemyLayer)
            Attack();
    }

    private void Attack()
    {
        if (_canAttack && !_gameManager.GameOver && _warrior.currentHealth > 0)
        {
            CloseAttack();
            StartCoroutine(AttackCooldown());
        }
    }

    private void CloseAttack()
    {
        _animator.CrossFadeInFixedTime("Attack", 0.5f, 0, 0);
    }

    private void RemoveFromActiveOpponents(GameObject opponent)
    {
        if (_warrior.isEnemy)
            _activeAllies.Remove(opponent);
        else
            _activeEnemies.Remove(opponent);

        _warrior.SelectTarget();
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackRate);
        _canAttack = true;
    }

    public void Damage()
    {
        _warrior?.TargetBattleEntity?.GetDamage(_warrior.power);
    }
}