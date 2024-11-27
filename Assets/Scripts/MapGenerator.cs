using UnityEngine;

public class MapGenerator : GenericSingleton<MapGenerator>
{
    [SerializeField] private TextAsset jsonFile;
    private MapData _mapData;

    public void Init()
    {
        LoadAndGenerateMap();
    }

    private void LoadAndGenerateMap()
    {
        if (jsonFile == null || TileController.Instance == null) // json dosyası veya tilecnotroller kontrolü
        {
            Debug.LogError("JSON file or TileController is missing!");
            return;
        }

        // mapdata json okunuyor.
        _mapData = JsonUtility.FromJson<MapData>(jsonFile.text);

        // Tilelar oluşturuluyor.
        TileController.Instance.GenerateTiles(_mapData);
    }
    
    public MapData GetMapData() => _mapData;
}