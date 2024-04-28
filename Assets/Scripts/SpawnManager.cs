using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] warriors;
    public GameObject playerBase;
    public GameObject enemyBase;
    public float spawnDelay;
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
        var ally = Instantiate(warriors[0], playerBase.transform.position, warriors[0].transform.rotation);
        ally.tag = "Ally";
        ally.transform.LookAt(enemyBase.transform.position);
    }

    private void InstantiateEnemy()
    {
        var enemy = Instantiate(warriors[0], enemyBase.transform.position, warriors[0].transform.rotation);
        enemy.tag = "Enemy";
        enemy.transform.LookAt(playerBase.transform);
    }

    private IEnumerator SpawnCooldown()
    {
        canSpawnEnemy = false;
        yield return new WaitForSeconds(spawnDelay);
        canSpawnEnemy = true;
    }
}