using UnityEngine;

namespace TowerDefense.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Enemy Configuration")]
    public class EnemyConfiguration : ScriptableObject
    {
        [Header("Particles")] 
        public GameObject deathParticles;
        public GameObject hitParticles;
        public GameObject destinationReachedParticles;
        
        [Header("Enemy Settings")]
        public int health = 100;
        public int damage = 5;
        public int currency = 10;
        public float speed = 1f;
    }
}
