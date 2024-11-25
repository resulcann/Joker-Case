using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private TileController tileController;

    private void Start()
    {
        LoadTileProperties();
    }

    private void LoadTileProperties()
    {
        if (jsonFile == null || tileController == null)
        {
            Debug.LogError("JSON dosyası veya TileController eksik!");
            return;
        }

        // JSON'u deserializasyon yap
        MapData mapData = JsonUtility.FromJson<MapData>(jsonFile.text);

        // Tüm Tile'lar için varsayılan türü Empty olarak ayarla
        var tileTypes = new List<TileType>();
        for (int i = 0; i < 28; i++)
        {
            tileTypes.Add(TileType.Empty);
        }

        // JSON'da belirtilen özellikleri uygula
        foreach (var tileInfo in mapData.tileMap)
        {
            if (tileInfo.index >= 0 && tileInfo.index < tileTypes.Count &&
                tileInfo.type >= 0 && tileInfo.type < mapData.tileTypes.Count)
            {
                if (System.Enum.TryParse(mapData.tileTypes[tileInfo.type], out TileType tileType))
                {
                    tileTypes[tileInfo.index] = tileType;
                }
                else
                {
                    Debug.LogError($"Geçersiz Tile Türü: {mapData.tileTypes[tileInfo.type]}");
                }
            }
            else
            {
                Debug.LogError($"Geçersiz Tile index veya tür indeksi: {tileInfo.index}, {tileInfo.type}");
            }
        }

        // TileController üzerinden türleri uygula
        tileController.ApplyTileProperties(tileTypes);
    }
}