using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{
    private List<GameObject> _activeAllies;
    private List<GameObject> _activeEnemies;
    private Animator _animator;
    private float _attackRate;
    private bool _canAttack = true;
    private CharacterFeature _characterFeature;
    private GameManager _gameManager;

    private ShopHelper _shopHelper;
    private SpawnManager _spawnManager;

    private void Start()
    {
        Initialize();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ally"))
        {
            var opponentFeature = other.gameObject.GetComponent<CharacterFeature>();
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
        if (_characterFeature.isEnemy && (other.CompareTag("Ally") || other.CompareTag("AllyBase")))
            Attack(other);
        else if (!_characterFeature.isEnemy && (other.CompareTag("Enemy") || other.CompareTag("EnemyBase")))
            Attack(other);
    }

    private void Initialize()
    {
        _characterFeature = GetComponent<CharacterFeature>();
        _attackRate = _characterFeature.attackRate;
        _spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        _activeEnemies = _spawnManager.ActiveEnemies;
        _activeAllies = _spawnManager.ActiveAllies;
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
    }

    private void Attack(Collider other)
    {
        if (_canAttack && !_gameManager.GameOver)
        {
            _animator.SetTrigger("AttackTrigger");
            CloseAttack(other);
            StartCoroutine(AttackCooldown());
        }
    }

    private void CloseAttack(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ally"))
        {
            var opponentFeature = other.gameObject.GetComponent<CharacterFeature>();
            opponentFeature.currentHealth -= _characterFeature.power;
            opponentFeature.UpdateHealthBar();
        }
        else
        {
            var opponentFeature = other.gameObject.GetComponent<BaseFeature>();

            opponentFeature.currentHealth -= _characterFeature.power;
            opponentFeature.UpdateHealthBar();
        }
    }

    private void RemoveFromActiveOpponents(GameObject opponent)
    {
        if (_characterFeature.isEnemy)
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
}