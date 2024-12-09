using System.Collections.Generic;

[System.Serializable]
public class GridInfo
{
    public float tileSize;  // Tile size
    public int gridSize;    // Grid size (Row X Column) (NxN)
}

[System.Serializable]
public class InnerTileInfo
{
    public int index;  // index in list
    public string type; // tile type
    public string amount; // amount at tile
}

[System.Serializable]
public class OuterTileInfo
{
    public int index; // index in list
    public string type; // tile type
}

[System.Serializable]
public class MapData
{
    public GridInfo grid;
    public List<string> innerTileTypes;
    public List<string> outerTileTypes;
    public List<InnerTileInfo> innerTiles; 
    public List<OuterTileInfo> outerTiles;
}