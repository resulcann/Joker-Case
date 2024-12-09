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
    /// Meyve miktarını arttırır ve UI'yi günceller.
    /// </summary>
    /// <param name="fruitType">Meyve türü</param>
    /// <param name="amount">Eklenecek miktar</param>
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
            Debug.LogWarning($"Fruit type not found: {fruitType}");
        }
    }

    /// <summary>
    /// Meyve miktarını UI'ye günceller.
    /// </summary>
    /// <param name="fruitType">Meyve türü</param>
    private void UpdateFruitUI(string fruitType)
    {
        foreach (var fruitUI in fruitUIList)
        {
            if (fruitUI.type == fruitType)
            {
                var value = _fruitInventory[fruitType];
                fruitUI.amount.text = GameUtilities.Instance.FormatNumber(value);
                return;
            }
        }
    }

    /// <summary>
    /// Tüm UI elemanlarını günceller.
    /// </summary>
    private void UpdateAllFruitUI()
    {
        foreach (var fruitUI in fruitUIList)
        {
            UpdateFruitUI(fruitUI.type);
        }
    }

    /// <summary>
    /// Meyvenin değerini saveler.
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
    /// Tüm meyve envanterini yükler.
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
