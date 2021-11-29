using TowerDefense.Enemies;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Speed Effect")]
public class ScriptableSpeedEffect : ScriptableEffect
{
    public float SpeedChange;
    
    public override TimedEffect InitializeBuff(Enemy enemy)
    {
        return new TimedSpeedEffect(this, enemy);
    }
}
