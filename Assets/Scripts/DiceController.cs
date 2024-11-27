using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceController : GenericSingleton<DiceController>
{
    [SerializeField] private TMP_Dropdown diceDropdown; // Dropdown menü
    [SerializeField] private Button rollButton;
    [SerializeField] private Button diceSettingsBtn;
    [SerializeField] private Transform diceSettingsParent;

    private string _dropDownValuePrefName = "DiceDropdownValue";
    
    public void Init()
    {
        DiceSettingsPanel.Instance.Init();
        Load();
        ShowButtons(true);
        
        rollButton.onClick.AddListener(OnRollButton_Clicked);
        diceSettingsBtn.onClick.AddListener(OnDiceSettingsButton_Clicked);
        diceDropdown.onValueChanged.AddListener(OnDropDownValueChanged);
        
    }

    private void RollDices()
    {
        // diceların atılma animasyonu vs burada yapılcak
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
        PlayerPrefs.SetInt(_dropDownValuePrefName, diceDropdown.value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        diceDropdown.value = PlayerPrefs.GetInt(_dropDownValuePrefName, 2);
        diceDropdown.onValueChanged?.Invoke(diceDropdown.value);
    }
}
