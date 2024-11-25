using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum TileType { Empty, Apple, Pear, Strawberry, Start}

public class TileController : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefab; // Tile için prefab
    [SerializeField] private Transform _tilesParent; // Tilelar'ın parent'ı
    [SerializeField] private List<Tile> _tiles = new List<Tile>();    // Tüm Tile'ları saklar

    public void GenerateTiles(MapData mapData)
    {
        foreach (var tileInfo in mapData.tiles)
        {
            // Tile'ın pozisyonunu belirle
            Vector3 position = new Vector3(tileInfo.position.x, tileInfo.position.y, tileInfo.position.z);

            // Tile'ı oluştur ve özelliklerini ayarla
            var tile = Instantiate(_tilePrefab, position, Quaternion.identity, _tilesParent).GetComponent<Tile>();
            tile.name = $"Tile_{tileInfo.index}";

            // Tile türünü ve diğer özelliklerini ayarla
            if (System.Enum.TryParse(mapData.tileTypes[tileInfo.type], out TileType tileType))
            {
                tile.SetType(tileType);
            }

            _tiles.Add(tile);
        }
    }
}