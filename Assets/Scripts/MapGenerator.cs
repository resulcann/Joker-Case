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
        if (jsonFile == null || TileController.Instance == null)
        {
            Debug.LogError("JSON dosyası veya TileController eksik!");
            return;
        }

        // JSON'u deserialize et
        _mapData = JsonUtility.FromJson<MapData>(jsonFile.text);

        // Tile'ları oluştur
        TileController.Instance.GenerateTiles(_mapData);
    }
    
    public MapData GetMapData() => _mapData;
}