using System.Collections.Generic;
using UnityEngine.Serialization;

[System.Serializable]
public class GridInfo
{
    public float tileSize;  // Her bir Tile'ın boyutu
    public int gridSize;    // Satır sayısı
}

[System.Serializable]
public class TileInfo
{
    public int index;  // Tile'ın sırası
    public string type; // Tile türü (örneğin: "Start", "Apple")
}

[System.Serializable]
public class MapData
{
    public List<string> tileTypes; // Tile türlerinin isimleri
    public GridInfo grid;          // Grid bilgileri
    public List<TileInfo> tiles;   // Özel türdeki Tile'ların bilgileri
}