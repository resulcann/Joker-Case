using System.Collections.Generic;

[System.Serializable]
public class TileInfo
{
    public int index; // Tile'ın pozisyonu
    public int type;  // Tile türü (tileTypes dizininde)
}

[System.Serializable]
public class MapData
{
    public List<string> tileTypes; // Tile türlerinin isimleri
    public List<TileInfo> tileMap; // Her bir Tile'ın bilgileri
}