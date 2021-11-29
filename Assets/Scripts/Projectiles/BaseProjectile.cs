using System;
using System.Collections;
using System.Collections.Generic;
using Core.Utilities.ObjectPool;
using TowerDefense.Enemies;
using UnityEngine;

namespace TowerDefense.Projectiles
{
    public abstract class BaseProjectile : MonoBehaviour, IProjectile
    {
        public float liveTime = 1f;
        public List<ScriptableEffect> effects;
        public event Action fired;
        
        protected bool IsFired;
        protected Rigidbody Rigidbody;
        
        private LayerMask _enemyLayer;

        protected virtual void Awake()
        {
            _enemyLayer = LayerMask.NameToLayer("Enemy");
        }

        private void OnEnable()
        {
            StartCoroutine(DisableOnTime());
        }

        public abstract void FireAtPoint(Vector3 startPoint, Vector3 targetPoint);
        public abstract void FireInDirection(Vector3 startPoint, Vector3 fireVector);
        public abstract void FireAtVelocity(Vector3 startPoint, Vector3 fireVelocity);
        
        protected virtual void Fire(Vector3 firingVector)
        {
            IsFired = true;
            transform.rotation = Quaternion.LookRotation(firingVector);
            Rigidbody.velocity = firingVector;
            fired?.Invoke();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _enemyLayer)
            {
                return;
            }

            var enemy = other.GetComponent<Enemy>();
            foreach (var effect in effects)
            {
                enemy.AddEffect(effect.InitializeBuff(enemy));
            }
            
            Poolable.TryPool(gameObject);
        }

        private IEnumerator DisableOnTime()
        {
            yield return new WaitForSeconds(liveTime);
            Poolable.TryPool(gameObject);
        }
    }
}