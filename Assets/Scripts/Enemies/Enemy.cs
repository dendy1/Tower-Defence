using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities.ObjectPool;
using DG.Tweening;
using TowerDefense.Levels;
using UnityEngine;

namespace TowerDefense.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyConfiguration enemyConfiguration;
        [SerializeField] private Animator animator;
        
        public event Action<Enemy> DestinationReached;
        public event Action<Enemy> Removed;
        public event Action<Enemy> Died;
        public event Action<HealthChangeInfo> HealthChanged;
        
        private int _currentHealth, _currentDamage, _currentCurrency;
        private float _currentSpeed;

        private Queue<Vector3> _waypointsQueue = new Queue<Vector3>();
        private Vector3? _currentWaypoint;   
        
        private readonly Dictionary<ScriptableEffect, TimedEffect> _effects = new Dictionary<ScriptableEffect, TimedEffect>();
        private static readonly int DeadHash = Animator.StringToHash("dead");
        private static readonly int HitHash = Animator.StringToHash("hit");

        private Coroutine _hitCoroutine;

        public int Health
        {
            get => _currentHealth;
            set
            {
                if (_currentHealth <= 0)
                    return;
                
                var oldHealth = _currentHealth;
                _currentHealth = value;
                HealthChanged?.Invoke(new HealthChangeInfo(this, oldHealth, _currentHealth));

                if (_currentHealth <= 0)
                {
                   StartCoroutine(Die());
                }
            }
        }

        public float NormalizedHealth
        {
            get
            {
                var maxHealth = (float)enemyConfiguration.health;
                if (Math.Abs(enemyConfiguration.health) <= Mathf.Epsilon)
                {
                    maxHealth = 1f;
                }
                return Mathf.Clamp01(_currentHealth / maxHealth);
            }
        }
        
        public int Damage
        {
            get => _currentDamage;
            set => _currentDamage = Mathf.Clamp(value, 0, int.MaxValue);
        }
        
        public int Currency
        {
            get => _currentCurrency;
            set => _currentCurrency = Mathf.Clamp(value, 0, int.MaxValue);
        }
        
        public float Speed
        {
            get => _currentSpeed;
            set => _currentSpeed = Mathf.Clamp(value, 0, float.MaxValue);
        }
       
        public bool IsDead => _currentHealth <= 0;

        public void InitializeEnemy(Vector3[] waypoints)
        {
            _currentHealth = enemyConfiguration.health;
            _currentSpeed = enemyConfiguration.speed;
            _currentDamage = enemyConfiguration.damage;
            _currentCurrency = enemyConfiguration.currency;
            _currentWaypoint = null;

            _waypointsQueue.Clear();
            for (var i = 0; i < waypoints.Length; i++)
            {   
                _waypointsQueue.Enqueue(waypoints[i]);
            }
            
            Died += GameManager.Instance.OnEnemyKilled;
            DestinationReached += GameManager.Instance.OnBaseAttacked;
        }

        public void RemoveEnemy()
        {
            Died = null;
            DestinationReached = null;
            Poolable.TryPool(gameObject);
        }

        public void AddEffect(TimedEffect newEffect)
        {
            if (_effects.TryGetValue(newEffect.Effect, out var effect))
            {
                effect.Activate();
            }
            else
            {
                _effects.Add(newEffect.Effect, newEffect);
                newEffect.Activate();
            }
        }

        private void Start()
        {
            HealthChanged += info =>
            {
                if (_hitCoroutine == null)
                {
                    StartCoroutine(Hit());
                }
            };
        }

        private void Update()
        {
            foreach (var effect in _effects.Values.ToList())
            {
                effect.Tick(Time.deltaTime);
                if (effect.IsFinished)
                {
                    _effects.Remove(effect.Effect);
                }
            }
            
            if (!IsDead)
                MoveToWaypoint();
        }

        private void MoveToWaypoint()
        {
            if (_currentWaypoint == null)
            {
                if (_waypointsQueue.Count == 0)
                {
                    DestinationReached?.Invoke(this);
                    Removed?.Invoke(this);
                    Poolable.TryPool(gameObject);
                    return;
                }

                _currentWaypoint = _waypointsQueue.Dequeue();
                transform.DOLookAt(_currentWaypoint.Value, 0.25f);
            }

            var direction = _currentWaypoint.Value - transform.position;
            transform.Translate(direction.normalized * _currentSpeed * Time.deltaTime, Space.World);

            if (Vector3.Distance(_currentWaypoint.Value, transform.position) <= 0.01f)
            {
                _currentWaypoint = null;
            }
        }

        private IEnumerator Hit()
        {
            ParticleSystem hitParticles = null;
            if (enemyConfiguration.hitParticles != null)
            {
                hitParticles = Poolable.TryGetPoolable<ParticleSystem>(enemyConfiguration.hitParticles);
                hitParticles.transform.position = transform.position;
            }
            
            // animator.SetBool(HitHash, true);
            // while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            // {
            //     yield return null;
            // }
            yield return new WaitForSeconds(0.25f);
            if (hitParticles != null)
            {
                Poolable.TryPool(hitParticles.gameObject);
            }

            _hitCoroutine = null;
        }
        
        private IEnumerator Die()
        {
            ParticleSystem deathParticles = null;
            if (enemyConfiguration.deathParticles != null)
            {
                deathParticles = Poolable.TryGetPoolable<ParticleSystem>(enemyConfiguration.deathParticles);
                deathParticles.transform.position = transform.position;
            }
            
            Died?.Invoke(this);
            Removed?.Invoke(this);
            
            animator.SetBool(DeadHash, true);
            
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }

            if (deathParticles != null)
            {
                Poolable.TryPool(deathParticles.gameObject);
            }
            
            yield return new WaitForSeconds(0.25f);
            RemoveEnemy();
        }
    }
}