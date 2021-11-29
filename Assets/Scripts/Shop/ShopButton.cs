using System;
using TMPro;
using TowerDefense.Currency;
using TowerDefense.Levels;
using TowerDefense.Towers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefense.Shop
{
    [RequireComponent(typeof(Button))]
    public class ShopButton : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color unAffordableInvalidColor;
        [SerializeField] private Image towerIcon;
        [SerializeField] private TMP_Text towerPrice;
        
        private Tower _tower;
        private Currency.Currency _currency;
        private RectTransform _rectTransform;
        
        public event Action<Tower> buttonTapped;
        public event Action<Tower> buttonDragged;
        public event Action<Tower> buttonDragEnded;

        public Button BuyButton { get; set; }
    
        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
            BuyButton = GetComponent<Button>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, eventData.position))
            {
                buttonDragged?.Invoke(_tower);
            }
        }
        
        private void OnClick()
        {
            buttonTapped?.Invoke(_tower);
        }
        
        public void InitializeButton(Tower tower)
        {
            BuyButton.onClick.AddListener(OnClick);
            _tower = tower;

            towerPrice.text = tower.Cost.ToString();
            towerIcon.sprite = tower.Icon;

            _currency = GameManager.Instance.Currency;
            _currency.CurrencyChanged += UpdateButton;
            UpdateButton(new CurrencyChangeInfo(_currency.CurrentCurrency, _currency.CurrentCurrency));
        }
        
        private void UpdateButton(CurrencyChangeInfo info)
        {
            if (_currency == null)
            {
                return;
            }

            if (_currency.CanAfford(_tower.Cost) && !BuyButton.interactable)
            {
                BuyButton.interactable = true;
                towerIcon.color = defaultColor;
                towerPrice.color = defaultColor;
            }
            else if (!_currency.CanAfford(_tower.Cost) && BuyButton.interactable)
            {
                BuyButton.interactable = false;
                towerIcon.color = unAffordableInvalidColor;
                towerPrice.color = unAffordableInvalidColor;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            buttonDragEnded?.Invoke(_tower);
        }
    }
}