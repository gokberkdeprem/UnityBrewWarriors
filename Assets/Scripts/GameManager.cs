using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<BaseFeature> onGameOver;
    private BaseFeature _allyBaseFeature;
    private BaseFeature _enemyBaseFeature;
    public bool GameOver { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        _allyBaseFeature = GameObject.FindWithTag("AllyBase").GetComponent<BaseFeature>();
        _enemyBaseFeature = GameObject.FindWithTag("EnemyBase").GetComponent<BaseFeature>();

        _enemyBaseFeature.onBaseDeath.AddListener(x =>
        {
            GameOver = true;
            onGameOver.Invoke(_enemyBaseFeature);
        });
        _allyBaseFeature.onBaseDeath.AddListener(x =>
        {
            GameOver = true;
            onGameOver.Invoke(_allyBaseFeature);
        });
    }
}