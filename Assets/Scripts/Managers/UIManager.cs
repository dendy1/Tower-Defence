using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Win/Lost Screens")] 
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    
    [Header("Text Fields")]
    [SerializeField] private TMP_Text healthValueText;
    [SerializeField] private TMP_Text currencyValueText;
    [SerializeField] private TMP_Text wavesValueText;
    [SerializeField] private float textAnimationDuration;

    [Header("Buttons")] 
    [SerializeField] private Button nextWaveButton;
    [SerializeField] private Toggle autoStartToggle;

    public bool AutoStart => autoStartToggle.isOn;
    
    public void InitializeValues(int currentHealth, int currentCurrency, int currentWave, int maxWaves)
    {
        healthValueText.text = currentHealth.ToString();
        currencyValueText.text = currentCurrency.ToString();
        wavesValueText.text = $"{currentWave}/{maxWaves}";
    }
    
    public void UpdateHealthValue(int previousHealth, int currentHealth)
    {
        healthValueText.DOTextInt(previousHealth, currentHealth, textAnimationDuration);
    }

    public void UpdateCurrencyValue(int previousCurrency, int currentCurrency)
    {
        currencyValueText.DOTextInt(previousCurrency, currentCurrency, textAnimationDuration);
    }

    public void UpdateWavesValue(int currentWave, int maxWaves)
    {
        wavesValueText.text = $"{currentWave}/{maxWaves}";
    }

    public void SetActiveNextWaveButton(bool active)
    {
        nextWaveButton.gameObject.SetActive(active);
    }

    public void SetActiveWinScreen(bool active)
    {
        winScreen.SetActive(active);
    }

    public void SetActiveLoseScreen(bool active)
    {
        loseScreen.SetActive(active);
    }
}
