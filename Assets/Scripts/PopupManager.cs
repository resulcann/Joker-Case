using System.Collections.Generic;
using UnityEngine;

public class PopupManager : GenericSingleton<PopupManager>
{
    [SerializeField] private Popup popupPrefab; 
    [SerializeField] private Transform popupParent;

    private readonly Queue<Popup> _popupPool = new Queue<Popup>();

    public Popup GetPopup()
    {
        if (_popupPool.Count > 0)
        {
            var popup = _popupPool.Dequeue();
            popup.gameObject.SetActive(true);
            return popup;
        }

        // Havuzda yoksa yeni bir popup oluştur
        return Instantiate(popupPrefab, popupParent);
    }

    public void ReturnToPool(Popup popup)
    {
        popup.gameObject.SetActive(false);
        _popupPool.Enqueue(popup);
    }

    public void ShowPopup(string message, Sprite sprite, Vector3 position)
    {
        var popup = GetPopup();
        popup.Setup(message, sprite, position);
    }
}