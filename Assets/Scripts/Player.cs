using System;
using System.Collections;
using UnityEngine;

public class Player : GenericSingleton<Player>
{
    [Header("REFERENCES")]
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [Space]
    [Header("MOVE SETTINGS")]
    [SerializeField] private float moveSpeed = 5f; // Hareket hızı
    [SerializeField] private float rotationSpeed = 5f; // Dönüş hızı
    
    private int _currentTileIndex = 0; // Oyuncunun bulunduğu Tile indeksi
    private bool _isMoving = false; // Hareket halinde olup olmadığını kontrol eder
    private int _totalStepsToMove = 0; // Karakterin toplam hareket edeceği adım sayısı

    /// <summary>
    /// Oyuncuyu başlatır ve başlangıç pozisyonuna yerleştirir.
    /// </summary>
    public void Init()
    {
        var tiles = TileController.Instance.GetInnerTiles();
        if (tiles != null && tiles.Count > 0)
        {
            var startTile = tiles[0].transform; // 0. Tile
            transform.position = startTile.position; // Oyuncu başlangıç pozisyonuna yerleştiriliyor
            LookAtNextTile(); // Bir sonraki Tile'a rotasyon döndürülüyor
        }
        
        playerAnimationController.PlayIdleAnimation();
    }

    void Update()
    {
        // TEST İÇİN KLAVYE İLE HAREKET
        if (Input.GetKeyDown(KeyCode.Space) && !_isMoving)
        {
            AddSteps(1); // Space tuşuna basıldığında toplam adımlara +1 ekle
        }
        if (Input.GetKeyDown(KeyCode.S) && !_isMoving)
        {
            AddSteps(3); // S tuşuna basıldığında toplam adımlara +3 ekle
        }
    }

    /// <summary>
    /// Hareket etmek için toplam adım sayısına ekleme yapar.
    /// </summary>
    /// <param name="stepsToAdd">Eklenmek istenen adım sayısı</param>
    public void AddSteps(int stepsToAdd)
    {
        _totalStepsToMove += stepsToAdd;

        if (!_isMoving) // Karakter hareket etmiyorsa hemen harekete geç
        {
            StartCoroutine(MoveThroughSteps());
        }
    }

    /// <summary>
    /// Oyuncunun hareket etmesi için gerekli coroutine.
    /// </summary>
    private IEnumerator MoveThroughSteps()
    {
        var tiles = TileController.Instance.GetInnerTiles();
        _isMoving = true;

        while (_totalStepsToMove > 0)
        {
            // Bir sonraki Tile'a geç
            _currentTileIndex = (_currentTileIndex + 1) % tiles.Count;

            // Oyuncuyu sıradaki Tile'a hareket ettir
            yield return StartCoroutine(MoveToTile(tiles[_currentTileIndex].transform.position));

            // Gidilecek toplam adım sayısını azalt
            _totalStepsToMove--;
        }

        _isMoving = false;
    }

    /// <summary>
    /// Oyuncuyu belirtilen pozisyona yumuşakça hareket ettirir.
    /// </summary>
    /// <param name="targetPosition">Hedef Tile'ın pozisyonu</param>
    /// <returns></returns>
    private IEnumerator MoveToTile(Vector3 targetPosition)
    {
        // Hedef pozisyona kadar hareket et
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition; // Kesin pozisyonu ayarla
        LookAtNextTile(); // Bir sonraki Tile'a dönük ol
    }

    /// <summary>
    /// Oyuncuyu bir sonraki Tile'a doğru döndürür.
    /// </summary>
    private void LookAtNextTile()
    {
        var tiles = TileController.Instance.GetInnerTiles();

        if (tiles != null && tiles.Count > 0)
        {
            var nextIndex = (_currentTileIndex + 1) % tiles.Count; // Bir sonraki Tile'ın indeksi
            var direction = (tiles[nextIndex].transform.position - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
