using UnityEngine;

public class OuterTile : MonoBehaviour
{
    public void SetType(string type)
    {
        UpdateVisual(type);
    }

    private void UpdateVisual(string type)
    {
        // OuterTile'ın türü burada atanır, görsel güncellemeler yapılabilir.
        // Debug.Log($"OuterTile set to type: {type}");
    }
}