using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType tileType; // Tile'ın türü (özellik)
    [SerializeField] private Renderer _tileRenderer; // Renk için kullanılan renderer
    [SerializeField] private SpriteRenderer _spriteRenderer; // Alt obje üzerindeki SpriteRenderer
    private MaterialPropertyBlock _propertyBlock;
    
    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
    }
    public void SetType(TileType newType)
    {
        tileType = newType;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _tileRenderer.GetPropertyBlock(_propertyBlock); // Mevcut blok değerlerini al
        _propertyBlock.SetColor("_Color", TileController.Instance.GetTileColorByType(tileType)); // Rengi ayarla
        _tileRenderer.SetPropertyBlock(_propertyBlock); // Güncellenmiş bloğu uygula

        // Sprite güncelleme
        string spriteName = tileType switch
        {
            TileType.Start => "StartSprite",
            TileType.Apple => "AppleSprite",
            TileType.Pear => "PearSprite",
            TileType.Strawberry => "StrawberrySprite",
            _ => null // Empty veya bilinmeyen türler için null
        };

        _spriteRenderer.sprite = spriteName != null ? Resources.Load<Sprite>($"Sprites/{spriteName}") : null;
    }
}