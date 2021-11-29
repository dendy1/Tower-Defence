using System;
using System.Collections.Generic;
using Core.Utilities.Timer;
using TowerDefense.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefense.Towers
{
    public class Targetter : MonoBehaviour
    {
        public event Action<Enemy> targetEntersRange;
        public event Action<Enemy> targetExitsRange;
        public event Action<Enemy> acquiredTarget;
        public event Action lostTarget;

        [Header("Targetting")] 
        public Transform turret;
        public Vector2 turretXRotationRange = new Vector2(0, 359);
        public bool onlyYTurretRotation = true;
        public float searchWaitTime = 0.5f;
        public Filter searchCondition;
        public float idleRotationSpeed = 39f;
        public float idleCorrectionTime = 2.0f;
        public Collider attachedCollider;
        public float idleWaitTime = 2.0f;

        private int _enemyLayer;
        private float _currentRotationSpeed, _xRotationCorrectionTime;
        private List<Enemy> _targetsInRange = new List<Enemy>();
        private Enemy _currentTarget;
        private Timer _searchTimer, _idleTimer;
        
        public List<Enemy> AllTargets => _targetsInRange;
        public Enemy CurrentTarget => _currentTarget;

        private void Awake()
        {
            _enemyLayer = LayerMask.NameToLayer("Enemy");
        }

        private void Start()
        {
            _searchTimer = new Timer(searchWaitTime);
            _idleTimer = new Timer(idleWaitTime);
        }

        private void Update()
        {
            if (_searchTimer.Tick(Time.deltaTime) && _currentTarget == null && _targetsInRange.Count > 0)
            {
                _currentTarget = GetNearestEnemy();
                if (_currentTarget != null)
                {
                    acquiredTarget?.Invoke(_currentTarget);
                    _searchTimer.Reset();
                }
            }

            AimTurret();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (LayerManager.Instance.enemyLayerMask != (LayerManager.Instance.enemyLayerMask | (1 << other.gameObject.layer)))
            {
                return;
            }

            var enemy = other.GetComponent<Enemy>();
            enemy.Removed += OnTargetRemoved;
            _targetsInRange.Add(enemy);
            targetEntersRange?.Invoke(enemy);
        }

        private void OnTriggerExit(Collider other)
        {
            if (LayerManager.Instance.enemyLayerMask != (LayerManager.Instance.enemyLayerMask | (1 << other.gameObject.layer)))
            {
                return;
            }

            var enemy = other.GetComponent<Enemy>();
            _targetsInRange.Remove(enemy);
            targetExitsRange?.Invoke(enemy);

            if (enemy == _currentTarget)
            {
                OnTargetRemoved(enemy);
            }
            else
            {
                enemy.Removed -= OnTargetRemoved;
            }
        }

        private Enemy GetNearestEnemy()
        {
            var length = _targetsInRange.Count;
            if (length == 0)
            {
                return null;
            }

            Enemy nearest = null;
            var distance = float.MaxValue;
            for (var i = length - 1; i >= 0; i--)
            {
                var potentialTarget = _targetsInRange[i];
                if (potentialTarget == null || potentialTarget.IsDead)
                {
                    _targetsInRange.RemoveAt(i);
                    continue;
                }
                
                var currentDistance = Vector3.Distance(transform.position, potentialTarget.transform.position);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    nearest = potentialTarget;
                }
            }

            return nearest;
        }

        private void OnTargetRemoved(Enemy target)
        {
            target.Removed -= OnTargetRemoved;

            if (_currentTarget != null && target == _currentTarget)
            {
                lostTarget?.Invoke();
                _targetsInRange.Remove(_currentTarget);
                _currentTarget = null;
                _xRotationCorrectionTime = 0.0f;
            }
            else
            {
                for (var i = 0; i < _targetsInRange.Count; i++)
                {
                    if (_targetsInRange[i] != target)
                        continue;

                    _targetsInRange.RemoveAt(i);
                    break;
                }
            }
        }
        
        private void AimTurret()
        {
            if (turret == null) 
                return;
            
            if (_currentTarget == null)
            {
                if (_idleTimer.Tick(Time.deltaTime))
                {
                    _currentRotationSpeed = (Random.value * 2 - 1) * idleRotationSpeed;
                
                    var euler = turret.rotation.eulerAngles;
                    euler.x = Mathf.Lerp(Wrap180(euler.x), 0, _xRotationCorrectionTime);
                    _xRotationCorrectionTime = Mathf.Clamp01((_xRotationCorrectionTime + Time.deltaTime) / idleCorrectionTime);
                    euler.y += _currentRotationSpeed * Time.deltaTime;
                }
                else
                {
                    var euler = turret.rotation.eulerAngles;
                    euler.x = Mathf.Lerp(Wrap180(euler.x), 0, _xRotationCorrectionTime);
                    _xRotationCorrectionTime = Mathf.Clamp01((_xRotationCorrectionTime + Time.deltaTime) / idleCorrectionTime);
                    euler.y += _currentRotationSpeed * Time.deltaTime;

                    turret.eulerAngles = euler;
                }
            }
            else
            {
                _idleTimer.Reset();
                
                var targetPosition = _currentTarget.transform.position;
                if (onlyYTurretRotation)
                {
                    targetPosition.y = turret.position.y;
                }
                
                var direction = targetPosition - turret.position;
                var look = Quaternion.LookRotation(direction, Vector3.up);
                var lookEuler = look.eulerAngles;
                
                // We need to convert the rotation to a -180/180 wrap so that we can clamp the angle with a min/max
                var x = Wrap180(lookEuler.x);
                lookEuler.x = Mathf.Clamp(x, turretXRotationRange.x, turretXRotationRange.y);
                look.eulerAngles = lookEuler;
                turret.rotation = look;
            }
        }
        
        private static float Wrap180(float angle)
        {
            angle %= 360;
            if (angle < -180)
            {
                angle += 360;
            }
            else if (angle > 180)
            {
                angle -= 360;
            }
            return angle;
        }

        public void ResetTargetting()
        {
            _targetsInRange.Clear();
            _currentTarget = null;

            targetEntersRange = null;
            targetExitsRange = null;
            acquiredTarget = null;
            lostTarget = null;

            if (turret != null)
            {
                turret.localRotation = Quaternion.identity;
            }
        }
        
        public float EffectRadius
        {
            get
            {
                var sphere = attachedCollider as SphereCollider;
                if (sphere != null)
                {
                    return sphere.radius;
                }
                var capsule = attachedCollider as CapsuleCollider;
                if (capsule != null)
                {
                    return capsule.radius;
                }
                return 0;
            }
        }
    }
}