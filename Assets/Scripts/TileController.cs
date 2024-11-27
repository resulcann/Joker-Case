using System.Collections.Generic;
using UnityEngine;

public class TileController : GenericSingleton<TileController>
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject tilePrefab; // Tile için prefab
    [SerializeField] private Transform tilesParent; // Tile'ların bağlanacağı parent transform
    [SerializeField] private List<Tile> tileList = new List<Tile>(); // Tüm Tile'ları saklar
    [Space]
    [Header("SETTINGS")]
    [SerializeField, Tooltip("Tile türlerinin renkleri (JSON'dan eşleştirilecek)")] 
    private Color[] tileColors;

    public void GenerateTiles(MapData mapData)
    {
        tileList.Clear();

        var grid = mapData.grid;
        int gridSize = grid.gridSize;
        float tileSize = grid.tileSize;

        // Dinamik başlangıç pozisyonlarını hesapla
        float startX = -(gridSize / 2f) * tileSize + (tileSize / 2f);
        float startZ = -(gridSize / 2f) * tileSize + (tileSize / 2f);

        // Toplam Tile sayısını hesapla
        int totalTiles = gridSize * 2 + gridSize * 2 - 4;

        for (int i = 0; i < totalTiles; i++)
        {
            Vector3 position = CalculateTilePosition(i, gridSize, gridSize, startX, startZ, tileSize);

            // Tile'ı oluştur
            var tile = Instantiate(tilePrefab, position, Quaternion.identity, tilesParent).GetComponent<Tile>();
            
            if (tile != null)
            {
                tile.SetType("Empty"); // Varsayılan tür
                tile.name = $"Tile_{i}";
                tileList.Add(tile);
            }
        }

        // JSON'da belirtilen özel türleri uygula
        foreach (var tileInfo in mapData.tiles)
        {
            if (tileInfo.index >= 0 && tileInfo.index < tileList.Count)
            {
                string tileType = tileInfo.type; // Doğrudan tür adı
                Tile tile = tileList[tileInfo.index];
                tile.SetType(tileType);
            }
        }
    }

    private Vector3 CalculateTilePosition(int index, int rowCount, int columnCount, float startX, float startZ, float tileSize)
    {
        // Sol kenar (aşağıdan yukarıya, sol alt köşeden sol üst köşeye kadar)
        if (index < columnCount)
        {
            return new Vector3(startX, 0, startZ + index * tileSize);
        }
        // Üst kenar (soldan sağa, sol üst köşeden sağ üst köşeye kadar)
        else if (index < columnCount + rowCount - 1)
        {
            int relativeIndex = index - columnCount;
            return new Vector3(startX + (relativeIndex + 1) * tileSize, 0, startZ + (columnCount - 1) * tileSize);
        }
        // Sağ kenar (yukarıdan aşağıya, sağ üst köşeden sağ alt köşeye kadar)
        else if (index < columnCount * 2 + rowCount - 2)
        {
            int relativeIndex = index - (columnCount + rowCount - 1);
            return new Vector3(startX + (rowCount - 1) * tileSize, 0, startZ + (columnCount - 2 - relativeIndex) * tileSize);
        }
        // Alt kenar (sağdan sola, sağ alt köşeden sol alt köşeye kadar)
        else
        {
            int relativeIndex = index - (columnCount * 2 + rowCount - 2);
            return new Vector3(startX + (rowCount - 2 - relativeIndex) * tileSize, 0, startZ);
        }
    }

    public Color GetTileColorByType(string tileType)
    {
        int index = MapGenerator.Instance.GetMapData().tileTypes.IndexOf(tileType);
        if (index >= 0 && index < tileColors.Length)
        {
            return tileColors[index];
        }

        Debug.LogWarning($"Tile type not found: {tileType}");
        return Color.gray; // Varsayılan renk
    }
    
    public List<Tile> GetTiles() => tileList;
}
