using UnityEngine;

public class TowerPlatform : MonoBehaviour
{
    [SerializeField] private float onMouseOverScale;

    private Events.TowerBoughtEvent _towerBought;
    
    private bool _haveTower;
    private Renderer _renderer;
    private Vector3 _normalScale;
    
    private void Start()
    {
        if (_towerBought == null) 
            _towerBought = new Events.TowerBoughtEvent();
        
        _towerBought.AddListener(GameManager.Instance.OnTowerBuyed);
        _normalScale = transform.localScale;
    }

    private void OnMouseOver()
    {
        var tower = BuildManager.Instance.CurrentTower;
        if (!tower)
            return;

        transform.localScale = _normalScale * onMouseOverScale;
    }

    private void OnMouseDown()
    {
        var tower = BuildManager.Instance.CurrentTower;
        
        if (!tower || _haveTower)
            return;
        
        int price = tower.GetComponent<TowerController>().Price;
        if (GameManager.Instance.Gold < price)
            return;

        if (!PoolManager.GetObject(tower.name, transform.position, Quaternion.identity))
        {
            Instantiate(tower, transform.position, Quaternion.identity);
        }

        _haveTower = true;
        _towerBought?.Invoke(price);
    }

    private void OnMouseExit()
    {
        transform.localScale = _normalScale;
    }
}
