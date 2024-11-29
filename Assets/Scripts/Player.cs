using System;
using System.Collections;
using UnityEngine;

public class Player : GenericSingleton<Player>
{
    [Header("REFERENCES")]
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [Space]
    [Header("MOVE SETTINGS")]
    [SerializeField] private float moveSpeed = 5f; // hareket hızı
    [SerializeField] private float rotationSpeed = 5f; // dönüş hızı
    
    private int _currentTileIndex = 0; // oyuncunun bulunduğu Tile indeksi
    private bool _isMoving = false; // Hareket ediyor mu
    private int _totalStepsToMove = 0; // Oyuncunun totalde edeceği hareket sayısı
    
    public void Init()
    {
        var tiles = TileController.Instance.GetInnerTiles();
        if (tiles != null && tiles.Count > 0)
        {
            var startTile = tiles[0].transform; // 0. Tile
            transform.position = startTile.position; // Oyuncu başlangıç pozisyonuna yerleştiriliyor (0. indekse)
            LookAtNextTile(); // Oyuncunun yönü bir sonraki tile'a bakacak şekilde ayarlanıyor.
        }
        
        playerAnimationController.PlayIdleAnimation();
    }
    
    
#if UNITY_EDITOR
    private void Update()
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
#endif
    
    
    /// <summary>
    /// Hareket edilecek total adım sayısına ekleme yapar.
    /// </summary>
    /// <param name="stepsToAdd">Eklenmek istenen adım sayısı</param>
    public void AddSteps(int stepsToAdd)
    {
        _totalStepsToMove += stepsToAdd;

        if (!_isMoving)
        {
            StartCoroutine(MoveThroughSteps());
        }
    }
    
    private IEnumerator MoveThroughSteps()
    {
        var tiles = TileController.Instance.GetInnerTiles();
        _isMoving = true;
        
        // Koşma animasyonu
        playerAnimationController.PlayRunAnimation();

        while (_totalStepsToMove > 0)
        {
            // index bir sonraki tile'a ayarlanıyor
            _currentTileIndex = (_currentTileIndex + 1) % tiles.Count;
            
            // Burada varsa tiledaki kaynak toplanıyor.
            var currentTile = tiles[_currentTileIndex];
            CollectTileResources(currentTile);

            // Oyuncuyu sıradaki Tile'a hareket ediyor
            yield return StartCoroutine(MoveToTile(tiles[_currentTileIndex].transform.position));

            // Gidilecek toplam adım sayısı azaltılıyor.
            _totalStepsToMove--;
        }

        _isMoving = false;
        playerAnimationController.PlayIdleAnimation();
    }

    /// <summary>
    /// Oyuncuyu belirtilen pozisyona yumuşakça hareket ettirir.
    /// </summary>
    /// <param name="targetPosition">Hedef Tile'ın pozisyonu</param>
    /// <returns></returns>
    private IEnumerator MoveToTile(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition; // Kesin pozisyon
        LookAtNextTile(); // Bir sonraki tile'a dön
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
    
    /// <summary>
    /// Oyuncu bir Tile'dan geçtiğinde kaynağı toplar.
    /// </summary>
    private void CollectTileResources(InnerTile tile)
    {
        if (!string.IsNullOrEmpty(tile.GetTileType()) && tile.GetTileAmount() > 0)
        {
            // Inventory'e ekle
            InventoryManager.Instance.AddFruit(tile.GetTileType(), tile.GetTileAmount());

            // Popup göster
            var tileSprite = TileController.Instance.GetTileSprite(tile.GetTileType());
            var popupPosition = tile.transform.position + Vector3.up * 2f;
            PopupManager.Instance.ShowPopup($"+{tile.GetTileAmount()}", tileSprite, popupPosition);
        }
    }

}
