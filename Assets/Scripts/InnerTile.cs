using TMPro;
using UnityEngine;

public class InnerTile : MonoBehaviour, ITile
{
    [SerializeField] private Renderer _tileRenderer;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro amountText;
    
    private MaterialPropertyBlock _propertyBlock;

    #region PROPERTIES

    public int Amount { get; set; } = 0;
    public string TileType { get; set; } = "";
    public Sprite TileSprite => TileController.Instance.TileSettings[TileType].sprite;
    public Color TileColor => TileController.Instance.TileSettings[TileType].color;

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
    
    public void UpdateVisual()
    {
        _tileRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor("_Color", TileColor);
        _tileRenderer.SetPropertyBlock(_propertyBlock);
        
        _spriteRenderer.sprite = TileSprite;
        UpdateAmountText();
    }

    private void UpdateAmountText()
    {
        amountText.text = GameUtility.FormatNumber(Amount);
        amountText.gameObject.SetActive(Amount > 0);
    }
}