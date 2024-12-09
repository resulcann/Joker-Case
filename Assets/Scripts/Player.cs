using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : GenericSingleton<Player>
{
    [Header("REFERENCES")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI stepCountText;
    [Space]
    [Header("MOVE SETTINGS")]
    [SerializeField] private float moveSpeed = 5f; // move speed
    [SerializeField] private float rotationSpeed = 5f; // rotate speed
    
    private int _currentTileIndex = 0;
    private bool _isMoving = false; // is player moving?
    private int _totalStepsToMove = 0; // player's total step count
    
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Run = Animator.StringToHash("Run");
    
    public void Init()
    {
        var tiles = TileController.Instance.GetInnerTiles();
        if (tiles != null && tiles.Count > 0)
        {
            var startTile = tiles[0].transform; // Tile 0
            transform.position = startTile.position; // Player moving to start tile (index 0)
            LookAtNextTile(); // Character rotating to next tile, character will be looking next tile
        }
        
        PlayIdleAnimation();
        UpdateStepText();
    }
    
    
#if UNITY_EDITOR
    private void Update()
    {
        // Editor test
        if (Input.GetKeyDown(KeyCode.Space) && !_isMoving)
        {
            AddSteps(1); // when "space" key down add +1 step
        }
        if (Input.GetKeyDown(KeyCode.S) && !_isMoving)
        {
            AddSteps(3); // when "s" key down add +3 step
        }
    }
#endif
    
    
    /// <summary>
    /// Increases to total step count
    /// </summary>
    /// <param name="stepsToAdd">Steps amount</param>
    public void AddSteps(int stepsToAdd)
    {
        _totalStepsToMove += stepsToAdd;
        UpdateStepText();

        if (!_isMoving)
        {
            StartCoroutine(MoveThroughSteps());
        }
    }
    
    private IEnumerator MoveThroughSteps()
    {
        var tiles = TileController.Instance.GetInnerTiles();
        _isMoving = true;
        
        // Plays running animation
        PlayRunAnimation();

        while (_totalStepsToMove > 0)
        {
            // index setting up to next tile.
            _currentTileIndex = (_currentTileIndex + 1) % tiles.Count;

            // player moves to next tile
            yield return StartCoroutine(MoveToTile(tiles[_currentTileIndex].transform.position));

            // total step count decreasing 1
            _totalStepsToMove--;
            UpdateStepText();
        }
        
        // when player stopped and if there is a collectible fruit. Player collects it.
        CollectTileResources(tiles[_currentTileIndex]);

        _isMoving = false;
        PlayIdleAnimation();
    }

    /// <summary>
    /// Moves player to target tile.
    /// </summary>
    /// <param name="targetPosition">Target tile position.</param>
    /// <param name="immediately">Move to tile immediately.</param>
    /// <returns></returns>
    private IEnumerator MoveToTile(Vector3 targetPosition, bool immediately = false)
    {
        if (!immediately)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        
        transform.position = targetPosition; // absolute position
        LookAtNextTile(); // Rotate to next tile
    }

    /// <summary>
    /// Rotates player to next tile.
    /// </summary>
    private void LookAtNextTile()
    {
        var tiles = TileController.Instance.GetInnerTiles();

        if (tiles != null && tiles.Count > 0)
        {
            var nextIndex = (_currentTileIndex + 1) % tiles.Count; // next tile index
            var direction = (tiles[nextIndex].transform.position - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Player collects resources.
    /// </summary>
    private void CollectTileResources(InnerTile tile)
    {
        if (!string.IsNullOrEmpty(tile.TileType) && tile.Amount > 0)
        {
            // Add to inventory
            InventoryManager.Instance.AddFruit(tile.TileType, tile.Amount);

            // Show fruit animation
            var tileSprite = TileController.Instance.GetTileSprite(tile.TileType);
            var popupPosition = tile.transform.position + Vector3.up * 2f;
            FruitAnimationManager.Instance.ShowAnimation($"+{tile.Amount}", tileSprite, popupPosition);
        }
    }

    private void UpdateStepText()
    {
        stepCountText.text = GameUtility.FormatNumber(_totalStepsToMove);
    }
    
    private void PlayIdleAnimation()
    {
        animator.SetBool(Idle, true);
        animator.SetBool(Run, false);
    }

    private void PlayRunAnimation()
    {
        animator.SetBool(Idle, false);
        animator.SetBool(Run, true);
    }

}
