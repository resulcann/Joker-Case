using System;
using System.Collections.Generic;
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

    public void Init()
    {
        Load();
        inputField.onEndEdit.AddListener(ValidateInput);
    }

    public void SetDiceValue(int diceValue)
    {
        _currentDiceValue = Mathf.Clamp(diceValue, DiceController.Instance.MinDiceValue, DiceController.Instance.MaxDiceValue);
        SetDiceImage();
        Save();
    }
    
    private void ValidateInput(string input)
    {
        if (int.TryParse(input, out var value))
        {
            SetDiceValue(Mathf.Clamp(value, DiceController.Instance.MinDiceValue, DiceController.Instance.MaxDiceValue));
        }
        else
        {
            _currentDiceValue = Mathf.Clamp(_currentDiceValue, DiceController.Instance.MinDiceValue, DiceController.Instance.MaxDiceValue);
        }

        UpdateInputFieldText();
    }

    
    private void SetDiceImage()
    {
        if (_currentDiceValue >= DiceController.Instance.MinDiceValue && _currentDiceValue <= DiceController.Instance.MaxDiceValue)
        {
            diceImg.sprite = DiceSettingsPanel.Instance.GetDiceSprite(_currentDiceValue - 1);
        }
    }

    public void UpdateInputFieldText()
    {
        inputField.text = _currentDiceValue.ToString();
    }

    private void Save()
    {
        PlayerPrefs.SetInt(prefName, _currentDiceValue);
        PlayerPrefs.Save();
    }
    public void Load()
    {
        _currentDiceValue = PlayerPrefs.GetInt(prefName, Random.Range(DiceController.Instance.MinDiceValue, DiceController.Instance.MaxDiceValue + 1));
        UpdateInputFieldText();
        SetDiceImage();
    }

    public int GetCurrentDiceValue() => _currentDiceValue;

}
