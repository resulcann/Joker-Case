using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceController : GenericSingleton<DiceController>
{
    [Header("References")]
    [SerializeField] private TMP_Dropdown diceDropdown; // Dropdown menü
    [SerializeField] private Button rollButton;
    [SerializeField] private Button diceSettingsBtn;
    [SerializeField] private Transform diceSettingsParent;
    [Space]
    [SerializeField] private Dice dicePrefab; // Zar prefab
    [SerializeField] private Transform diceSpawnPoint; // Zarların spawnlanacağı nokta
    [SerializeField] private Transform diceContainer; // Zarların tutulacağı parent
    [Space]
    [SerializeField] private float gapBetweenDices = 0.5f;
    [SerializeField] private float gapBetweenRows = 1f;
    [SerializeField] private int maxDicePerRow;
    [SerializeField] private float timeThresholdBetweenDices = 0;
    
    private readonly List<Dice> _currentDice = new();
    private const string DropDownValuePrefName = "DiceDropdownValue";

    public void Init()
    {
        Load();
        ShowButtons(true);
        DiceSettingsPanel.Instance.Init(GetDiceAmount());
        
        rollButton.onClick.AddListener(OnRollButton_Clicked);
        diceSettingsBtn.onClick.AddListener(OnDiceSettingsButton_Clicked);
        diceDropdown.onValueChanged.AddListener(OnDropDownValueChanged);
        
    }

    private void RollDices()
    {
        ShowButtons(false);
    
        var diceValues = DiceSettingsPanel.Instance.GetDiceValues(); 
        
        foreach (var dv in diceValues)
        {
            Debug.Log(dv);
        }
        StartDiceRollProcess(diceValues, () =>
        {
            var totalValue = diceValues.Sum();

            Player.Instance.AddSteps(totalValue);
            ShowButtons(true);
        });
    }

    private void StartDiceRollProcess(List<int> diceValues, System.Action onComplete)
    {
        StartCoroutine(RollDicesCoroutine(diceValues, onComplete));
    }

    private IEnumerator RollDicesCoroutine(List<int> diceValues, System.Action onComplete)
    {
        ClearDice();
        
        var totalRows = Mathf.CeilToInt((float)diceValues.Count / maxDicePerRow); // Toplam satır sayısı

        for (var i = 0; i < diceValues.Count; i++)
        {
            var currentRow = i / maxDicePerRow; // Şu anki satır
            var indexInRow = i % maxDicePerRow; // Satırdaki indeks
        
            // X ekseninde pozisyon (satırdaki zarları ortalamak için hesaplanır)
            var startX = -(Mathf.Min(diceValues.Count - (currentRow * maxDicePerRow), maxDicePerRow) - 1) * gapBetweenDices / 2f;
            var xPosition = startX + indexInRow * gapBetweenDices;

            // Z ekseninde pozisyon
            var zPosition = (totalRows > 1) ? (gapBetweenRows / 2f * (totalRows - 1)) - currentRow * gapBetweenRows : 0f;

            // Zar spawn pozisyonu
            var spawnPosition = new Vector3(xPosition, diceSpawnPoint.position.y, zPosition);
            
            var dice = Instantiate(dicePrefab, spawnPosition, Quaternion.identity, diceContainer);
            dice.SetTargetValue(diceValues[i]);
            dice.Roll(spawnPosition); // Zar animasyonuna başla

            _currentDice.Add(dice);

            yield return new WaitForSeconds(timeThresholdBetweenDices); // Zarların atılma arası gecikme
        }

        // Tüm zarların durmasını bekle
        yield return new WaitUntil(AllDiceStopped);

        onComplete?.Invoke();
    }
    
    private bool AllDiceStopped()
    {
        foreach (var dice in _currentDice)
        {
            if (!dice.HasStopped()) return false;
        }
        return true;
    }


    private void ClearDice()
    {
        foreach (var dice in _currentDice)
        {
            Destroy(dice.gameObject);
        }
        _currentDice.Clear();
    }

    private void OnRollButton_Clicked()
    {
        RollDices();
    }
    
    private void OnDiceSettingsButton_Clicked()
    {
        DiceSettingsPanel.Instance.OpenDiceSettingsPanel(GetDiceAmount());
        ShowButtons(false);
    }

    private void OnDropDownValueChanged(int dropDownValue)
    {
        DiceSettingsPanel.Instance.UpdateDiceSettingsVisual(dropDownValue);
        Save();
    }
    
    public void ShowButtons(bool show)
    {
        rollButton.gameObject.SetActive(show);
        diceSettingsBtn.gameObject.SetActive(show);
    }
    public int GetDiceAmount() => diceDropdown.value;

    public void Save()
    {
        PlayerPrefs.SetInt(DropDownValuePrefName, diceDropdown.value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        diceDropdown.value = PlayerPrefs.GetInt(DropDownValuePrefName, 2);
        diceDropdown.onValueChanged?.Invoke(diceDropdown.value);
    }
}
