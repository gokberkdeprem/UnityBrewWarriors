using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<Castle> onGameOver;

    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject defeatText;

    [SerializeField] public bool GameOver;
    private Castle _allyCastle;
    private Castle _enemyCastle;

    // Start is called before the first frame update
    private void Start()
    {
        _allyCastle = GameObject.FindWithTag("AllyBase").GetComponent<Castle>();
        _enemyCastle = GameObject.FindWithTag("EnemyBase").GetComponent<Castle>();

        _enemyCastle.onDestroy.AddListener(x =>
        {
            GameOver = true;
            onGameOver.Invoke(_enemyCastle);
            ShowEndGameComponents(_enemyCastle);
        });
        _allyCastle.onDestroy.AddListener(x =>
        {
            GameOver = true;
            onGameOver.Invoke(_allyCastle);
            ShowEndGameComponents(_allyCastle);
        });
    }

    private void ShowEndGameComponents(Castle defeatedCastle)
    {
        if (defeatedCastle.isEnemy)
        {
            victoryText.SetActive(true);
            victoryText.GetComponentInChildren<TMP_Text>().text =
                $"Victory \n {defeatedCastle.destroyReward}";
        }
        else
        {
            defeatText.SetActive(true);
            defeatText.GetComponentInChildren<TMP_Text>().text =
                $"Defeat \n {defeatedCastle.destroyReward}";
        }
    }
}