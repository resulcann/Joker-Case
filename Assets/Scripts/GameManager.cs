using System;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
    public void Start()
    {
        MapGenerator.Instance.Init();
        DiceController.Instance.Init();
        InventoryManager.Instance.Init();
        Player.Instance.Init();
    }
}