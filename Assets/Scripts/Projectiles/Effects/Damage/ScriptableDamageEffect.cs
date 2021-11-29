using TowerDefense.Enemies;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Instant Damage Effect")]
public class ScriptableDamageEffect : ScriptableEffect
{
    [Min(0)]
    public int Damage;
    
    public override TimedEffect InitializeBuff(Enemy enemy)
    {
        return new TimedDamageEffect(this, enemy);
    }
}
