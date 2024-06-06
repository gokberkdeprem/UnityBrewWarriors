using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public UnityEvent<Castle> OnGameOver;
    public UnityEvent OnGameStart;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _exitButton;

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
        _startButton.SetActive(true);
        _startButton.GetComponentInChildren<Button>().onClick.AddListener(StartGame);
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

    private void StartGame()
    {
        Debug.Log("GameStarted");
        GameOver = false;
        _enemyCastle.onDestroy.AddListener(x =>
        {
            GameOver = true;
            OnGameOver.Invoke(_enemyCastle);
            ShowEndGameComponents(_enemyCastle);
        });
        _allyCastle.onDestroy.AddListener(x =>
        {
            GameOver = true;
            OnGameOver.Invoke(_allyCastle);
            ShowEndGameComponents(_allyCastle);
        });
        OnGameStart.Invoke();
    }
}