using TowerDefense.Enemies;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Area Damage Over Time Effect")]
public class ScriptableAreaDamageOverTimeEffect : ScriptableEffect
{
    [Min(0f)]
    public float damageOverTime;

    [Range(0f, 10f)]
    public float areaSize;

    public int maxEnemies;
    
    public override TimedEffect InitializeBuff(Enemy enemy)
    {
        return new TimedAreaDamageOverTimeEffect(this, enemy);
    }
}
