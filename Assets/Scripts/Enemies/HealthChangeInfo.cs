using UnityEngine;

namespace TowerDefense.Enemies
{
    public class HealthChangeInfo
    {
        public Enemy Enemy;
        public int OldHealth;
        public int NewHealth;

        public HealthChangeInfo(Enemy enemy, int oldHealth, int newHealth)
        {
            Enemy = enemy;
            OldHealth = oldHealth;
            NewHealth = newHealth;
        }

        public int HealthDifference => NewHealth - OldHealth;
        public int AbsHealthDifference => Mathf.Abs(HealthDifference);
    }
}
