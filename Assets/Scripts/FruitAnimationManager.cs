using System.Collections.Generic;
using UnityEngine;

public class FruitAnimationManager : GenericSingleton<FruitAnimationManager>
{
    [SerializeField] private FruitAnimation fruitAnimationPrefab; 
    [SerializeField] private Transform popupParent;

    private readonly Queue<FruitAnimation> _popupPool = new Queue<FruitAnimation>();

    public FruitAnimation GetPopup()
    {
        if (_popupPool.Count > 0)
        {
            var popup = _popupPool.Dequeue();
            popup.gameObject.SetActive(true);
            return popup;
        }

        // Havuzda yoksa yeni bir popup oluştur
        return Instantiate(fruitAnimationPrefab, popupParent);
    }

    public void ReturnToPool(FruitAnimation fruitAnimation)
    {
        fruitAnimation.gameObject.SetActive(false);
        _popupPool.Enqueue(fruitAnimation);
    }

    public void ShowPopup(string message, Sprite sprite, Vector3 position)
    {
        var popup = GetPopup();
        popup.Setup(message, sprite, position);
    }
}