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
            TileType.Empty => Color.gray,
            TileType.Apple => Color.red,
            TileType.Pear => Color.green,
            TileType.Strawberry => Color.magenta,
            _ => _tileRenderer.material.color
        };
    }
}