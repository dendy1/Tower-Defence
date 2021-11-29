using TowerDefense.Enemies;

public class TimedSpeedEffect : TimedEffect
{
    private Enemy _enemy;
    
    public TimedSpeedEffect(ScriptableEffect effect, Enemy enemy) : base(effect, enemy)
    {
        _enemy = enemy;
    }

    protected override void ApplyEffect()
    {
        var speedEffect = (ScriptableSpeedEffect)Effect;
        _enemy.Speed -= speedEffect.SpeedChange;
    }

    public override void End()
    {
        var speedEffect = (ScriptableSpeedEffect)Effect;
        _enemy.Speed += speedEffect.SpeedChange * EffectStacks;
        EffectStacks = 0;
    }
}
