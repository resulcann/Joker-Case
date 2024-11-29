using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private float lifeTime = 1f;  // Popup'ın ekranda kalma süresi

    private RectTransform _rectTransform;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private float _elapsedTime;
    private Camera _mainCamera;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _mainCamera = Camera.main;
    }

    public void Setup(string message, Sprite sprite, Vector3 worldPosition, float yOffset = 50f)
    {
        text.text = message;
        image.sprite = sprite;

        // Dünya pozisyonunu Canvas pozisyonuna dönüştür (canvas renderer modum camera)
        var screenPosition = _mainCamera.WorldToScreenPoint(worldPosition); // Dünya -> Ekran pozisyonu
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent.GetComponent<RectTransform>(),
            screenPosition,
            _mainCamera,
            out _startPosition
        );
        
        _endPosition = _startPosition + Vector2.up * yOffset;
        _rectTransform.anchoredPosition = _startPosition;

        _elapsedTime = 0f;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        var progress = _elapsedTime / lifeTime;
        _rectTransform.anchoredPosition = Vector2.Lerp(_startPosition, _endPosition, progress);

        // Alfa değerini azaltarak kaybolma efekti
        var color = text.color;
        color.a = Mathf.Lerp(1f, 0f, progress);
        text.color = color;
        image.color = color;
        
        if (_elapsedTime >= lifeTime)
        {
            PopupManager.Instance.ReturnToPool(this);
        }
    }
}
