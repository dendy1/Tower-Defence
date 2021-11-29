using UnityEngine;

public class LayerManager : Singleton<LayerManager>
{
    public LayerMask enemyLayerMask;
    public LayerMask platformLayerMask;
    public LayerMask towerLayerMask;

    protected override void Awake()
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");
        platformLayerMask = LayerMask.GetMask("Platform");
        towerLayerMask = LayerMask.GetMask("Tower");
    }
}