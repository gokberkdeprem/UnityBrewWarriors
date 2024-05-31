using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _activeAllies;
    [SerializeField] private List<GameObject> _activeEnemies;
    private Animator _animator;
    private float _attackRate;
    private bool _canAttack = true;
    private GameManager _gameManager;

    private ShopHelper _shopHelper;
    private SpawnManager _spawnManager;
    private Warrior _warrior;

    private void Start()
    {
        Initialize();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ally"))
        {
            var opponentFeature = other.gameObject.GetComponent<Warrior>();
            opponentFeature.onCharacterDeath.AddListener(RemoveFromActiveOpponents);
        }
        else if (other.CompareTag("EnemyBase") || other.CompareTag("AllyBase"))
        {
            var opponentFeature = other.gameObject.GetComponent<BaseFeature>();
            opponentFeature.onBaseDeath.AddListener(RemoveFromActiveOpponents);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (_warrior.isEnemy && (other.CompareTag("Ally") || other.CompareTag("AllyBase")))
            Attack(other);
        else if (!_warrior.isEnemy && (other.CompareTag("Enemy") || other.CompareTag("EnemyBase")))
            Attack(other);
    }

    private void Initialize()
    {
        _warrior = GetComponent<Warrior>();
        _attackRate = _warrior.attackRate;
        _spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        _activeEnemies = _spawnManager.ActiveEnemies;
        _activeAllies = _spawnManager.ActiveAllies;
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
    }

    private void Attack(Collider other)
    {
        if (_canAttack && !_gameManager.GameOver && _warrior.currentHealth > 0)
        {
            CloseAttack(other);
            StartCoroutine(AttackCooldown());
        }
    }

    private void CloseAttack(Collider other)
    {
        _animator.CrossFadeInFixedTime("Attack", 0.5f, 0, 0);

        if (other.CompareTag("Enemy") || other.CompareTag("Ally"))
        {
            var opponentFeature = other.gameObject.GetComponent<Warrior>();
            _animator.CrossFadeInFixedTime("Attack", 0.5f, 0);
        }
        else
        {
            var opponentFeature = other.gameObject.GetComponent<BaseFeature>();
            _animator.CrossFadeInFixedTime("Attack", 0.5f, 0);
            opponentFeature.GetDamage(_warrior.power);
        }
    }

    private void RemoveFromActiveOpponents(GameObject opponent)
    {
        if (_warrior.isEnemy)
            _activeAllies.Remove(opponent);
        else
            _activeEnemies.Remove(opponent);
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackRate);
        _canAttack = true;
    }

    public void Damage()
    {
        if (_warrior.isEnemy)
            _activeAllies.FirstOrDefault(x => x.CompareTag("Ally"))?.GetComponent<BaseFeature>().GetDamage(_warrior.power);
        else
            _activeEnemies.First().GetComponent<BaseFeature>().GetDamage(_warrior.power);
    }
}