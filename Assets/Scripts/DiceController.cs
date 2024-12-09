using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceController : GenericSingleton<DiceController>
{
    [Header("References")]
    [SerializeField] private TMP_Dropdown diceDropdown; 
    [SerializeField] private Button rollButton;
    [SerializeField] private Button diceSettingsBtn;
    [SerializeField] private Transform diceSettingsParent;
    [Space]
    [SerializeField] private Dice dicePrefab; 
    [SerializeField] private Transform diceSpawnPoint; 
    [SerializeField] private Transform diceContainer; 
    [Space]
    [SerializeField] private float gapBetweenDices = 0.5f;
    [SerializeField] private float gapBetweenRows = 1f;
    [SerializeField] private int maxDicePerRow;
    [SerializeField] private float timeThresholdBetweenDices;
    [Space] [Header("Dice Value Settings")]
    [SerializeField] private int minDiceValue = 1;
    [SerializeField] private int maxDiceValue = 6;

    [Header("Pooling Settings")]
    [SerializeField] private int initialPoolSize = 10;

    private readonly List<Dice> _currentDice = new();
    private readonly Queue<Dice> _dicePool = new(); // Object Pool

    private const string DropDownValuePrefName = "DiceDropdownValue";

    #region PROPERTIES

    public int MinDiceValue { get => minDiceValue; set => minDiceValue = value; }
    public int MaxDiceValue { get => maxDiceValue; set => maxDiceValue = value; }
    public int DiceAmount { get => diceDropdown.value; set => diceDropdown.value = value; }

    #endregion

    public void Init()
    {
        InitializePool();
        Load();
        ShowButtons(true);
        DiceSettingsPanel.Instance.Init(DiceAmount);

        rollButton.onClick.AddListener(OnRollButton_Clicked);
        diceSettingsBtn.onClick.AddListener(OnDiceSettingsButton_Clicked);
        diceDropdown.onValueChanged.AddListener(OnDropDownValueChanged);
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            var dice = Instantiate(dicePrefab, diceContainer);
            dice.gameObject.SetActive(false);
            _dicePool.Enqueue(dice);
        }
    }

    private Dice GetDiceFromPool()
    {
        if (_dicePool.Count > 0)
        {
            var dice = _dicePool.Dequeue();
            dice.gameObject.SetActive(true);
            return dice;
        }
        else
        {
            var newDice = Instantiate(dicePrefab, diceContainer);
            return newDice;
        }
    }

    private void ReturnDiceToPool(Dice dice)
    {
        dice.gameObject.SetActive(false);
        _dicePool.Enqueue(dice);
    }

    private void RollDices()
    {
        ShowButtons(false);

        var diceValues = DiceSettingsPanel.Instance.GetDiceValues();
        var diceValueString = diceValues.Aggregate("", (current, dv) => current + (dv + ", "));
        
        Debug.Log("Dices Values: " + diceValueString);

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
        var stoppedDiceCount = 0;
        var totalRows = Mathf.CeilToInt((float)diceValues.Count / maxDicePerRow);

        for (var i = 0; i < diceValues.Count; i++)
        {
            var currentRow = i / maxDicePerRow;
            var indexInRow = i % maxDicePerRow;

            var startX = -(Mathf.Min(diceValues.Count - (currentRow * maxDicePerRow), maxDicePerRow) - 1) * gapBetweenDices / 2f;
            var xPosition = startX + indexInRow * gapBetweenDices;

            var zPosition = (totalRows > 1) ? (gapBetweenRows / 2f * (totalRows - 1)) - currentRow * gapBetweenRows : 0f;

            var spawnPosition = new Vector3(xPosition, diceSpawnPoint.position.y, zPosition);
            var dice = GetDiceFromPool();
            dice.transform.position = spawnPosition;
            dice.SetTargetValue(diceValues[i]);
            dice.Roll(spawnPosition);

            dice.OnDiceStopped += () =>
            {
                stoppedDiceCount++;
                if (stoppedDiceCount == diceValues.Count)
                {
                    onComplete?.Invoke();
                }
            };

            _currentDice.Add(dice);

            yield return new WaitForSeconds(timeThresholdBetweenDices);
        }
    }

    private void ClearDice()
    {
        foreach (var dice in _currentDice)
        {
            ReturnDiceToPool(dice);
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

    private void Save()
    {
        PlayerPrefs.SetInt(DropDownValuePrefName, diceDropdown.value);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        diceDropdown.value = PlayerPrefs.GetInt(DropDownValuePrefName, 2);
    }
}
