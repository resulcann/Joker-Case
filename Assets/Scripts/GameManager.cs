using System;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
    public void Start()
    {
        Application.targetFrameRate = 60;
        
        MapGenerator.Instance.Init();
        DiceController.Instance.Init();
        InventoryManager.Instance.Init();
        Player.Instance.Init();
    }
    
    /// <summary>
    /// Sayı biçimlendirme ("1.23K", "1.5M" gibi)
    /// </summary>
    public string FormatNumber(int value)
    {
        if (value >= Mathf.Pow(10,6))
        {
            return (value / Mathf.Pow(10,6)).ToString("F2") + "M";
        }
        else if (value >= Mathf.Pow(10,3))
        {
            return (value / Mathf.Pow(10,3)).ToString("F2") + "K";
        }
        return value.ToString();
    }
}