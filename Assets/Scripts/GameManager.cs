using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<BaseFeature> onGameOver;

    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject defeatText;
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
            ShowEndGameComponents(_enemyBaseFeature);
        });
        _allyBaseFeature.onBaseDeath.AddListener(x =>
        {
            GameOver = true;
            onGameOver.Invoke(_allyBaseFeature);
            ShowEndGameComponents(_allyBaseFeature);
        });
    }

    private void ShowEndGameComponents(BaseFeature defeatedBaseFeature)
    {
        if (defeatedBaseFeature.isEnemy)
        {
            victoryText.SetActive(true);
            victoryText.GetComponentInChildren<TMP_Text>().text =
                $"Victory \n + {defeatedBaseFeature.destroyReward} GOLD";
        }
        else
        {
            defeatText.SetActive(true);
            defeatText.GetComponentInChildren<TMP_Text>().text =
                $"Defeat \n + {defeatedBaseFeature.destroyReward} GOLD";
        }
    }
}