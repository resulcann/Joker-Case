using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType type; // Tile'ın türü (özellik)
    [SerializeField] private Renderer _tileRenderer;

    public void SetType(TileType newType)
    {
        type = newType;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _tileRenderer.material.color = type switch
        {
            TileType.Empty => Color.white,
            TileType.Apple => Color.red,
            TileType.Pear => Color.green,
            TileType.Strawberry => Color.magenta,
            TileType.Start => Color.yellow,
            _ => _tileRenderer.material.color
        };
    }
}