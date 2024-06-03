using System.Linq;
using Enums;
using UnityEngine;

public class Warrior : BattleEntity
{
    [SerializeField] public float power;
    [SerializeField] public float attackRate;
    [SerializeField] public float speed;
    [SerializeField] public float spawnRate;
    [SerializeField] public int spawnPrice;
    [SerializeField] public int purchasePrice;
    [SerializeField] public int upgradePrice;
    public GameObject Target;
    public BattleEntity TargetBattleEntity;
    private Animator _animator;
    private SpawnManager _spawnManager;
    private GameObject _spawnManagerGameObject;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        SelectTarget();
        _spawnManagerGameObject = GameObject.FindWithTag("SpawnManager");
        _spawnManager = _spawnManagerGameObject.GetComponent<SpawnManager>();
        _spawnManager.OnWarriorSpawn.AddListener(SelectTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        var isAllyLayer = other.gameObject.layer == LayerMask.NameToLayer("Ally");
        var isEnemyLayer = other.gameObject.layer == LayerMask.NameToLayer("Enemy");

        if ((isEnemy && isAllyLayer) || (!isEnemy && isEnemyLayer)) SelectTarget(other.gameObject);
    }

    public override void GetDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            onDestroy.Invoke(gameObject);
            GetComponent<Collider>().enabled = false;
            _animator.CrossFade("Death", 0, 0);
            Destroy(gameObject, 3);
        }
    }

    public void SelectTarget(GameObject target = null)
    {
        if (target != null)
        {
            Target = target;
            TargetBattleEntity = target.GetComponent<BattleEntity>();
        }
        else
        {
            var targetWarriors = isEnemy ? SpawnManager.ActiveAllies : SpawnManager.ActiveEnemies;
            var anyWarrior = targetWarriors.Any(warrior =>
                warrior.GetComponent<BattleEntity>().EntityType == EntityType.Warrior);

            target = targetWarriors.First(warrior =>
                warrior.GetComponent<BattleEntity>().EntityType ==
                (anyWarrior ? EntityType.Warrior : EntityType.Castle));

            Target = target;
            TargetBattleEntity = target.GetComponent<BattleEntity>();
        }
    }
}