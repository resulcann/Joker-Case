using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum TileType { Start, Empty, Apple, Pear, Strawberry }

public class TileController : GenericSingleton<TileController>
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject tilePrefab; // Tile için prefab
    [SerializeField] private Transform tilesParent; // Tile'ların bağlanacağı parent transform
    [SerializeField] private List<Tile> tileList = new List<Tile>(); // Tüm Tile'ları saklar
    [Space] [Header("SETTINGS")]
    [SerializeField, Tooltip("0: Start, 1: Empty, 2: Apple, 3: Pear, 4: Strawberry")] 
    private Color[] tileColors;
    
    public void GenerateTiles(MapData mapData)
    {
        tileList.Clear();

        var grid = mapData.grid;
        int rowCount = grid.rowCount;
        int columnCount = grid.columnCount;
        float startX = grid.startX;
        float startZ = grid.startZ;
        float tileSize = grid.tileSize;

        // Toplam Tile sayısını hesapla
        int totalTiles = rowCount * 2 + columnCount * 2 - 4;

        for (int i = 0; i < totalTiles; i++)
        {
            Vector3 position = CalculateTilePosition(i, rowCount, columnCount, startX, startZ, tileSize);

            // Tile'ı oluştur
            var tile = Instantiate(tilePrefab, position, Quaternion.identity, tilesParent).GetComponent<Tile>();
            
            if (tile != null)
            {
                tile.SetType(TileType.Empty); // Varsayılan tür
                tile.name = $"Tile_{i}";
                tileList.Add(tile);
            }
            
        }

        // JSON'da belirtilen özel türleri uygula
        foreach (var tileInfo in mapData.tiles)
        {
            if (tileInfo.index >= 0 && tileInfo.index < tileList.Count)
            {
                if (System.Enum.TryParse(mapData.tileTypes[tileInfo.type], out TileType tileType))
                {
                    Tile tile = tileList[tileInfo.index].GetComponent<Tile>();
                    tile.SetType(tileType);
                }
            }
        }
    }

    private Vector3 CalculateTilePosition(int index, int columnCount, int rowCount, float startX, float startZ, float tileSize)
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

    public Color GetTileColorByType(TileType tileType) => tileColors[(int)tileType];
    public List<Tile> GetTiles() => tileList;
}
