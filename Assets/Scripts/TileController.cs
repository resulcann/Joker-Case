using System.Collections.Generic;
using UnityEngine;

public enum TileType { Start, Empty, Apple, Pear, Strawberry }

public class TileController : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab; // Tile için prefab
    [SerializeField] private Transform tilesParent; // Tile'ların bağlanacağı parent transform
    [SerializeField] private List<GameObject> tiles = new List<GameObject>(); // Tüm Tile'ları saklar

    public void GenerateTiles(MapData mapData)
    {
        tiles.Clear();

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
            GameObject tileObject = Instantiate(tilePrefab, position, Quaternion.identity, tilesParent);
            Tile tile = tileObject.GetComponent<Tile>();
            if (tile == null)
            {
                tile = tileObject.AddComponent<Tile>();
            }
            tile.SetType(TileType.Empty); // Varsayılan tür
            tile.name = $"Tile_{i}";

            tiles.Add(tileObject);
        }

        // JSON'da belirtilen özel türleri uygula
        foreach (var tileInfo in mapData.tiles)
        {
            if (tileInfo.index >= 0 && tileInfo.index < tiles.Count)
            {
                if (System.Enum.TryParse(mapData.tileTypes[tileInfo.type], out TileType tileType))
                {
                    Tile tile = tiles[tileInfo.index].GetComponent<Tile>();
                    tile.SetType(tileType);
                }
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
}
