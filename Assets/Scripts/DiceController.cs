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
    [Space] [Header("Dice Value Settings")]
    [SerializeField] private int minDiceValue = 1;
    [SerializeField] private int maxDiceValue = 6;
    
    private readonly List<Dice> _currentDice = new();
    private const string DropDownValuePrefName = "DiceDropdownValue";
    
    #region PROPERTIES
    
        public int MinDiceValue { get => minDiceValue; set => minDiceValue = value; }
        public int MaxDiceValue { get => maxDiceValue; set => maxDiceValue = value; }
        public int DiceAmount { get => diceDropdown.value; set => diceDropdown.value = value; }
    
    #endregion

    public void Init()
    {
        Load();
        ShowButtons(true);
        DiceSettingsPanel.Instance.Init(DiceAmount);
        
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
        var stoppedDiceCount = 0; // Counter for stopped dices
        var totalRows = Mathf.CeilToInt((float)diceValues.Count / maxDicePerRow); // Total rows

        for (var i = 0; i < diceValues.Count; i++)
        {
            var currentRow = i / maxDicePerRow; // Current row
            var indexInRow = i % maxDicePerRow; // Index in row

            // X position for centering dice in the row
            var startX = -(Mathf.Min(diceValues.Count - (currentRow * maxDicePerRow), maxDicePerRow) - 1) * gapBetweenDices / 2f;
            var xPosition = startX + indexInRow * gapBetweenDices;

            // Z position
            var zPosition = (totalRows > 1) ? (gapBetweenRows / 2f * (totalRows - 1)) - currentRow * gapBetweenRows : 0f;

            var spawnPosition = new Vector3(xPosition, diceSpawnPoint.position.y, zPosition);
            var dice = Instantiate(dicePrefab, spawnPosition, Quaternion.identity, diceContainer);
            dice.SetTargetValue(diceValues[i]);
            dice.Roll(spawnPosition); // Start dice animation

            // Subscribe to the OnDiceStopped event
            dice.OnDiceStopped += () =>
            {
                stoppedDiceCount++;
                if (stoppedDiceCount == diceValues.Count) // Check if all dice stopped
                {
                    onComplete?.Invoke();
                }
            };

            _currentDice.Add(dice);

            yield return new WaitForSeconds(timeThresholdBetweenDices); // Delay between dice rolls
        }
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
        DiceSettingsPanel.Instance.OpenDiceSettingsPanel(DiceAmount);
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
    
    public void Save()
    {
        PlayerPrefs.SetInt(DropDownValuePrefName, diceDropdown.value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        diceDropdown.value = PlayerPrefs.GetInt(DropDownValuePrefName, 2);
    }
}
