using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Header("Dice Roll Settings")]
    [SerializeField] private float fallHeight = 5f; // zarların başlangıç yüksekliği
    [SerializeField] private float fallDuration = 1.5f; // zarların düşme süresi
    [SerializeField] private float spinSpeed = 720f; // zarların düşerken dönüş hızı

    private int _targetValue; // zarın hedef değeri
    private bool _hasStopped; // zar durdu mu?

    private readonly Quaternion[] _faceRotations = new Quaternion[]
    {
        Quaternion.Euler(-90, 0, 0),   // 1 (üst yüz)
        Quaternion.Euler(0, 0, 0),    // 2
        Quaternion.Euler(0, 0, -90),  // 3
        Quaternion.Euler(180, 0, -90),// 4
        Quaternion.Euler(180, 0, 0),  // 5
        Quaternion.Euler(90, 0, 0)    // 6
    };

    public void SetTargetValue(int value)
    {
        _targetValue = Mathf.Clamp(value, 1, 6); // 1 ile 6 arasında sınırla
    }

    public void Roll(Vector3 spawnPosition)
    {
        _hasStopped = false;
        StartCoroutine(RollCoroutine(spawnPosition));
    }

    private IEnumerator RollCoroutine(Vector3 spawnPosition)
    {
        var startPosition = spawnPosition + Vector3.up * fallHeight;
        var endPosition = spawnPosition + Vector3.up * 0.25f;
        transform.position = startPosition;

        // İlk random rotasyon
        transform.rotation = new Quaternion(Random.Range(15f,75f), Random.Range(15f,75f), Random.Range(15f,75f), Random.Range(15f,75f));

        float elapsedTime = 0f;

        // Düşerken rastgele rotasyon ve aşağı düşme işlemleri
        while (elapsedTime < fallDuration)
        {
            var t = elapsedTime / fallDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t); // yer çekimi ile beraber yere düşüş simüle ediliyor
            
            // Rastgele dönüş zar dönme efekti
            transform.Rotate(Vector3.right, spinSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endPosition; // Nihai pozisyon
        Quaternion targetRotation = _faceRotations[_targetValue - 1];
        elapsedTime = 0f;

        while (elapsedTime < 0.5f) // Yere ulaştığında hedef rotasyona geçiş
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        _hasStopped = true;
    }

    public bool HasStopped()
    {
        return _hasStopped;
    }
}
