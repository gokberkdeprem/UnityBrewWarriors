using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] warriors;
    public GameObject playerBase;
    public GameObject enemyBase;
    public float enemySpawnDelay;
    public bool canSpawnEnemy = true;

    // Start is called before the first frame update
    private void Start()
    {
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

    public void InstantiateAlly()
    {
        float randomZPos = Random.Range(-1, 2);
        var spawnPos = new Vector3(playerBase.transform.position.x, playerBase.transform.position.y,
            playerBase.transform.position.z + randomZPos);
        var ally = Instantiate(warriors[0], spawnPos, warriors[0].transform.rotation);
        ally.tag = "Ally";
    }

    private void InstantiateEnemy()
    {
        float randomZPos = Random.Range(-1, 2);
        var spawnPos = new Vector3(enemyBase.transform.position.x, enemyBase.transform.position.y,
            enemyBase.transform.position.z + randomZPos);
        var enemy = Instantiate(warriors[0], spawnPos, warriors[0].transform.rotation);
        enemy.tag = "Enemy";
    }

    private IEnumerator SpawnCooldown()
    {
        canSpawnEnemy = false;
        yield return new WaitForSeconds(enemySpawnDelay);
        canSpawnEnemy = true;
    }
}