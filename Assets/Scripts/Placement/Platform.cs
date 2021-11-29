using DG.Tweening;
using TowerDefense.Levels;
using TowerDefense.Shop;
using UnityEngine;

namespace TowerDefense.Placement
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private float onMouseOverScale;
        public bool TowerPlaced { get; set; }
        
        private Vector3 _normalScale;
        private float _onMouseOverScaleDuration = 0.5f;
        
        private void Start()
        {
            _normalScale = transform.localScale;
        }
    
        private void OnMouseEnter()
        {
            if (TowerPlaced)
                return;

            var currentTower = ShopManager.Instance.CurrentTower;
            if (!currentTower)
                return;

            currentTower.transform.position = transform.position;
            currentTower.CurrentPlatform = this;
            transform.DOScale(_normalScale * onMouseOverScale, _onMouseOverScaleDuration);
        }
    
        private void OnMouseExit()
        {
            if (TowerPlaced)
                return;
            
            transform.DOScale(_normalScale, _onMouseOverScaleDuration);
            
            var currentTower = ShopManager.Instance.CurrentTower;
            if (!currentTower)
                return;

            currentTower.CurrentPlatform = null;
        }

        private void OnMouseUp()
        {
            if (TowerPlaced)
                return;
            
            PlaceTower();
        }

        private void OnMouseDown()
        {
            if (TowerPlaced)
                return;
            
            PlaceTower();
        }

        public void PlaceTower()
        {
            var currentTower = ShopManager.Instance.CurrentTower;
            if (!currentTower || !GameManager.Instance.Currency.CanAfford(currentTower.Cost))
                return;
    
            if (GameManager.Instance.Currency.TryPurchase(currentTower.Cost))
            {
                TowerPlaced = true;
                currentTower.PlaceTower();
                transform.DOScale(_normalScale, _onMouseOverScaleDuration);
            }
        }
    }
}
