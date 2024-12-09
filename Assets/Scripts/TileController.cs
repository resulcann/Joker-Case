using System.Collections.Generic;
using UnityEngine;

public class TileController : GenericSingleton<TileController>
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject innerTilePrefab;
    [SerializeField] private GameObject outerTilePrefab;
    [SerializeField] private Transform innerTilesParent;
    [SerializeField] private Transform outerTilesParent; 
    
    [Space]
    [Header("SETTINGS")]
    [SerializeField, Tooltip("0 = Empty, 1 = Start, 2 = Apple, 3 = Pear, 4 = Strawberry")] 
    private Color[] tileColors;

    private readonly List<InnerTile> _innerTiles = new();
    private readonly List<OuterTile> _outerTiles = new();
    

    public void GenerateTiles(MapData mapData)
    {
        _innerTiles.Clear();
        _outerTiles.Clear();

        GenerateInnerTiles(mapData);
        GenerateOuterTiles(mapData);
    }

    private void GenerateInnerTiles(MapData mapData)
    {
        var gridSize = mapData.grid.gridSize;
        var tileSize = mapData.grid.tileSize;

        var startX = -(gridSize / 2f) * tileSize + (tileSize / 2f);
        var startZ = -(gridSize / 2f) * tileSize + (tileSize / 2f);

        var totalTiles = gridSize * 2 + gridSize * 2 - 4;

        for (int i = 0; i < totalTiles; i++)
        {
            var position = CalculateTilePosition(i, gridSize, gridSize, startX, startZ, tileSize);
            var tile = Instantiate(innerTilePrefab, position, Quaternion.identity, innerTilesParent).GetComponent<InnerTile>();

            if (tile != null)
            {
                tile.SetTileValues("Empty");
                tile.name = $"InnerTile_{i}";
                _innerTiles.Add(tile);
            }
        }

        foreach (var tileInfo in mapData.innerTiles)
        {
            if (tileInfo.index >= 0 && tileInfo.index < _innerTiles.Count)
            {
                _innerTiles[tileInfo.index].SetTileValues(tileInfo.type, tileInfo.amount);
            }
        }
    }

    private void GenerateOuterTiles(MapData mapData)
    {
        var outerGridSize = mapData.grid.gridSize + 2; // For outer layer grid size +2
        var tileSize = mapData.grid.tileSize;

        var startX = -(outerGridSize / 2f) * tileSize + (tileSize / 2f);
        var startZ = -(outerGridSize / 2f) * tileSize + (tileSize / 2f);

        var totalOuterTiles = outerGridSize * 2 + outerGridSize * 2 - 4;

        for (var i = 0; i < totalOuterTiles; i++)
        {
            var position = CalculateTilePosition(i, outerGridSize, outerGridSize, startX, startZ, tileSize);
            var tile = Instantiate(outerTilePrefab, position, Quaternion.identity, outerTilesParent).GetComponent<OuterTile>();

            if (tile != null)
            {
                tile.Type = "Empty"; // default value set to empty
                tile.name = $"OuterTile_{i}";
                _outerTiles.Add(tile);
            }
        }

        foreach (var outerTileInfo in mapData.outerTiles)
        {
            if (outerTileInfo.index >= 0 && outerTileInfo.index < _outerTiles.Count)
            {
                _outerTiles[outerTileInfo.index].Type = outerTileInfo.type;
            }
        }
    }

    private Vector3 CalculateTilePosition(int index, int rowCount, int columnCount, float startX, float startZ, float tileSize)
    {
        // left edge
        if (index < columnCount)
        {
            return new Vector3(startX, 0, startZ + index * tileSize);
        }
        // top edge
        if (index < columnCount + rowCount - 1)
        {
            var relativeIndex = index - columnCount;
            return new Vector3(startX + (relativeIndex + 1) * tileSize, 0, startZ + (columnCount - 1) * tileSize);
        }
        // right edge
        if (index < columnCount * 2 + rowCount - 2)
        {
            var relativeIndex = index - (columnCount + rowCount - 1);
            return new Vector3(startX + (rowCount - 1) * tileSize, 0, startZ + (columnCount - 2 - relativeIndex) * tileSize);
        }
        // bottom edge
        else
        {
            var relativeIndex = index - (columnCount * 2 + rowCount - 2);
            return new Vector3(startX + (rowCount - 2 - relativeIndex) * tileSize, 0, startZ);
        }
    }

    public Color GetTileColorByType(string tileType)
    {
        var index = MapGenerator.Instance.MapData.innerTileTypes.IndexOf(tileType);
        
        if (index >= 0 && index < tileColors.Length)
        {
            return tileColors[index];
        }

        throw new System.NullReferenceException($"Tile type not found: {tileType}");
    }

    
    public List<InnerTile> GetInnerTiles() => _innerTiles;
    public Sprite GetTileSprite(string tileType)
    {
        return tileType switch
        {
            "Apple" => Resources.Load<Sprite>($"Sprites/AppleSprite"),
            "Pear" => Resources.Load<Sprite>($"Sprites/PearSprite"),
            "Strawberry" => Resources.Load<Sprite>($"Sprites/StrawberrySprite"),
            _ => null
        };
    }


}
