using System;
using System.Collections.Generic;
using TowerDefense.Currency;
using TowerDefense.Enemies;
using TowerDefense.Towers.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense.Levels
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("UI Manager")] 
        [SerializeField] private UIManager uiManager;

        [Header("Game Settings")] 
        [SerializeField] private int baseHealth = 100;

        [Header("Currency")] 
        [SerializeField] private int startingCurrency;
        [SerializeField] private CurrencyGainer currencyGainer;

        [Header("Tower Library")] 
        [SerializeField] private TowerLibrary towerLibrary;

        [Header("Wave Spawners")] 
        [SerializeField] private List<Spawner.Spawner> spawners;
        
        public Currency.Currency Currency { get; private set; }
        public TowerLibrary TowerLibrary => towerLibrary;

        protected override void Awake()
        {
            base.Awake();

            Currency = new Currency.Currency(startingCurrency);
            currencyGainer.Initialize(Currency);
        }

        private void Start()
        {
            uiManager.InitializeValues(baseHealth, Currency.CurrentCurrency, _currentWave, _wavesCount);
            Currency.CurrencyChanged += OnCurrencyChanged;
            currencyGainer.CurrencyChanged += OnCurrencyChanged;

            foreach (var spawner in spawners)
            {
                spawner.WaveIsOver += OnSpawnerWaveIsOver;
            }
        }

        private void Update()
        {
            if (CurrentState != GameState.Paused && CurrentState != GameState.GameOver && CurrentState != GameState.Idle)
            {
                currencyGainer.Tick(Time.deltaTime);
            }
            
            if (CurrentState == GameState.GameOver || _spawnersReady != spawners.Count || EnemiesRemaining > 0)
                return;
            
            _spawnersReady = 0;
            WaveIsOver();
        }

        private int _spawnersReady;
        private int _wavesCount, _currentWave;

        public int WavesCount
        {
            set
            {
                if (value > _wavesCount)
                {
                    _wavesCount = value;
                }
            }
        }

        public int EnemiesRemaining { get; set; }

        public int Health
        {
            get => baseHealth;
            set
            {
                var previousHealth = baseHealth;
                var currentHealth = Mathf.Clamp(value, 0, int.MaxValue);
                uiManager.UpdateHealthValue(previousHealth, currentHealth);
                baseHealth = currentHealth;
            }
        }

        public int Wave
        {
            get => _currentWave;
            set
            {
                _currentWave = value;
                uiManager.UpdateWavesValue(_currentWave, _wavesCount);
            }
        }

        private void OnSpawnerWaveIsOver()
        {
            _spawnersReady++;
        }

        private void WaveIsOver()
        {
            SetState(GameState.Idle);
            if (_currentWave == _wavesCount)
            {
                uiManager.SetActiveWinScreen(true);
            }
            else
            {
                uiManager.SetActiveNextWaveButton(true);
                if (uiManager.AutoStart)
                    StartWave();
            }
        }

        public void StartWave()
        {
            SetState(GameState.Wave);
            uiManager.SetActiveNextWaveButton(false);
            foreach (var spawner in spawners)
            {
                spawner.SpawnWave(Wave++);
            }
        }

        public void OnBaseAttacked(Enemy enemy)
        {
            EnemiesRemaining--;
            Health -= enemy.Damage;
            if (baseHealth <= 0)
            {
                SetState(GameState.GameOver);
                uiManager.SetActiveLoseScreen(true);
            }
        }

        public void OnEnemyKilled(Enemy enemy)
        {
            EnemiesRemaining--;
            Currency.AddCurrency(enemy.Currency);
        }

        public void Menu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnCurrencyChanged(CurrencyChangeInfo info)
        {
            uiManager.UpdateCurrencyValue(info.previousCurrency, info.currentCurrency);
        }
        
        public enum GameState
        {
            Idle,
            Wave,
            Paused,
            GameOver
        }
        public event Action<GameState, GameState> stateChanged;
        public GameState CurrentState { get; private set; }
        public void SetState(GameState newState)
        {
            if (CurrentState == newState)
            {
                return;
            }
            var oldState = CurrentState;
            if (oldState == GameState.Paused || oldState == GameState.GameOver)
            {
                Time.timeScale = 1f;
            }

            switch (newState)
            {
                case GameState.Idle:
                    break;
                case GameState.Wave:
                    break;
                case GameState.Paused:
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("newState", newState, null);
            }
            CurrentState = newState;
            stateChanged?.Invoke(oldState, CurrentState);
        }
    }
}
