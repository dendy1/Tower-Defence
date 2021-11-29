using TowerDefense.Enemies;
using UnityEngine;

public abstract class ScriptableEffect : ScriptableObject
{
    /// <summary>
    /// Time duration of the buff in seconds.
    /// </summary>
    public float Duration;

    /// <summary>
    /// Duration is increased each time the buff is applied.
    /// </summary>
    public bool IsDurationStacked;

    /// <summary>
    /// Effect value is increased each time the buff is applied.
    /// </summary>
    public bool IsEffectStacked;

    /// <summary>
    /// Effect value is applied in some interval.
    /// </summary>
    public bool IsOverTime;
    
    public abstract TimedEffect InitializeBuff(Enemy enemy);
}
