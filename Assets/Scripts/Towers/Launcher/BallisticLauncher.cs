using TowerDefense.Enemies;
using UnityEngine;

namespace TowerDefense.Towers.Launchers
{
    public class BallisticLauncher : BaseLauncher
    {
        public override void Launch(Enemy enemy, GameObject projectile, Transform firingPoint)
        {
            // var startPosition = firingPoint.position;
            // var ballisticProjectile = projectile.GetComponent<BallisticProjectile>();
            //
            // if (ballisticProjectile == null)
            // {
            //     Debug.LogError("No ballistic projectile attached to projectile");
            //     DestroyImmediate(projectile);
            //     return;
            // }
            // Vector3 targetPoint;
            // if (ballisticProjectile.fireMode == BallisticFireMode.UseLaunchSpeed)
            // {
            //     // use speed
            //     targetPoint = Ballistics.CalculateBallisticLeadingTargetPointWithSpeed(
            //         startPosition,
            //         enemy.position, enemy.velocity,
            //         ballisticProjectile.startSpeed, ballisticProjectile.arcPreference, Physics.gravity.y, 4);
            // }
            // else
            // {
            //     // use angle
            //     targetPoint = Ballistics.CalculateBallisticLeadingTargetPointWithAngle(
            //         startPosition,
            //         enemy.position, enemy.velocity, ballisticProjectile.firingAngle,
            //         ballisticProjectile.arcPreference, Physics.gravity.y, 4);
            // }
            // ballisticProjectile.FireAtPoint(startPosition, targetPoint);
            // ballisticProjectile.IgnoreCollision(LevelManager.instance.environmentColliders);
            // PlayParticles(fireParticleSystem, startPosition, targetPoint);
        }
    }

}