using System.Collections;
using DG.Tweening;
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
    
    [Header("Another")]
    [SerializeField] private float textAnimationTime;

    [SerializeField] private GameObject[] spawners;

    private SpawnerController[] _spawnerControllers;
    private int _creepsCount;
    private int _spawnersReady;
    private int _currentWave;
    private bool _gameover;

    private string _wavesPrefix = "WAVES REMAINIG: ";
    private string _goldPrefix = "GOLD: ";
    private string _healthPrefix = "HEALTH: ";

    public int CreepsPerWave => creepsPerWave;

    public int Gold
    {
        get { return gold; }
        set
        {
            var delta = value - gold;
            
            gold = value;
    
            goldText.DOKill();
            float deltaTime = 0f;
            goldText.DOFade(1f, textAnimationTime).OnUpdate(() =>
            {
                deltaTime += Time.deltaTime;
                var newTime = delta * (1 - deltaTime / textAnimationTime);

                SetText(gold - delta + (delta - newTime), goldText, _goldPrefix);
            }).OnComplete(() => { SetText(gold, goldText, _goldPrefix); });
        }
    }

    public float Health
    {
        get => health;
        set
        {
            var delta = value - health;
            if (value <= 0)
            {
                health = 0;
                return;
            }
            
            health = value;
            
            healthText.DOKill();
            float deltaTime = 0f;
            
            healthText.DOFade(1f, textAnimationTime).OnUpdate(() =>
            {
                deltaTime += Time.deltaTime;
                var newTime = delta * (1 - deltaTime / textAnimationTime);

                SetText(health - delta + (delta - newTime), healthText, _healthPrefix);
            }).OnComplete(() => { SetText(health, healthText, _healthPrefix); });
        }
    }

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
        healthText.text = _healthPrefix + health;
        wavesText.text = _wavesPrefix + wavesCount;
        goldText.text = _goldPrefix + gold;
        
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
        Gold += price / 2;
    }

    public void OnTowerBuyed(int price)
    {
        Gold -= price;
    }

    public void OnBaseAttacked(float damage)
    {
        Health -= damage;
        RemoveCreep();
        if (health <= 0)
        {
            GameOver();
        }
    }

    public void OnCreepKilled(int goldGiven)
    {
        Gold += goldGiven;
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

    private void SetText(float value, Text text, string prefix = "")
    {
        text.text = string.Format("{0}{1}", prefix, Mathf.Round(value));
    }
}
