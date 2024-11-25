using System.Collections.Generic;
using UnityEngine;

public enum TileType { Empty, Apple, Pear, Strawberry } // Tile türleri
public class TileController : MonoBehaviour
{
    [SerializeField] private List<Tile> tiles = new List<Tile>(); // Tüm Tile'ları saklar

    public void ApplyTileProperties(List<TileType> types)
    {
        for (int i = 0; i < tiles.Count && i < types.Count; i++)
        {
            tiles[i].SetType(types[i]);
        }
    }
}