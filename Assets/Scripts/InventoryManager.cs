using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : GenericSingleton<InventoryManager>
{
    [System.Serializable]
    public class FruitUI
    {
        public string type;
        public TMP_Text amount;
        public string prefKey;
    }

    [Header("Fruit UI Elements")]
    [SerializeField] private List<FruitUI> fruitUIList;

    private readonly Dictionary<string, int> _fruitInventory = new();

    public void Init()
    {
        LoadInventory();
        UpdateAllFruitUI();
    }

    /// <summary>
    /// Increases fruit amount and updates it.
    /// </summary>
    /// <param name="fruitType">Fruit type</param>
    /// <param name="amount">Amount to add</param>
    public void AddFruit(string fruitType, int amount)
    {
        if (_fruitInventory.ContainsKey(fruitType))
        {
            _fruitInventory[fruitType] += amount;
            SaveFruit(fruitType);
            UpdateFruitUI(fruitType);
        }
        else
        {
            throw new System.Exception($"Fruit type {fruitType} not found");
        }
    }

    /// <summary>
    /// Updates fruit amounts on canvas.
    /// </summary>
    /// <param name="fruitType">Fruit Type</param>
    private void UpdateFruitUI(string fruitType)
    {
        foreach (var fruitUI in fruitUIList)
        {
            if (fruitUI.type == fruitType)
            {
                var value = _fruitInventory[fruitType];
                fruitUI.amount.text = GameUtility.FormatNumber(value);
                return;
            }
        }
    }

    /// <summary>
    /// Updates all fruit's UI elements.
    /// </summary>
    private void UpdateAllFruitUI()
    {
        foreach (var fruitUI in fruitUIList)
        {
            UpdateFruitUI(fruitUI.type);
        }
    }

    /// <summary>
    /// Saves values of fruit
    /// </summary>
    private void SaveFruit(string fruitType)
    {
        foreach (var fruitUI in fruitUIList)
        {
            if (fruitUI.type == fruitType)
            {
                PlayerPrefs.SetInt(fruitUI.prefKey, _fruitInventory[fruitType]);
                PlayerPrefs.Save();
                return;
            }
        }
    }

    /// <summary>
    /// Loads fruits.
    /// </summary>
    private void LoadInventory()
    {
        foreach (var fruitUI in fruitUIList)
        {
            var savedValue = PlayerPrefs.GetInt(fruitUI.prefKey, 0);
            _fruitInventory[fruitUI.type] = savedValue;
        }
    }
}
