using UnityEngine;

public class MapGenerator : GenericSingleton<MapGenerator>
{
    [SerializeField] private TextAsset jsonFile;

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
        MapData mapData = JsonUtility.FromJson<MapData>(jsonFile.text);

        // Tile'ları oluştur
        TileController.Instance.GenerateTiles(mapData);
    }
}