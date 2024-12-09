using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSettingsPanel : GenericSingleton<DiceSettingsPanel>
{
    [Header("UI Elements")]
    [SerializeField] private GameObject inner;
    [SerializeField] private Button closePanelBtn;
    [SerializeField] private Button randomBtn;
    [SerializeField] private Sprite[] diceSprites;
    [Space]
    [SerializeField] private List<DiceSettingsUI> diceSettingsList = new();
    
    public void Init(int diceAmount)
    {
        diceSettingsList.ForEach(ds => ds.Init());
        CloseDiceSettingsPanel();
        UpdateDiceSettingsVisual(diceAmount);
        
        closePanelBtn.onClick.AddListener(CloseDiceSettingsPanel);
        randomBtn.onClick.AddListener(OnRandomBtn_Clicked);
    }

    public void OpenDiceSettingsPanel(int diceAmount)
    {
        if(!inner.activeInHierarchy) inner.gameObject.SetActive(true);
        
        UpdateDiceSettingsVisual(diceAmount);
    }
    private void CloseDiceSettingsPanel()
    {
        inner.gameObject.SetActive(false);
        DiceController.Instance.ShowButtons(true);
    }

    private void OnRandomBtn_Clicked()
    {
        foreach (var ds in diceSettingsList)
        {
            ds.SetDiceValue(Random.Range(1, 7));
            ds.UpdateInputFieldText();
        }
    }

    public void UpdateDiceSettingsVisual(int diceAmount)
    {
        diceSettingsList.ForEach(ds => ds.gameObject.SetActive(false));
        
        for (var i = 0; i < diceAmount + 1; i++)
        {
            diceSettingsList[i].gameObject.SetActive(true);
            diceSettingsList.ForEach(ds => ds.Load());
        }
    }
    
    public Sprite GetDiceSprite(int diceValue)
    {
        if (diceValue < 0 || diceValue >= diceSprites.Length)
        {
            throw new System.ArgumentOutOfRangeException(nameof(diceValue), $"Dice value must be between 0 and {diceSprites.Length - 1}, but was {diceValue}.");
        }
        return diceSprites[diceValue];
    }

    
    public List<int> GetDiceValues()
    {
        var diceValues = new List<int>();
        foreach (var ds in diceSettingsList)
        {
            if (ds.gameObject.activeSelf)
            {
                diceValues.Add(ds.GetCurrentDiceValue());
            }
        }
        return diceValues;
    }

}
