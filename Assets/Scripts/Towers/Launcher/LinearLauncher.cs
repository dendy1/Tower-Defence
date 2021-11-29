using TowerDefense.Enemies;
using TowerDefense.Projectiles;
using TowerDefense.Towers.Launchers;
using UnityEngine;

public class LinearLauncher : BaseLauncher
{
    public override void Launch(Enemy enemy, GameObject projectile, Transform firingPoint)
    {
        var linearProjectile = projectile.GetComponent<LinearProjectile>();
        if (linearProjectile == null)
        {
            return;
        }
        
        linearProjectile.transform.position = firingPoint.position;
        linearProjectile.FireAtPoint(firingPoint.position, enemy.transform.position);
    }
}
