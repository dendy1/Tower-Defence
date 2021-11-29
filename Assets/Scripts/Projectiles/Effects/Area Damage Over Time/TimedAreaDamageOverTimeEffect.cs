using System;
using TowerDefense.Enemies;
using UnityEngine;

public class TimedAreaDamageOverTimeEffect : TimedEffect
{
    private Enemy _mainEnemy;
    private Collider[] _closeEnemies;
    private int _enemiesCount;
    
    public TimedAreaDamageOverTimeEffect(ScriptableEffect effect, Enemy enemy) : base(effect, enemy)
    {
        var damageEffect = (ScriptableAreaDamageEffect)Effect;
        _mainEnemy = enemy;
        _closeEnemies = new Collider[damageEffect.maxEnemies];
        _enemiesCount = Physics.OverlapSphereNonAlloc(_mainEnemy.transform.position, damageEffect.areaSize, _closeEnemies, LayerManager.Instance.enemyLayerMask);
    }

    protected override void ApplyEffect()
    {
        var effect = (ScriptableAreaDamageEffect)Effect;
        _mainEnemy.Health -= effect.damage;

        if (_enemiesCount > 0) {
            for (var i = 0; i < _enemiesCount; i++)
            {
                var target = _closeEnemies[i];
                if (target.gameObject == _mainEnemy.gameObject) 
                    continue;
                
                var closeEnemy = target.GetComponent<Enemy>();
                var distance = Vector3.Distance(closeEnemy.transform.position, _mainEnemy.transform.position);
                closeEnemy.Health -= Mathf.RoundToInt(distance / effect.areaSize * effect.damage);
            }
        }
    }

    public override void End()
    {
        EffectStacks = 0;
    }
}
