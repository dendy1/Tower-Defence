using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    
    [Header("Text Fields")]
    [SerializeField] private Text healthText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text wavesText;
    
    [Header("Game Settings")]
    [SerializeField] private int wavesCount;
    [SerializeField] private int creepsPerWave;
    [SerializeField] private int wavePeriod;
    [SerializeField] private int gold;
    [SerializeField] private float health;

    [SerializeField] private GameObject[] spawners;

    private SpawnerController[] _spawnerControllers;
    private int _creepsCount;
    private int _spawnersReady;
    private int _currentWave;
    private bool _gameover;

    public int CreepsPerWave => creepsPerWave;
    public int Gold => gold;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GameManager is existing!");
            return;
        }
        
        Instance = this;
    }

    private void Start()
    {        
        healthText.text = "HEALTH: " + health;
        wavesText.text = "WAVES: " + wavesCount;
        goldText.text = "GOLD: " + gold;
        
        _spawnerControllers = new SpawnerController[spawners.Length];
        for (int i = 0; i < _spawnerControllers.Length; i++)
        {
            _spawnerControllers[i] = spawners[i].GetComponent<SpawnerController>();
            _spawnerControllers[i].waveisOverEvent.AddListener(OnWaveIsOver);
            _spawnerControllers[i].creepSpawned.AddListener(AddCreep);
            _spawnerControllers[i].StartWave();
        }
    }

    private void AddCreep() => _creepsCount++;
    private void RemoveCreep() => _creepsCount--;
    
    public void OnTowerSelled(int price)
    {
        gold += price / 2;
        goldText.text = "GOLD: " + gold;
    }

    public void OnTowerBuyed(int price)
    {
        gold -= price;
        goldText.text = "GOLD: " + gold;
    }

    public void OnBaseAttacked(float damage)
    {
        health -= damage;
        healthText.text = "HEALTH: " + health;
        
        RemoveCreep();
        
        if (health <= 0)
        {
            GameOver();
        }
    }

    public void OnCreepKilled(int goldGiven)
    {
        gold += goldGiven;
        goldText.text = "GOLD: " + gold;
        RemoveCreep();
    }

    private void GameOver()
    {
        _gameover = true;
        loseScreen.SetActive(true);
    }

    private void GameWin()
    {
        winScreen.SetActive(true);
    }

    IEnumerator CheckForWin()
    {
        while (_creepsCount > 0)
        {
            yield return null;
        }
        
        GameWin();
    }

    private void OnWaveIsOver()
    {
        if (_gameover)
            return;
        
        _spawnersReady++;

        if (_spawnersReady == spawners.Length)
        {
            _spawnersReady = 0;
            
            if (_currentWave < wavesCount)
                StartCoroutine("StartNewWave");
            else
                StartCoroutine("CheckForWin");
        }
    }

    IEnumerator StartNewWave()
    {
        while (_creepsCount > 0)
            yield return null;
        
        _currentWave++;
        wavesText.text = "WAVES: " + (wavesCount - _currentWave);
        
        foreach (var sc in _spawnerControllers)
            sc.SetTimer(wavePeriod);
        
        yield return new WaitForSeconds(wavePeriod);
        
        foreach (var sc in _spawnerControllers)
            sc.StartWave();
    }
    
    public void RestartLevel()
    {        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
