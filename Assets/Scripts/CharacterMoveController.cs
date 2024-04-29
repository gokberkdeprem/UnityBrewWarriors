using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    public bool canMove = true;
    private List<GameObject> _activeAllies;
    private List<GameObject> _activeEnemies;
    private GameObject _allyBase;
    private CharacterFeature _characterFeature;
    private GameObject _enemyBase;
    private SpawnManager _spawnManager;
    private GameObject _spawnManagerGameObject;

    // Start is called before the first frame update
    private void Start()
    {
        _characterFeature = gameObject.GetComponent<CharacterFeature>();
        _enemyBase = GameObject.FindWithTag("EnemyBaseFront");
        _allyBase = GameObject.FindWithTag("AllyBaseFront");
        _spawnManagerGameObject = GameObject.FindWithTag("SpawnManager");
        _spawnManager = _spawnManagerGameObject.GetComponent<SpawnManager>();
        _activeAllies = _spawnManager.activeAllies;
        _activeEnemies = _spawnManager.activeEnemies;
    }

    // Update is called once per frame
    private void Update()
    {
        // if (_characterFeature.characterType == CharacterType.StoneCharacter && _characterFeature.canAttack)
        //     canMove = false;
        // else if (_characterFeature.characterType == CharacterType.StoneCharacter && !_characterFeature.canAttack)
        //     canMove = true;

        if (canMove)
        {
            if (_characterFeature.isEnemy)
            {
                if (_activeAllies.Any(a => a.CompareTag("Ally")))
                {
                    var activeAllyTransform = _activeAllies.First(a => a.CompareTag("Ally")).transform;
                    gameObject.transform.LookAt(activeAllyTransform);
                }
                else
                {
                    gameObject.transform.LookAt(_allyBase.transform);
                }
            }
            else if (!_characterFeature.isEnemy)
            {
                if (_activeEnemies.Any(e => e.CompareTag("Enemy")))
                {
                    var activeEnemyTransform = _activeEnemies.First(e => e.CompareTag("Enemy")).transform;
                    gameObject.transform.LookAt(activeEnemyTransform);
                }
                else
                {
                    gameObject.transform.LookAt(_enemyBase.transform);
                }
            }

            Move();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_characterFeature.isEnemy && (other.CompareTag("Ally") || other.CompareTag("AllyBase")))
            canMove = false;
        else if (!_characterFeature.isEnemy && (other.CompareTag("Enemy") || other.CompareTag("EnemyBase")))
            canMove = false;

        if (other.CompareTag("Ally") || other.CompareTag("Enemy"))
            other.gameObject.GetComponent<CharacterFeature>().onCharacterDeath.AddListener(x => canMove = true);
    }

    private void Move()
    {
        transform.position += transform.forward * (_characterFeature.speed * Time.deltaTime);
    }
}