using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DiceSettingsUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Image diceImg;
    [Space] 
    [SerializeField] private string prefName;

    private int _currentDiceValue;

    public int CurrentDiceValue
    {
        get => _currentDiceValue;
        set
        {
            _currentDiceValue = Mathf.Clamp(value, DiceController.Instance.MinDiceValue, DiceController.Instance.MaxDiceValue);
            UpdateInputFieldText();
            
            if (_currentDiceValue >= DiceController.Instance.MinDiceValue && _currentDiceValue <= DiceController.Instance.MaxDiceValue)
            {
                diceImg.sprite = DiceSettingsPanel.Instance.GetDiceSprite(_currentDiceValue - 1);
            }
            
            Save();
        }
    }

    public void Init()
    {
        Load();
        inputField.onEndEdit.AddListener(ValidateInput);
    }

    private void ValidateInput(string input)
    {
        if (int.TryParse(input, out var value))
        {
            CurrentDiceValue = value;
        }
        else
        {
            UpdateInputFieldText();
        }
    }
    
    private void UpdateInputFieldText()
    {
        inputField.text = CurrentDiceValue.ToString();
    }

    private void Save()
    {
        PlayerPrefs.SetInt(prefName, CurrentDiceValue);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        CurrentDiceValue = PlayerPrefs.GetInt(prefName, Random.Range(DiceController.Instance.MinDiceValue, DiceController.Instance.MaxDiceValue + 1));
        UpdateInputFieldText();
    }
}