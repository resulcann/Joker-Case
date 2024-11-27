using System.Collections.Generic;
using UnityEngine;

public class TileController : GenericSingleton<TileController>
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject innerTilePrefab; // InnerTile için prefab
    [SerializeField] private GameObject outerTilePrefab; // OuterTile için prefab
    [SerializeField] private Transform innerTilesParent; // InnerTile'ların bağlanacağı parent
    [SerializeField] private Transform outerTilesParent; // OuterTile'ların bağlanacağı parent
    
    [Space]
    [Header("SETTINGS")]
    [SerializeField, Tooltip("Tile türlerinin renkleri (JSON'dan eşleştirilecek)")] 
    private Color[] tileColors;

    private List<InnerTile> innerTiles = new List<InnerTile>();
    private List<OuterTile> outerTiles = new List<OuterTile>();
    

    public void GenerateTiles(MapData mapData)
    {
        innerTiles.Clear();
        outerTiles.Clear();

        GenerateInnerTiles(mapData);
        GenerateOuterTiles(mapData);
    }

    private void GenerateInnerTiles(MapData mapData)
    {
        int gridSize = mapData.grid.gridSize;
        float tileSize = mapData.grid.tileSize;

        float startX = -(gridSize / 2f) * tileSize + (tileSize / 2f);
        float startZ = -(gridSize / 2f) * tileSize + (tileSize / 2f);

        int totalTiles = gridSize * 2 + gridSize * 2 - 4;

        for (int i = 0; i < totalTiles; i++)
        {
            Vector3 position = CalculateTilePosition(i, gridSize, gridSize, startX, startZ, tileSize);
            var tile = Instantiate(innerTilePrefab, position, Quaternion.identity, innerTilesParent).GetComponent<InnerTile>();

            if (tile != null)
            {
                tile.SetType("Empty");
                tile.name = $"InnerTile_{i}";
                innerTiles.Add(tile);
            }
        }

        foreach (var tileInfo in mapData.innerTiles)
        {
            if (tileInfo.index >= 0 && tileInfo.index < innerTiles.Count)
            {
                innerTiles[tileInfo.index].SetType(tileInfo.type);
            }
        }
    }

    private void GenerateOuterTiles(MapData mapData)
    {
        int outerGridSize = mapData.grid.gridSize + 2; // Dış katman için grid size +2
        float tileSize = mapData.grid.tileSize;

        float startX = -(outerGridSize / 2f) * tileSize + (tileSize / 2f);
        float startZ = -(outerGridSize / 2f) * tileSize + (tileSize / 2f);

        int totalOuterTiles = outerGridSize * 2 + outerGridSize * 2 - 4;

        for (int i = 0; i < totalOuterTiles; i++)
        {
            Vector3 position = CalculateTilePosition(i, outerGridSize, outerGridSize, startX, startZ, tileSize);
            var tile = Instantiate(outerTilePrefab, position, Quaternion.identity, outerTilesParent).GetComponent<OuterTile>();

            if (tile != null)
            {
                tile.SetType("Empty"); // Varsayılan tür
                tile.name = $"OuterTile_{i}";
                outerTiles.Add(tile);
            }
        }

        foreach (var outerTileInfo in mapData.outerTiles)
        {
            if (outerTileInfo.index >= 0 && outerTileInfo.index < outerTiles.Count)
            {
                outerTiles[outerTileInfo.index].SetType(outerTileInfo.type);
            }
        }
    }

    private Vector3 CalculateTilePosition(int index, int rowCount, int columnCount, float startX, float startZ, float tileSize)
    {
        // Sol kenar
        if (index < columnCount)
        {
            return new Vector3(startX, 0, startZ + index * tileSize);
        }
        // Üst kenar
        else if (index < columnCount + rowCount - 1)
        {
            int relativeIndex = index - columnCount;
            return new Vector3(startX + (relativeIndex + 1) * tileSize, 0, startZ + (columnCount - 1) * tileSize);
        }
        // Sağ kenar
        else if (index < columnCount * 2 + rowCount - 2)
        {
            int relativeIndex = index - (columnCount + rowCount - 1);
            return new Vector3(startX + (rowCount - 1) * tileSize, 0, startZ + (columnCount - 2 - relativeIndex) * tileSize);
        }
        // Alt kenar
        else
        {
            int relativeIndex = index - (columnCount * 2 + rowCount - 2);
            return new Vector3(startX + (rowCount - 2 - relativeIndex) * tileSize, 0, startZ);
        }
    }

    public Color GetTileColorByType(string tileType)
    {
        int index = MapGenerator.Instance.GetMapData().innerTileTypes.IndexOf(tileType);
        if (index >= 0 && index < tileColors.Length)
        {
            return tileColors[index];
        }

        Debug.LogWarning($"Tile type not found: {tileType}");
        return Color.gray;
    }

    
    public List<InnerTile> GetInnerTiles() => innerTiles;
}
