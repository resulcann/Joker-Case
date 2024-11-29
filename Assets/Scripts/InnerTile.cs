using TMPro;
using UnityEngine;

public class InnerTile : MonoBehaviour
{
    [SerializeField] private Renderer _tileRenderer;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro amountText;
    
    private MaterialPropertyBlock _propertyBlock;
    private string _amount = "";
    private string _type;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
    }

    public void SetType(string type, string amount = "")
    {
        _type = type;
        _amount = amount;

        UpdateVisual();
    }
    
    private void UpdateVisual()
    {
        var color = TileController.Instance.GetTileColorByType(_type);

        _tileRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor("_Color", color);
        _tileRenderer.SetPropertyBlock(_propertyBlock);

        // Sprite ataması
        var spriteName = _type switch
        {
            "Start" => "StartSprite",
            "Apple" => "AppleSprite",
            "Pear" => "PearSprite",
            "Strawberry" => "StrawberrySprite",
            _ => null
        };

        if (_type == "Start")
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
        amountText.text = !string.IsNullOrEmpty(_amount) ? _amount : "";
        amountText.gameObject.SetActive(!string.IsNullOrEmpty(_amount));
    }
    
    public string GetTileType() => _type;
    public int GetTileAmount()
    {
        return int.TryParse(_amount, out var result) ? result : 0;
    }

}