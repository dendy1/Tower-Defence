using TowerDefense.Enemies;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Damage Over Time Effect")]
public class ScriptableDamageOverTimeEffect : ScriptableEffect
{
    public int DamageOverTime;
    
    public override TimedEffect InitializeBuff(Enemy enemy)
    {
        return new TimedDamageOverTimeEffect(this, enemy);
    }
}
