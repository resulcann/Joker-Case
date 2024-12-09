using System;
using UnityEngine;

public class MapGenerator : GenericSingleton<MapGenerator>
{
    [SerializeField] private TextAsset mapDataJsonFile;
    public MapData MapData { get; private set; }

    public void Init()
    {
        LoadAndGenerateMap();
    }

    private void LoadAndGenerateMap()
    {
        if (mapDataJsonFile == null)
        {
            throw new NullReferenceException("Map data JSON file is null.");
        }

        try
        {
            MapData = JsonUtility.FromJson<MapData>(mapDataJsonFile.text);
            TileController.Instance.GenerateTiles(MapData);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load map data: {ex.Message}");
        }
    }
}