using System;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
    public void Start()
    {
        MapGenerator.Instance.Init();
        Player.Instance.Init();
    }
}