using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{
    [SerializeField] private List<GameObject> activeOpponents = new();
    private float _attackRate;
    private bool _canAttack = true;
    private CharacterFeature _characterFeature;
    private CharacterMoveController _characterMoveController;

    // Start is called before the first frame update
    private void Start()
    {
        _characterFeature = gameObject.GetComponent<CharacterFeature>();
        _characterMoveController = gameObject.GetComponent<CharacterMoveController>();
        _attackRate = _characterFeature.attackRate;
    }

    // Update is called once per frame
    private void Update()
    {
        if (activeOpponents.Count <= 0)
            _characterMoveController.canMove = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (_characterFeature.isEnemy && (other.CompareTag("Ally") || other.CompareTag("AllyBase")))
            activeOpponents.Add(other.gameObject);
        else if (!_characterFeature.isEnemy && (other.CompareTag("Enemy") || other.CompareTag("EnemyBase")))
            activeOpponents.Add(other.gameObject);
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
        if (other.CompareTag("Enemy") || other.CompareTag("Ally"))
        {
            var otherCharFeature = other.gameObject.GetComponent<CharacterFeature>();
            if (_canAttack)
            {
                otherCharFeature.currentHealth -= _characterFeature.power;
                otherCharFeature.UpdateHealthBar();
                otherCharFeature.onCharacterDeath.AddListener(RemoveFromActiveOpponents);
                StartCoroutine(AttackCoolDown());
            }
        }
        else if (other.CompareTag("EnemyBase") || other.CompareTag("AllyBase"))
        {
            var otherCharFeature = other.gameObject.GetComponent<BaseFeature>();
            if (_canAttack)
            {
                otherCharFeature.currentHealth -= _characterFeature.power;
                otherCharFeature.UpdateHealthBar();
                otherCharFeature.onBaseDeath.AddListener(RemoveFromActiveOpponents);
                StartCoroutine(AttackCoolDown());
            }
        }
    }

    private void RemoveFromActiveOpponents(GameObject go)
    {
        activeOpponents.Remove(go);
    }

    private IEnumerator AttackCoolDown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackRate);
        _canAttack = true;
    }
}