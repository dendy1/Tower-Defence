using TowerDefense.Enemies;

public abstract class TimedEffect
{
    protected readonly Enemy Enemy;
    
    /// <summary>
    /// Full effect duration
    /// </summary>
    protected float Duration;
    
    /// <summary>
    /// After how long will the effect be applied again
    /// </summary>
    protected float Interval;
    
    /// <summary>
    /// Current effect stacks count
    /// </summary>
    protected int EffectStacks;
    
    public ScriptableEffect Effect { get; }
    public bool IsFinished;

    private float _interval;
    
    public TimedEffect(ScriptableEffect effect, Enemy enemy)
    {
        Effect = effect;
        Enemy = enemy;
        _interval = Interval;
    }

    public void Tick(float delta)
    {
        Duration -= delta;
        if (Duration <= 0)
        {
            End();
            IsFinished = true;
        }

        if (Effect.IsOverTime)
        {
            _interval -= delta;
            if (_interval <= 0)
            {
                ApplyEffect();
                _interval = Interval;
            }
        }
    }

    /// <summary>
    /// Activates buff or extends duration if ScriptableBuff has IsDurationStacked or IsEffectStacked set to true.
    /// </summary>
    public void Activate()
    {
        if (Effect.IsEffectStacked || Duration <= 0)
        {
            ApplyEffect();
            EffectStacks++;
        }
        
        if (Effect.IsDurationStacked || Duration <= 0)
        {
            Duration += Effect.Duration;
        }
    }
    
    protected abstract void ApplyEffect();
    
    public abstract void End();
}
