using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] warriors;
    public GameObject allyBaseFront;
    public GameObject enemyBaseFront;
    public GameObject allyBase;
    public GameObject enemyBase;
    public float enemySpawnDelay;
    public bool canSpawnEnemy = true;
    public List<GameObject> activeAllies = new();
    public List<GameObject> activeEnemies = new();

    //ShopManager
    private ShopManager _shopManager;
    private GameObject _shopManagerGameObject;

    // Start is called before the first frame update
    private void Start()
    {
        activeAllies.Add(allyBase);
        activeEnemies.Add(enemyBase);
        _shopManagerGameObject = GameObject.FindWithTag("ShopManager");
        _shopManager = _shopManagerGameObject.GetComponent<ShopManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (canSpawnEnemy)
        {
            InstantiateEnemy();
            StartCoroutine(SpawnCooldown());
        }
    }

    private void PayForInstantiate(GameObject go)
    {
        _shopManager.PayGold(go.GetComponent<CharacterFeature>().spawnPrice);
    }

    public void InstantiateStickCharacter()
    {
        if (_shopManager.CanInstantiate(CharacterType.StickCharacter))
        {
            var ally = Instantiate(warriors[(int)CharacterType.StickCharacter], RandomAllyPosition(),
                warriors[0].transform.rotation);
            activeAllies.Add(ally);
            PayForInstantiate(ally);
            ally.tag = "Ally";
        }
    }

    public void InstantiateSpearCharacter()
    {
        if (_shopManager.CanInstantiate(CharacterType.SpearCharacter))
        {
            var ally = Instantiate(warriors[(int)CharacterType.SpearCharacter], RandomAllyPosition(),
                warriors[0].transform.rotation);
            activeAllies.Add(ally);
            PayForInstantiate(ally);
            ally.tag = "Ally";
        }
    }

    public void InstantiateStoneCharacter()
    {
        if (_shopManager.CanInstantiate(CharacterType.StickCharacter))
        {
            var ally = Instantiate(warriors[(int)CharacterType.StoneCharacter], RandomAllyPosition(),
                warriors[0].transform.rotation);
            activeAllies.Add(ally);
            PayForInstantiate(ally);
            ally.tag = "Ally";
        }
    }

    private void InstantiateEnemy()
    {
        float randomZPos = Random.Range(-1, 2);
        var spawnPos = new Vector3(enemyBaseFront.transform.position.x, enemyBaseFront.transform.position.y,
            enemyBaseFront.transform.position.z + randomZPos);
        var enemy = Instantiate(warriors[Random.Range(0, 3)], spawnPos, warriors[0].transform.rotation);
        activeEnemies.Add(enemy);
        enemy.tag = "Enemy";
    }

    private Vector3 RandomAllyPosition()
    {
        float randomZPos = Random.Range(-2, 2);
        return new Vector3(allyBaseFront.transform.position.x, allyBaseFront.transform.position.y,
            allyBaseFront.transform.position.z + randomZPos);
    }

    private IEnumerator SpawnCooldown()
    {
        canSpawnEnemy = false;
        yield return new WaitForSeconds(enemySpawnDelay);
        canSpawnEnemy = true;
    }
}