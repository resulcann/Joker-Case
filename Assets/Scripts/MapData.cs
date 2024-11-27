using System.Collections.Generic;

[System.Serializable]
public class GridInfo
{
    public float tileSize;  // Her bir Tile'ın boyutu
    public int gridSize;    // Satır sayısı
}

[System.Serializable]
public class InnerTileInfo
{
    public int index;  // Tile'ın sırası
    public string type; // Tile türü (örneğin: "Start", "Apple")
}

[System.Serializable]
public class OuterTileInfo
{
    public int index; // Outer tile'ın sırası
    public string type; // Outer tile'ın türü (ör: "Skyscraper", "JapaneseTower")
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