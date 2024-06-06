using System.Linq;
using Enums;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : BattleEntity
{
    [SerializeField] public float power;
    [SerializeField] public float attackRate;
    [SerializeField] public float speed;
    [SerializeField] public float spawnRate;
    [SerializeField] public int spawnPrice;
    [SerializeField] public int purchasePrice;
    [SerializeField] public int upgradePrice;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private ParticleSystem _deathParticle;
    public GameObject Target;
    public BattleEntity TargetBattleEntity;
    private Animator _animator;
    private SpawnManager _spawnManager;
    private GameObject _spawnManagerGameObject;


    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _spawnManagerGameObject = GameObject.FindWithTag("SpawnManager");
        _spawnManager = _spawnManagerGameObject.GetComponent<SpawnManager>();
        _spawnManager.OnWarriorSpawn.AddListener(x => SelectTarget());
        SelectTarget();
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     var isAllyLayer = other.gameObject.layer == LayerMask.NameToLayer("Ally");
    //     var isEnemyLayer = other.gameObject.layer == LayerMask.NameToLayer("Enemy");
    //
    //     if ((isEnemy && isAllyLayer) || (!isEnemy && isEnemyLayer)) SelectTarget(other.gameObject);
    // }

    public override void GetDamage(float damage, GameObject attacker)
    {
        if (Target != attacker)
            SelectTarget(attacker);

        currentHealth -= damage;

        UpdateHealthBar();

        if (currentHealth == 0)
        {
            if (isEnemy)
                _spawnManager.ActiveEnemies.Remove(gameObject);
            else
                _spawnManager.ActiveAllies.Remove(gameObject);

            onDestroy.Invoke(gameObject);
            gameObject.layer = LayerMask.NameToLayer("Default");
            healthBar.SetActive(false);
            GetComponent<Collider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponentInChildren<NavMeshObstacle>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            _animator.CrossFadeInFixedTime("Death", 0.1f, 0);
            Instantiate(_deathParticle, gameObject.transform.position, _deathParticle.transform.rotation);
            Destroy(gameObject, 2);
        }
    }

    public BattleEntity SelectTarget(GameObject target = null)
    {
        if (SpawnManager.ActiveAllies.Count == 0 || SpawnManager.ActiveEnemies.Count == 0)
            return null;

        if (target != null)
        {
            Target = target;
            TargetBattleEntity = target.GetComponent<BattleEntity>();
            return TargetBattleEntity;
        }

        var targetWarriors = isEnemy ? SpawnManager.ActiveAllies : SpawnManager.ActiveEnemies;
        var anyWarrior = targetWarriors.Any(warrior =>
            warrior.GetComponent<BattleEntity>().EntityType == EntityType.Warrior);

        target = targetWarriors.First(warrior =>
            warrior.GetComponent<BattleEntity>().EntityType ==
            (anyWarrior ? EntityType.Warrior : EntityType.Castle));

        Target = target;
        TargetBattleEntity = target.GetComponent<BattleEntity>();

        TargetBattleEntity.onDestroy.AddListener(x =>
        {
            while (SelectTarget().currentHealth <= 0 && !GameManager.GameOver) SelectTarget();
        });
        return TargetBattleEntity;
    }
}