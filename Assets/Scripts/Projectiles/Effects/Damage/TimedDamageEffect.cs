using TowerDefense.Enemies;

public class TimedDamageEffect : TimedEffect
{
    private Enemy _enemy;
    
    public TimedDamageEffect(ScriptableEffect effect, Enemy enemy) : base(effect, enemy)
    {
        _enemy = enemy;
    }

    protected override void ApplyEffect()
    {
    }

    public override void End()
    {
        var effect = (ScriptableDamageEffect)Effect;
        _enemy.Health -= effect.Damage;
        EffectStacks = 0;
    }
}
