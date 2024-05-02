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

    [SerializeField] private Button _spawnStickWarriorButton;
    [SerializeField] private Button _spawnSpearWarriorButton;
    [SerializeField] private Button _spawnStoneWarriorButton;

    public readonly List<GameObject> activeAllies = new();
    public readonly List<GameObject> activeEnemies = new();

    private ShopManager _shopManager;

    private void Start()
    {
        Initialize();
    }

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
        activeAllies.Add(allyBase);
        activeEnemies.Add(enemyBase);
        _shopManager = GameObject.FindWithTag("ShopManager").GetComponent<ShopManager>();
        InitializeSpawnButtons();
        UpdateSpawnButtonText();
    }

    private void InitializeSpawnButtons()
    {
        _spawnStickWarriorButton.onClick.AddListener(InstantiateStickCharacter);
        _spawnSpearWarriorButton.onClick.AddListener(InstantiateSpearCharacter);
        _spawnStoneWarriorButton.onClick.AddListener(InstantiateStoneCharacter);
    }

    private void UpdateSpawnButtonText()
    {
        var stickSpawnPrice = warriors[0].GetComponent<CharacterFeature>().spawnPrice;
        _spawnStickWarriorButton.GetComponentInChildren<TMP_Text>().text =
            $"Spawn \nStick Warrior \n({stickSpawnPrice} Gold) ";

        var spearSpawnPrice = warriors[1].GetComponent<CharacterFeature>().spawnPrice;
        _spawnSpearWarriorButton.GetComponentInChildren<TMP_Text>().text =
            $"Spawn \nSpear Warrior \n({spearSpawnPrice} Gold)";

        var stoneSpawnPrice = warriors[2].GetComponent<CharacterFeature>().spawnPrice;
        _spawnStoneWarriorButton.GetComponentInChildren<TMP_Text>().text =
            $"Spawn \nStone Warrior \n({stoneSpawnPrice} Gold)";
    }

    private void InstantiateCharacter(CharacterType type)
    {
        if (_shopManager.CanInstantiate(type))
        {
            var ally = Instantiate(warriors[(int)type], RandomAllyPosition(), warriors[0].transform.rotation);
            activeAllies.Add(ally);
            _shopManager.PayForInstantiate(type);
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