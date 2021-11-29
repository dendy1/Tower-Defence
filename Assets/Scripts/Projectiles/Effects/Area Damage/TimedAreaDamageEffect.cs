using TowerDefense.Enemies;
using UnityEngine;

public class TimedAreaDamageEffect : TimedEffect
{
    private Enemy _mainEnemy;
    
    public TimedAreaDamageEffect(ScriptableEffect effect, Enemy enemy) : base(effect, enemy)
    {
        _mainEnemy = enemy;
    }

    protected override void ApplyEffect()
    {
    }

    public override void End()
    {
        var effect = (ScriptableAreaDamageEffect)Effect;
        
        _mainEnemy.Health -= effect.damage;

        var targets = new Collider[effect.maxEnemies];
        var enemiesCount = Physics.OverlapSphereNonAlloc(_mainEnemy.transform.position, effect.areaSize, targets, LayerManager.Instance.enemyLayerMask);
        
        if (enemiesCount > 0) {
            for (var i = 0; i < enemiesCount; i++)
            {
                var target = targets[i];
                if (target.gameObject == _mainEnemy.gameObject) 
                    continue;
                
                var closeEnemy = target.GetComponent<Enemy>();
                var distance = Vector3.Distance(closeEnemy.transform.position, _mainEnemy.transform.position);
                closeEnemy.Health -= Mathf.RoundToInt(distance / effect.areaSize * effect.damage);
            }
        }
        
        EffectStacks = 0;
    }
}
