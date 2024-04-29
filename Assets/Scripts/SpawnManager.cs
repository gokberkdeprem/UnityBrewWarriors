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

    // Start is called before the first frame update
    private void Start()
    {
        activeAllies.Add(allyBase);
        activeEnemies.Add(enemyBase);
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

    public void InstantiateStickCharacter()
    {
        var ally = Instantiate(warriors[(int)CharacterType.StickCharacter], RandomAllyPosition(),
            warriors[0].transform.rotation);
        ally.GetComponent<CharacterFeature>().characterType = CharacterType.StickCharacter;
        activeAllies.Add(ally);
        ally.tag = "Ally";
    }

    public void InstantiateSpearCharacter()
    {
        var ally = Instantiate(warriors[(int)CharacterType.SpearCharacter], RandomAllyPosition(),
            warriors[0].transform.rotation);
        ally.GetComponent<CharacterFeature>().characterType = CharacterType.SpearCharacter;
        activeAllies.Add(ally);
        ally.tag = "Ally";
    }

    public void InstantiateStoneCharacter()
    {
        var ally = Instantiate(warriors[(int)CharacterType.StoneCharacter], RandomAllyPosition(),
            warriors[0].transform.rotation);
        ally.GetComponent<CharacterFeature>().characterType = CharacterType.StoneCharacter;
        activeAllies.Add(ally);
        ally.tag = "Ally";
    }

    private void InstantiateEnemy()
    {
        float randomZPos = Random.Range(-1, 2);
        var spawnPos = new Vector3(enemyBaseFront.transform.position.x, enemyBaseFront.transform.position.y,
            enemyBaseFront.transform.position.z + randomZPos);
        var enemy = Instantiate(warriors[(int)CharacterType.StickCharacter], spawnPos, warriors[0].transform.rotation);
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