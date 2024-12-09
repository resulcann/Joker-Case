using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FruitAnimationManager : GenericSingleton<FruitAnimationManager>
{
    [SerializeField] private FruitAnimation fruitAnimationPrefab; 
    [SerializeField] private Transform parentObject;

    private readonly Queue<FruitAnimation> _animationPool = new Queue<FruitAnimation>();

    public FruitAnimation GetFruitAnimation() // get available animation object.
    {
        if (_animationPool.Count > 0)
        {
            var fruitAnimation = _animationPool.Dequeue();
            fruitAnimation.gameObject.SetActive(true);
            return fruitAnimation;
        }

        // If there is not any available animation in pool, create new one.
        return Instantiate(fruitAnimationPrefab, parentObject);
    }

    public void ReturnToPool(FruitAnimation fruitAnimation) // returns the animation pool.
    {
        fruitAnimation.gameObject.SetActive(false);
        _animationPool.Enqueue(fruitAnimation);
    }

    public void ShowAnimation(string message, Sprite sprite, Vector3 position) // show animation with settings.
    {
        var fruitAnimation = GetFruitAnimation();
        fruitAnimation.Setup(message, sprite, position);
    }
}