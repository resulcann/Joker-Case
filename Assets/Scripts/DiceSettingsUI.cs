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
        _currentDiceValue = Mathf.Clamp(diceValue, 1, 6);
        SetDiceImage();
        Save();
    }
    
    private void ValidateInput(string input)
    {
        if (int.TryParse(input, out var value))
        {
            SetDiceValue(Mathf.Clamp(value, 1, 6));
        }
        else
        {
            _currentDiceValue = Mathf.Clamp(_currentDiceValue, 1, 6);
        }

        UpdateInputFieldText();
    }

    
    private void SetDiceImage()
    {
        if (_currentDiceValue >= 1 && _currentDiceValue <= 6)
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
        _currentDiceValue = PlayerPrefs.GetInt(prefName, Random.Range(1,7));
        UpdateInputFieldText();
        SetDiceImage();
    }

    public int GetCurrentDiceValue() => _currentDiceValue;

}
