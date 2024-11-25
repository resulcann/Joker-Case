using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;       // JSON dosyası
    [SerializeField] private TileController tileController; // TileController referansı

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

        // JSON'u deserialization yap
        MapData mapData = JsonUtility.FromJson<MapData>(jsonFile.text);

        // Tile'ları oluştur
        tileController.GenerateTiles(mapData);
    }
}