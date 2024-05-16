using System.Collections;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] warriors;
    public GameObject allyBaseFront;
    public GameObject enemyBaseFront;
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
        var stickSpawnPrice = warriors[0].GetComponent<CharacterFeature>().spawnPrice;
        spawnStickWarriorButton.GetComponentInChildren<TMP_Text>().text =
            $"{stickSpawnPrice} Gold ";

        var spearSpawnPrice = warriors[1].GetComponent<CharacterFeature>().spawnPrice;
        spawnSpearWarriorButton.GetComponentInChildren<TMP_Text>().text =
            $"{spearSpawnPrice} Gold";

        var stoneSpawnPrice = warriors[2].GetComponent<CharacterFeature>().spawnPrice;
        spawnStoneWarriorButton.GetComponentInChildren<TMP_Text>().text =
            $"{stoneSpawnPrice} Gold";
    }

    private void InstantiateCharacter(CharacterType type)
    {
        if (_shopManager.CanInstantiate(type) && !_gameManager.GameOver)
        {
            var ally = Instantiate(warriors[(int)type], RandomAllyPosition(), allyBase.transform.rotation);
            ActiveAllies.Add(ally);
            _shopManager.PayForInstantiate(type);
            ally.tag = "Ally";
        }
    }

    private void InstantiateEnemy()
    {
        float randomZPos = Random.Range(-1, 2);
        var spawnPos = new Vector3(enemyBaseFront.transform.position.x, enemyBaseFront.transform.position.y,
            enemyBaseFront.transform.position.z + randomZPos);
        var enemy = Instantiate(warriors[Random.Range(0, 3)], spawnPos, enemyBase.transform.rotation);
        ActiveEnemies.Add(enemy);
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