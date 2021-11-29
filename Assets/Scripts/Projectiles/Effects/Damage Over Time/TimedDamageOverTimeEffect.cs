using TowerDefense.Enemies;

public class TimedDamageOverTimeEffect : TimedEffect
{
    private Enemy _enemy;
    
    public TimedDamageOverTimeEffect(ScriptableEffect effect, Enemy enemy) : base(effect, enemy)
    {
        _enemy = enemy;
    }

    protected override void ApplyEffect()
    {        
        var effect = (ScriptableDamageOverTimeEffect)Effect;
        _enemy.Health -= effect.DamageOverTime;
    }

    public override void End()
    {
        EffectStacks = 0;
    }
}
