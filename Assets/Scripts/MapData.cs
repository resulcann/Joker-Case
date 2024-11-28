using System.Collections.Generic;

[System.Serializable]
public class GridInfo
{
    public float tileSize;  // Her bir Tile'ın boyutu
    public int gridSize;    // Satır,sütun sayısı (NxN)
}

[System.Serializable]
public class InnerTileInfo
{
    public int index;  // Inner tile'ın sırası
    public string type; // Inner tile'ın türü
    public string amount; // Tile'daki miktar
}

[System.Serializable]
public class OuterTileInfo
{
    public int index; // Outer tile'ın sırası
    public string type; // Outer tile'ın türü
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