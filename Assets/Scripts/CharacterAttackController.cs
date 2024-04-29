using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{
    private List<GameObject> _activeAllies;
    private List<GameObject> _activeEnemies;
    private float _attackRate;
    private bool _canAttack = true;
    private CharacterFeature _characterFeature;
    private SpawnManager _spawnManager;
    private GameObject _spawnManagerGameObject;

    // Start is called before the first frame update
    private void Start()
    {
        _characterFeature = gameObject.GetComponent<CharacterFeature>();
        _attackRate = _characterFeature.attackRate;
        _spawnManagerGameObject = GameObject.FindWithTag("SpawnManager");
        _spawnManager = _spawnManagerGameObject.GetComponent<SpawnManager>();
        _activeEnemies = _spawnManager.activeEnemies;
        _activeAllies = _spawnManager.activeAllies;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnTriggerStay(Collider other)
    {
        if (_characterFeature.isEnemy && (other.CompareTag("Ally") || other.CompareTag("AllyBase")))
            Attack(other);
        else if (!_characterFeature.isEnemy && (other.CompareTag("Enemy") || other.CompareTag("EnemyBase")))
            Attack(other);
    }


    private void Attack(Collider other)
    {
        CloseAttack(other);
    }

    private void CloseAttack(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ally"))
        {
            var opponentCharFeature = other.gameObject.GetComponent<CharacterFeature>();
            if (_canAttack)
            {
                opponentCharFeature.currentHealth -= _characterFeature.power;
                opponentCharFeature.UpdateHealthBar();
                opponentCharFeature.onCharacterDeath.AddListener(RemoveFromActiveOpponents);
                StartCoroutine(AttackCoolDown());
            }
        }
        else if (other.CompareTag("EnemyBase") || other.CompareTag("AllyBase"))
        {
            var opponentCharFeature = other.gameObject.GetComponent<BaseFeature>();
            if (_canAttack)
            {
                opponentCharFeature.currentHealth -= _characterFeature.power;
                opponentCharFeature.UpdateHealthBar();
                opponentCharFeature.onBaseDeath.AddListener(RemoveFromActiveOpponents);
                StartCoroutine(AttackCoolDown());
            }
        }
    }

    private void RemoveFromActiveOpponents(GameObject go)
    {
        var isEnemy = go.GetComponent<CharacterFeature>().isEnemy;
        if (isEnemy) _activeEnemies.Remove(go);
        else _activeAllies.Remove(go);
    }

    private IEnumerator AttackCoolDown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackRate);
        _canAttack = true;
    }

    // private void LongAttack()
    // {
    //     if (_canAttack && _characterFeature.characterType == CharacterType.StoneCharacter &&
    //         _characterFeature.canAttack)
    //     {
    //         if (_characterFeature.isEnemy)
    //         {
    //             if (_characterFeature.closestAlly.CompareTag("Ally"))
    //             {
    //                 _characterFeature.closestAlly.GetComponent<CharacterFeature>().currentHealth -= 1;
    //                 _characterFeature.closestAlly.GetComponent<CharacterFeature>().UpdateHealthBar();
    //                 _characterFeature.closestAlly.GetComponent<CharacterFeature>().onCharacterDeath
    //                     .AddListener(x => _characterFeature.canAttack = false);
    //             }
    //             else
    //             {
    //                 _characterFeature.closestAlly.GetComponent<BaseFeature>().currentHealth -= 1;
    //                 _characterFeature.closestAlly.GetComponent<BaseFeature>().UpdateHealthBar();
    //                 _characterFeature.closestAlly.GetComponent<BaseFeature>().onBaseDeath
    //                     .AddListener(x => _characterFeature.canAttack = false);
    //             }
    //         }
    //         else if (!_characterFeature.isEnemy)
    //         {
    //             if (_characterFeature.closestEnemy.CompareTag("Enemy"))
    //             {
    //                 _characterFeature.closestEnemy.GetComponent<CharacterFeature>().currentHealth -= 1;
    //                 _characterFeature.closestEnemy.GetComponent<CharacterFeature>().UpdateHealthBar();
    //                 _characterFeature.closestEnemy.GetComponent<CharacterFeature>().onCharacterDeath
    //                     .AddListener(x => _characterFeature.canAttack = false);
    //             }
    //             else
    //             {
    //                 _characterFeature.closestEnemy.GetComponent<BaseFeature>().currentHealth -= 1;
    //                 _characterFeature.closestEnemy.GetComponent<BaseFeature>().UpdateHealthBar();
    //                 _characterFeature.closestEnemy.GetComponent<BaseFeature>().onBaseDeath
    //                     .AddListener(x => _characterFeature.canAttack = false);
    //             }
    //         }
    //
    //         StartCoroutine(AttackCoolDown());
    //     }
    // }
}