using TMPro;
using UnityEngine;
using UnityEngine.WSA;

public class InnerTile : MonoBehaviour
{
    [SerializeField] private Renderer _tileRenderer;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro amountText;
    
    private MaterialPropertyBlock _propertyBlock;

    #region PROPERTIES

    public int Amount { get; set; } = 0;
    public string TileType { get; set; } = "";

    #endregion
    

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
    }

    public void SetTileValues(string type, string amount = "")
    {
        TileType = type;
        Amount = int.TryParse(amount, out var result) ? result : 0;

        UpdateVisual();
    }
    
    private void UpdateVisual()
    {
        var color = TileController.Instance.GetTileColorByType(TileType);

        _tileRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor("_Color", color);
        _tileRenderer.SetPropertyBlock(_propertyBlock);

        // Sprite ataması
        var spriteName = TileType switch
        {
            "Start" => "StartSprite",
            "Apple" => "AppleSprite",
            "Pear" => "PearSprite",
            "Strawberry" => "StrawberrySprite",
            _ => null
        };

        if (TileType == "Start")
        {
            var srTransform = _spriteRenderer.transform;
            srTransform.localPosition = new Vector3(srTransform.localPosition.x, 0.0101f, 0);
            srTransform.localScale = Vector3.one * 0.0025f;
        }

        _spriteRenderer.sprite = spriteName != null ? Resources.Load<Sprite>($"Sprites/{spriteName}") : null;
        UpdateAmountText();
    }

    private void UpdateAmountText()
    {
        amountText.text = GameUtilities.Instance.FormatNumber(Amount);
        amountText.gameObject.SetActive(Amount > 0);
    }
}