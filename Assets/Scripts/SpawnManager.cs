using System.Collections;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] warriors;
    public GameObject allySpawnPoint;
    public GameObject enemySpawnPoint;
    public GameObject allyBase;
    public GameObject enemyBase;
    public float enemySpawnDelay;
    public bool canSpawnEnemy = true;

    [SerializeField] private Button spawnStickWarriorButton;
    [SerializeField] private Button spawnSpearWarriorButton;
    [SerializeField] private Button spawnStoneWarriorButton;

    public readonly List<GameObject> ActiveAllies = new();
    public readonly List<GameObject> ActiveEnemies = new();

    private GameManager _gameManager;
    private ShopManager _shopManager;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (canSpawnEnemy && !_gameManager.GameOver)
        {
            InstantiateEnemy();
            StartCoroutine(SpawnCooldown());
        }
    }

    private void DisableSpawnButtons()
    {
        spawnStickWarriorButton.interactable = false;
        spawnSpearWarriorButton.interactable = false;
        spawnStoneWarriorButton.interactable = false;
    }

    public void InstantiateStickCharacter()
    {
        InstantiateCharacter(CharacterType.StickCharacter);
    }

    public void InstantiateSpearCharacter()
    {
        InstantiateCharacter(CharacterType.SpearCharacter);
    }

    public void InstantiateStoneCharacter()
    {
        InstantiateCharacter(CharacterType.StoneCharacter);
    }

    private void Initialize()
    {
        ActiveAllies.Add(allyBase);
        ActiveEnemies.Add(enemyBase);
        _shopManager = GameObject.FindWithTag("ShopManager").GetComponent<ShopManager>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        InitializeSpawnButtons();
        UpdateSpawnButtonText();
        _gameManager.onGameOver.AddListener(x => DisableSpawnButtons());
    }

    private void InitializeSpawnButtons()
    {
        spawnStickWarriorButton.onClick.AddListener(InstantiateStickCharacter);
        spawnSpearWarriorButton.onClick.AddListener(InstantiateSpearCharacter);
        spawnStoneWarriorButton.onClick.AddListener(InstantiateStoneCharacter);
    }

    private void UpdateSpawnButtonText()
    {
        var stickSpawnPrice = warriors[0].GetComponent<Warrior>().spawnPrice;
        spawnStickWarriorButton.GetComponentInChildren<TMP_Text>().text =
            $"{stickSpawnPrice} ";

        var spearSpawnPrice = warriors[1].GetComponent<Warrior>().spawnPrice;
        spawnSpearWarriorButton.GetComponentInChildren<TMP_Text>().text =
            $"{spearSpawnPrice}";

        var stoneSpawnPrice = warriors[2].GetComponent<Warrior>().spawnPrice;
        spawnStoneWarriorButton.GetComponentInChildren<TMP_Text>().text =
            $"{stoneSpawnPrice}";
    }

    private void InstantiateCharacter(CharacterType type)
    {
        if (_shopManager.CanInstantiate(type) && !_gameManager.GameOver)
        {
            var ally = Instantiate(warriors[(int)type], RandomAllyPosition(), allySpawnPoint.transform.rotation);
            ActiveAllies.Add(ally);
            _shopManager.PayForInstantiate(type);
            ally.tag = "Ally";
            ally.name = "Ally" + ally.name;
        }
    }

    private void InstantiateEnemy()
    {
        var spawnPoint = enemySpawnPoint;
        float randomSpawnLoc = Random.Range(-2, 2);

        var spawnPos = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y,
            spawnPoint.transform.position.z + randomSpawnLoc);
        var enemy = Instantiate(warriors[Random.Range(0, 3)], spawnPos, spawnPoint.transform.rotation);
        ActiveEnemies.Add(enemy);
        enemy.tag = "Enemy";
        enemy.name = "Enemy" + enemy.name;
    }

    private Vector3 RandomAllyPosition()
    {
        var spawnPoint = allySpawnPoint;
        float randomSpawnLoc = Random.Range(-2, 2);
        return new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y,
            spawnPoint.transform.position.z + randomSpawnLoc);
    }

    private IEnumerator SpawnCooldown()
    {
        canSpawnEnemy = false;
        yield return new WaitForSeconds(enemySpawnDelay);
        canSpawnEnemy = true;
    }
}