using System.Collections.Generic;
using Core.Utilities.ObjectPool;
using TowerDefense.Enemies;
using UnityEngine;

namespace TowerDefense.Towers.Launchers
{
    public abstract class BaseLauncher : MonoBehaviour, ILauncher
    {
        public abstract void Launch(Enemy enemy, GameObject projectile, Transform firingPoint);

        public void Launch(Enemy enemy, GameObject projectile, Transform[] firingPoints)
        {
            var poolable = Poolable.TryGetPoolable<Poolable>(projectile);
            if (poolable == null)
            {
                return;
            }

            Launch(enemy, poolable.gameObject, GetRandomTransform(firingPoints));
        }

        public void Launch(List<Enemy> enemies, GameObject projectile, Transform[] firingPoints)
        {
            var enemiesCount = enemies.Count;
            var currentFiringPointIndex = 0;
            var firingPointLength = firingPoints.Length;
            for (var i = 0; i < enemiesCount; i++)
            {
                var enemy = enemies[i];
                var firingPoint = firingPoints[currentFiringPointIndex];
                currentFiringPointIndex = (currentFiringPointIndex + 1) % firingPointLength;
                var poolable = Poolable.TryGetPoolable<Poolable>(projectile);
                if (poolable == null)
                {
                    return;
                }

                Launch(enemy, poolable.gameObject, firingPoint);
            }
        }

        public Transform GetRandomTransform(Transform[] launchPoints)
        {
            var index = Random.Range(0, launchPoints.Length);
            return launchPoints[index];
        }
    }
}
