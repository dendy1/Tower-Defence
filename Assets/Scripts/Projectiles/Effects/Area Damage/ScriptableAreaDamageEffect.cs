using TowerDefense.Enemies;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Area Instant Damage Effect")]
public class ScriptableAreaDamageEffect : ScriptableEffect
{
    [Min(0)]
    public int damage;

    [Range(0f, 10f)]
    public float areaSize;

    public int maxEnemies;
    
    public override TimedEffect InitializeBuff(Enemy enemy)
    {
        return new TimedAreaDamageEffect(this, enemy);
    }
}
