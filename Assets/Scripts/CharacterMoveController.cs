using System.Linq;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    public bool canMove = true;
    private GameObject _allyBase;
    private CharacterFeature _characterFeature;
    private GameObject _enemyBase;

    // Start is called before the first frame update
    private void Start()
    {
        _characterFeature = gameObject.GetComponent<CharacterFeature>();
        _enemyBase = GameObject.FindWithTag("EnemyBaseFront");
        _allyBase = GameObject.FindWithTag("AllyBaseFront");
    }

    // Update is called once per frame
    private void Update()
    {
        // TODO: Optimize the activeAllies and activeEnemies arrays.
        if (canMove)
        {
            if (_characterFeature.isEnemy)
            {
                var activeAllies = GameObject.FindGameObjectsWithTag("Ally");
                if (activeAllies.Length > 0)
                {
                    var activeAllyTransform = activeAllies.First().transform;
                    gameObject.transform.LookAt(activeAllyTransform);
                }
                else
                {
                    gameObject.transform.LookAt(_allyBase.transform);
                }
            }
            else if (!_characterFeature.isEnemy)
            {
                var activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

                if (activeEnemies.Length > 0)
                {
                    var activeEnemyTransform = activeEnemies.First().transform;
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

    private void OnTriggerEnter(Collider other)
    {
        if (_characterFeature.isEnemy && (other.CompareTag("Ally") || other.CompareTag("AllyBase")))
            canMove = false;
        else if (!_characterFeature.isEnemy && (other.CompareTag("Enemy") || other.CompareTag("EnemyBase")))
            canMove = false;
    }

    private void Move()
    {
        transform.position += transform.forward * (_characterFeature.speed * Time.deltaTime);
    }
}