using System.Collections.Generic;

[System.Serializable]
public class Position
{
    public float x; // Tile'ın X pozisyonu
    public float y; // Tile'ın Y pozisyonu
    public float z; // Tile'ın Z pozisyonu
}

[System.Serializable]
public class TileInfo
{
    public int index; // Tile'ın sırası
    public int type;  // Tile türü (tileTypes dizininde)
    public Position position; // Tile'ın pozisyonu
}

[System.Serializable]
public class MapData
{
    public List<string> tileTypes; // Tile türlerinin isimleri
    public List<TileInfo> tiles;  // Tüm Tile bilgileri
}