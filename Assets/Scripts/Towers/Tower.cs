using System;
using System.Collections;
using System.Linq;
using Core.Utilities.ObjectPool;
using Core.Utilities.Timer;
using TowerDefense.Levels;
using TowerDefense.Placement;
using TowerDefense.Towers.Launchers;
using UnityEngine;

namespace TowerDefense.Towers
{
    [RequireComponent(typeof(ILauncher)), RequireComponent(typeof(Collider))]
    public class Tower : MonoBehaviour
    {
        [Header("Tower Configuration")] 
        public string towerName;
        [SerializeField] private TowerConfiguration towerConfiguration;
        [SerializeField] private TowerRadiusVisualizer towerRadiusVisualizer;
        
        [Header("Targetting")] 
        [SerializeField] private Targetter targetter;
        [SerializeField] private Transform[] projectilePoints;
        
        public event Action<Tower> TowerPlaced; 
        public event Action<Tower> TowerRemoved; 

        private Timer _fireTimer;
        private ILauncher _launcher;

        private Collider _sellCollider;
        
        public Platform CurrentPlatform { get; set; }
        public Sprite Icon => towerConfiguration.icon;
        public int Cost => towerConfiguration.cost;

        private Coroutine _fireCoroutine;
        
        private void Awake()
        {
            _launcher = GetComponent<ILauncher>();
            _sellCollider = GetComponent<Collider>();
        }

        private void Start()
        {
            towerRadiusVisualizer.DoRenderer(targetter.EffectRadius);
            _sellCollider.enabled = false;
            targetter.gameObject.SetActive(false);
            _fireTimer = new Timer(1 / towerConfiguration.fireRate);
            
            if (string.IsNullOrEmpty(name))
            {
                name = gameObject.name;
            }
        }

        private void OnEnable()
        {
            towerRadiusVisualizer.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (_fireTimer.Tick(Time.deltaTime))
            {
                if (towerConfiguration.fireCondition != null)
                {
                    if (!towerConfiguration.fireCondition.Invoke())
                    {
                        return;
                    }
                }

                FireProjectile();
                _fireTimer.Reset();
            }
        }

        private void FireProjectile()
        {
            if (targetter.CurrentTarget == null)
            {
                return;
            }

            if (towerConfiguration.fireParticles != null && _fireCoroutine == null)
            {
                _fireCoroutine = StartCoroutine(FireParticles());
            }
            
            if (towerConfiguration.multiAttack)
            {
                var enemies = targetter.AllTargets.Take(towerConfiguration.maxEnemies).ToList();
                _launcher.Launch(enemies, towerConfiguration.projectile, projectilePoints);
            }
            else
            {
                _launcher.Launch(targetter.CurrentTarget, towerConfiguration.projectile, projectilePoints);
            }
        }

        private const float Interval = 0.5f;
        private bool _click;
        private float _clickTime;
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (_click && Time.time <= _clickTime + Interval)
                {
                    OnDoubleClick();
                    _click = false;
                }
                else
                {
                    _click = true;
                    _clickTime = Time.time;
                }
            }
        }

        private void OnDoubleClick()
        {
            SellTower();
        }

        private IEnumerator FireParticles()
        {
            ParticleSystem fireParticles = null;
            if (towerConfiguration.fireParticles != null)
            {
                fireParticles = Poolable.TryGetPoolable<ParticleSystem>(towerConfiguration.fireParticles);
                fireParticles.transform.position = projectilePoints[0].transform.position;
            }

            yield return new WaitForSeconds(0.1f);
            if (fireParticles != null)
            {
                Poolable.TryPool(fireParticles.gameObject);
            }

            _fireCoroutine = null;
        }
        
        public void PlaceTower()
        {
            towerRadiusVisualizer.gameObject.SetActive(false);
            targetter.gameObject.SetActive(true);
            _sellCollider.enabled = true;
            TowerPlaced?.Invoke(this);
        }

        public void SellTower()
        {
            if (towerConfiguration.sellParticles != null)
            {
                var sellParticles = Poolable.TryGetPoolable<ParticleSystem>(towerConfiguration.sellParticles);
                sellParticles.transform.position = transform.position;
            }
            
            GameManager.Instance.Currency.AddCurrency(towerConfiguration.sell);
            targetter.gameObject.SetActive(false);
            _sellCollider.enabled = false;
            TowerRemoved?.Invoke(this);
            CurrentPlatform.TowerPlaced = false;
            CurrentPlatform = null;
            targetter.ResetTargetting();
            Poolable.TryPool(gameObject);
        }
    }
}