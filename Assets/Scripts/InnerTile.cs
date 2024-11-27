using UnityEngine;

public class InnerTile : MonoBehaviour
{
    [SerializeField] private Renderer _tileRenderer;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private MaterialPropertyBlock _propertyBlock;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
    }

    public void SetType(string type)
    {
        UpdateVisual(type);
    }
    
    private void UpdateVisual(string type)
    {
        // Renk ataması
        Color color = TileController.Instance.GetTileColorByType(type);
        _tileRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor("_Color", color);
        _tileRenderer.SetPropertyBlock(_propertyBlock);

        // Sprite ataması
        string spriteName = type switch
        {
            "Start" => "StartSprite",
            "Apple" => "AppleSprite",
            "Pear" => "PearSprite",
            "Strawberry" => "StrawberrySprite",
            _ => null
        };

        _spriteRenderer.sprite = spriteName != null ? Resources.Load<Sprite>($"Sprites/{spriteName}") : null;
    }
}