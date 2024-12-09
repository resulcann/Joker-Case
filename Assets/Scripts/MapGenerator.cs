using UnityEngine;

public class MapGenerator : GenericSingleton<MapGenerator>
{
    [SerializeField] private TextAsset mapDataJsonFile;
    private MapData _mapData;

    public void Init()
    {
        LoadAndGenerateMap();
    }

    private void LoadAndGenerateMap()
    {
        if (mapDataJsonFile == null || TileController.Instance == null) // json dosyası veya tilecnotroller kontrolü
        {
            Debug.LogError("JSON file or TileController is missing!");
            return;
        }

        // mapdata json okunuyor.
        _mapData = JsonUtility.FromJson<MapData>(mapDataJsonFile.text);

        // Tilelar oluşturuluyor.
        TileController.Instance.GenerateTiles(_mapData);
    }
    
    public MapData GetMapData() => _mapData;
}