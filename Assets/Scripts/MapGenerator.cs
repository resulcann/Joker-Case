using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private TileController tileController;

    private void Start()
    {
        LoadAndGenerateMap();
    }

    private void LoadAndGenerateMap()
    {
        if (jsonFile == null || tileController == null)
        {
            Debug.LogError("JSON dosyası veya TileController eksik!");
            return;
        }

        // JSON'u deserialize et
        MapData mapData = JsonUtility.FromJson<MapData>(jsonFile.text);

        // Tile'ları oluştur
        tileController.GenerateTiles(mapData);
    }
}