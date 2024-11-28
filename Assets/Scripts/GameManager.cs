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
}