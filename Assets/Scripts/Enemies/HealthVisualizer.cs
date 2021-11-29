using System;
using DG.Tweening;
using UnityEngine;

namespace TowerDefense.Enemies
{
    public class HealthVisualizer : MonoBehaviour
    {
        public Enemy enemy;
        public Transform healthBar;
        public Transform backgroundBar;
        public bool showWhenFull;
        
        private Transform _cameraToFace;

        private void Awake()
        {
            _cameraToFace = Camera.main.transform;
        }

        private void Start()
        {
            enemy.HealthChanged += OnHealthChanged;
        }

        private void Update()
        {
            var direction = _cameraToFace.transform.forward;
            transform.forward = -direction;
        }
        
        private void OnHealthChanged(HealthChangeInfo healthChangeInfo)
        {
            UpdateHealth(enemy.NormalizedHealth);
        }

        public void UpdateHealth(float normalizedHealth)
        {
            var scale = Vector3.one;

            if (healthBar != null)
            {
                scale.x = normalizedHealth;
                healthBar.transform.DOScale(scale, 0.25f);
            }

            if (backgroundBar != null)
            {
                scale.x = 1 - normalizedHealth;
                backgroundBar.transform.DOScale(scale, 0.25f);
            }

            SetVisible(showWhenFull && Math.Abs(normalizedHealth - 1.0f) < 0.05f || normalizedHealth < 1.0f && normalizedHealth > 0.0f);
        }
        
        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
