using System;
using System.Collections;
using System.Collections.Generic;
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
    public int moveValue = 3;

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
        if (Input.GetKeyDown(KeyCode.Space) && !_isMoving)
        {
            MoveToTileBySteps(1); // Space tuşuna basıldığında bir sonraki Tile'a hareket et
        }
        if (Input.GetKeyDown(KeyCode.S) && !_isMoving)
        {
            MoveToTileBySteps(moveValue); // S tuşuna basıldığında moveValue kadar hareket et
        }
    }

    /// <summary>
    /// Oyuncuyu belirtilen adım sayısı kadar hareket ettirir. 
    /// Tur atma mantığını da içerir.
    /// </summary>
    /// <param name="stepCount">Hareket edilecek adım sayısı</param>
    public void MoveToTileBySteps(int stepCount)
    {
        var tiles = TileController.Instance.GetInnerTiles();

        if (tiles != null && tiles.Count > 0 && stepCount > 0)
        {
            StartCoroutine(MoveThroughSteps(stepCount));
        }
    }

    /// <summary>
    /// Oyuncuyu mevcut konumundan belirtilen adım sayısı kadar hareket ettirir.
    /// </summary>
    /// <param name="stepsToMove">Hareket edilecek toplam adım sayısı</param>
    /// <param name="tiles">Tile listesi</param>
    private IEnumerator MoveThroughSteps(int stepsToMove)
    {
        var tiles = TileController.Instance.GetInnerTiles();
        
        while (stepsToMove > 0)
        {
            // Bir sonraki Tile'a geç
            _currentTileIndex = (_currentTileIndex + 1) % tiles.Count;

            // Oyuncuyu sıradaki Tile'a hareket ettir
            yield return StartCoroutine(MoveToTile(tiles[_currentTileIndex].transform.position));

            // Gidilecek adım sayısını azalt
            stepsToMove--;
        }
    }

    /// <summary>
    /// Oyuncuyu belirtilen pozisyona yumuşakça hareket ettirir.
    /// </summary>
    /// <param name="targetPosition">Hedef Tile'ın pozisyonu</param>
    /// <returns></returns>
    private IEnumerator MoveToTile(Vector3 targetPosition)
    {
        _isMoving = true;

        // Hedef pozisyona kadar hareket et
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition; // Kesin pozisyona yerleştir
        LookAtNextTile(); // Bir sonraki Tile'a dönük ol
        _isMoving = false;
    }

    /// <summary>
    /// Oyuncuyu bir sonraki Tile'a doğru döndürür.
    /// </summary>
    private void LookAtNextTile()
    {
        var tiles = TileController.Instance.GetInnerTiles();

        if (tiles != null && tiles.Count > 0)
        {
            int nextIndex = (_currentTileIndex + 1) % tiles.Count; // Bir sonraki Tile'ın indeksi
            Vector3 direction = (tiles[nextIndex].transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
