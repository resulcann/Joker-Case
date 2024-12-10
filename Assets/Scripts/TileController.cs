using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileController : GenericSingleton<TileController>
{
    [Header("REFERENCES")] [SerializeField]
    private GameObject innerTilePrefab;

    [SerializeField] private GameObject outerTilePrefab;
    [SerializeField] private Transform innerTilesParent;
    [SerializeField] private Transform outerTilesParent;

    [Space] [Header("Tile Settings")] 
    [SerializeField] private List<TileSettings> tileSettings;

    private readonly List<InnerTile> _innerTiles = new();
    private readonly List<OuterTile> _outerTiles = new();

    #region PROPERTIES

    public List<InnerTile> InnerTiles => _innerTiles;
    public List<OuterTile> OuterTiles => _outerTiles;
    public int OuterGridSize => MapGenerator.Instance.MapData.grid.gridSize + 2;
    public int InnerGridSize => MapGenerator.Instance.MapData.grid.gridSize;
    public Dictionary<string, TileData> TileSettings { get; private set; } = new Dictionary<string, TileData>();

    #endregion

    protected override void Awake()
    {
        InitializeTileSettings();
    }

    private void InitializeTileSettings()
    {
        TileSettings = new Dictionary<string, TileData>();
        foreach (var mapping in tileSettings)
        {
            if (!TileSettings.ContainsKey(mapping.tileType))
            {
                TileSettings.Add(mapping.tileType, new TileData
                {
                    sprite = mapping.sprite,
                    color = mapping.tileColor
                });
            }
        }
    }

    public void GenerateTiles(MapData mapData)
    {
        _innerTiles.Clear();
        _outerTiles.Clear();

        GenerateTilesByType<InnerTile, InnerTileInfo>(
            InnerGridSize,
            innerTilePrefab,
            innerTilesParent,
            InnerTiles,
            mapData.innerTiles,
            true
        );

        GenerateTilesByType<OuterTile, OuterTileInfo>(
            OuterGridSize,
            outerTilePrefab,
            outerTilesParent,
            OuterTiles,
            mapData.outerTiles,
            false
        );
    }


    private void GenerateTilesByType<TTile, TTileInfo>(
        int gridSize,
        GameObject tilePrefab,
        Transform parentTransform,
        List<TTile> tileList,
        List<TTileInfo> tileDataList,
        bool isInnerTile)
        where TTile : MonoBehaviour
        where TTileInfo : class
    {
        var tileSize = MapGenerator.Instance.MapData.grid.tileSize;
        var startX = -(gridSize / 2f) * tileSize + (tileSize / 2f);
        var startZ = -(gridSize / 2f) * tileSize + (tileSize / 2f);
        var totalTiles = gridSize * 2 + gridSize * 2 - 4;

        for (var i = 0; i < totalTiles; i++)
        {
            var position = CalculateTilePosition(i, gridSize, gridSize, startX, startZ, tileSize);
            var tile = Instantiate(tilePrefab, position, Quaternion.identity, parentTransform).GetComponent<TTile>();

            if (tile != null)
            {
                if (isInnerTile && tile is InnerTile innerTile)
                {
                    innerTile.SetTileValues("Empty");
                }
                else if (!isInnerTile && tile is OuterTile outerTile)
                {
                    outerTile.TileType = "Empty";
                }

                tile.name = $"{(isInnerTile ? "InnerTile" : "OuterTile")}_{i}";
                tileList.Add(tile);
            }
        }

        foreach (var tileInfo in tileDataList)
        {
            if (tileInfo is InnerTileInfo innerTileInfo && innerTileInfo.index >= 0 &&
                innerTileInfo.index < tileList.Count)
            {
                if (tileList[innerTileInfo.index] is InnerTile innerTile)
                {
                    innerTile.SetTileValues(innerTileInfo.type, innerTileInfo.amount);
                }
            }
            else if (tileInfo is OuterTileInfo outerTileInfo && outerTileInfo.index >= 0 &&
                     outerTileInfo.index < tileList.Count)
            {
                if (tileList[outerTileInfo.index] is OuterTile outerTile)
                {
                    outerTile.TileType = outerTileInfo.type;
                }
            }
        }
    }

    private Vector3 CalculateTilePosition(int index, int rowCount, int columnCount, float startX, float startZ,
        float tileSize)
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
            return new Vector3(startX + (rowCount - 1) * tileSize, 0,
                startZ + (columnCount - 2 - relativeIndex) * tileSize);
        }
        // bottom edge
        else
        {
            var relativeIndex = index - (columnCount * 2 + rowCount - 2);
            return new Vector3(startX + (rowCount - 2 - relativeIndex) * tileSize, 0, startZ);
        }
    }
}

[System.Serializable]
public class TileSettings
{
    public string tileType;
    public Sprite sprite;
    public Color tileColor;
}

[System.Serializable]
public class TileData
{
    public Sprite sprite;
    public Color color;
}
