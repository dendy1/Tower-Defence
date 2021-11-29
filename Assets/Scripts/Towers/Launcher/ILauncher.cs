using System.Collections.Generic;
using TowerDefense.Enemies;
using UnityEngine;

namespace TowerDefense.Towers.Launchers
{
    public interface ILauncher
    {
        /// <summary>
        /// The method for crafting the firing logic for the tower
        /// </summary>
        /// <param name="enemy">
        /// The enemy that the tower is targeting
        /// </param>
        /// <param name="projectile">
        /// The projectile component used to attack the enemy
        /// </param>
        /// <param name="firingPoint"></param>
        void Launch(Enemy enemy, GameObject projectile, Transform firingPoint);

        /// <summary>
        /// The method for crafting the firing logic for the tower
        /// </summary>
        /// <param name="enemy">
        /// The enemy that the tower is targeting
        /// </param>
        /// <param name="projectile">
        /// The projectile component used to attack the enemy
        /// </param>
        /// <param name="firingPoints">
        /// A list of firing points to fire from
        /// </param>
        void Launch(Enemy enemy, GameObject projectile, Transform[] firingPoints);

        /// <summary>
        /// The method for crafting firing logic at multiple enemies
        /// </summary>
        /// <param name="enemies">
        /// The collection of enemies to attack
        /// </param>
        /// <param name="projectile">
        /// The projectile component used to attack the enemy
        /// </param>
        /// <param name="firingPoints"></param>
        void Launch(List<Enemy> enemies, GameObject projectile, Transform[] firingPoints);
    }
}
