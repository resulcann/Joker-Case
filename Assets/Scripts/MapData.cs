using System.Collections.Generic;
using UnityEngine.Serialization;

[System.Serializable]
public class GridInfo
{
    public float startX;    // Başlangıç X pozisyonu
    public float startZ;    // Başlangıç Z pozisyonu
    public float tileSize;  // Her bir Tile'ın boyutu
    public int rowCount;    // Satır sayısı
    public int columnCount; // Sütun sayısı
}

[System.Serializable]
public class TileInfo
{
    public int index; // Tile'ın sırası
    public int type;  // Tile türü (tileTypes dizininde)
}

[System.Serializable]
public class MapData
{
    public List<string> tileTypes; // Tile türlerinin isimleri
    public GridInfo grid;          // Grid bilgileri
    public List<TileInfo> tiles;   // Özel türdeki Tile'ların bilgileri
}