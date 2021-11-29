using UnityEngine;

namespace TowerDefense.Towers
{
    [CreateAssetMenu(menuName = "Towers/Tower Configuration")]
    public class TowerConfiguration : ScriptableObject
    {
        [Header("Particles")] 
        public GameObject fireParticles;
        public GameObject buyParticles;
        public GameObject sellParticles;
        
        [Header("Shop Settings")] 
        public Sprite icon;
    
        [Header("Common")]
        public new string name;
        public int cost;
        public int sell;
        
        [Header("Shooting")] 
        public GameObject projectile;
        public float fireRate;
        public Filter fireCondition;
        
        [Header("Multi Attack")]
        public bool multiAttack;
        public int maxEnemies; 
    }
    
    public delegate bool Filter();
}
