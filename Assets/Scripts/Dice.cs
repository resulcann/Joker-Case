using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Header("Dice Roll Settings")]
    [SerializeField] private float fallHeight = 5f; // starting height for the dice
    [SerializeField] private float fallDuration = 1.5f; // time it takes for the dice to fall
    [SerializeField] private float spinSpeed = 720f; // rotation speed while falling
    
    private int _targetValue; // the target value for the dice
    public event System.Action OnDiceStopped;

    private readonly Quaternion[] _faceRotations = new Quaternion[]
    {
        Quaternion.Euler(-90, 0, 0),   // 1
        Quaternion.Euler(0, 0, 0),    // 2
        Quaternion.Euler(0, 0, -90),  // 3
        Quaternion.Euler(180, 0, -90),// 4
        Quaternion.Euler(180, 0, 0),  // 5
        Quaternion.Euler(90, 0, 0)    // 6
    };

    public void SetTargetValue(int value)
    {
        var diceController = DiceController.Instance;
        _targetValue = Mathf.Clamp(value, diceController.MinDiceValue, diceController.MaxDiceValue);
    }

    public void Roll(Vector3 spawnPosition)
    {
        StartCoroutine(RollCoroutine(spawnPosition));
    }

    private IEnumerator RollCoroutine(Vector3 spawnPosition)
    {
        var startPosition = spawnPosition + Vector3.up * fallHeight;
        var endPosition = spawnPosition + Vector3.up * 0.25f;
        transform.position = startPosition;

        // Initial random rotation
        transform.rotation = new Quaternion(Random.Range(15f, 75f), Random.Range(15f, 75f), Random.Range(15f, 75f), Random.Range(15f, 75f));

        var elapsedTime = 0f;

        // Falling with random rotation
        while (elapsedTime < fallDuration)
        {
            var t = elapsedTime / fallDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t); // simulates falling due to gravity
            
            // Random spin effect while falling
            transform.Rotate(Vector3.right, spinSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endPosition; // final position
        var targetRotation = _faceRotations[_targetValue - 1];
        elapsedTime = 0f;

        while (elapsedTime < 0.5f) // adjust to target rotation once it lands
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        OnDiceStopped?.Invoke();
    }
}
