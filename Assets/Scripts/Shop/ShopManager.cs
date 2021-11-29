using Core.Utilities.ObjectPool;
using DG.Tweening;
using JetBrains.Annotations;
using TowerDefense.Levels;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Shop
{
    public class ShopManager : Singleton<ShopManager>
    {
        public GameObject shopContainer;
        public ShopButton towerShopButton;
        
        public Tower CurrentTower { get; private set; }

        private Camera _mainCamera;
        private Vector3 _moveVelocity;
        private float _dampSpeed = 0.5f;
        
        public ShopState CurrentState { get; private set; }
        public bool IsBuilding => CurrentState == ShopState.Building || CurrentState == ShopState.BuildingWithDrag;
        
        protected override void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main;
        }
        
        private void Start()
        {
            foreach (var tower in GameManager.Instance.TowerLibrary)
            {
                var shopButton = Instantiate(towerShopButton, shopContainer.transform);
                shopButton.InitializeButton(tower);
                shopButton.buttonTapped += OnButtonTapped;
                shopButton.buttonDragged += OnButtonDragged;
                shopButton.buttonDragEnded += OnButtonDragEnded;
            }
        }

        private void Update()
        {
            if (CurrentTower != null)
            {
                // Right mouse button pressed
                if (Input.GetMouseButtonDown(1))
                {
                    CancelTowerPlacement();
                    return;
                }
                
                var plane = new Plane(Vector3.up, 0);
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (plane.Raycast(ray, out var distance))
                {
                    if (Physics.Raycast(ray, out var hit, distance, LayerManager.Instance.platformLayerMask))
                    {
                        CurrentTower.transform.DOMove(hit.collider.transform.position, _dampSpeed);
                    }
                    else
                    {
                        var worldPosition = ray.GetPoint(distance);
                        CurrentTower.transform.DOMove(worldPosition, _dampSpeed);
                    }
                }
            }
        }

        private void OnButtonTapped(Tower tower)
        {
            if (CurrentTower != null)
            {
                CancelTowerPlacement();
            }
            SetUpTowerPlacement(tower);
            CurrentState = ShopState.Building;
        }
        
        private void OnButtonDragged(Tower tower)
        {
            if (!IsBuilding)
            {
                if (CurrentTower != null)
                {
                    CancelTowerPlacement();
                }
                SetUpTowerPlacement(tower);
                CurrentState = ShopState.BuildingWithDrag;
            }
        }
        
        private void OnButtonDragEnded(Tower tower)
        {
            if (IsBuilding)
            {
                if (CurrentTower != null)
                {
                    if (CurrentTower.CurrentPlatform != null)
                    {
                        CurrentTower.CurrentPlatform.PlaceTower();
                        CurrentState = ShopState.Normal;
                    }
                }
            }
        }
        
        private void SetUpTowerPlacement([NotNull] Tower towerToBuild)
        {
            CurrentTower = Poolable.TryGetPoolable<Tower>(towerToBuild.gameObject);
            CurrentTower.TowerPlaced += TowerPlaced;
            CurrentTower.TowerRemoved += TowerRemoved;
            
            var plane = new Plane(Vector3.up, 0);
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out var distance))
            {
                var worldPosition = ray.GetPoint(distance);
                CurrentTower.transform.position = worldPosition;
            }
        }
        
        private void CancelTowerPlacement()
        {
            CurrentState = ShopState.Normal;
            Poolable.TryPool(CurrentTower.gameObject);
            CurrentTower.TowerPlaced -= TowerPlaced;
            CurrentTower.TowerRemoved -= TowerRemoved;
            CurrentTower = null;
        }

        private void TowerPlaced(Tower tower)
        {
            CurrentState = ShopState.Normal;
            if (CurrentTower == tower)
            {
                CurrentTower.TowerPlaced -= TowerPlaced;
                CurrentTower.TowerRemoved -= TowerRemoved;
                CurrentTower = null;
            }
        }

        private void TowerRemoved(Tower tower)
        {
            CurrentState = ShopState.Normal;
        }
        
        public enum ShopState
        {
            Normal,
            Building,
            BuildingWithDrag,
        }
    }
}