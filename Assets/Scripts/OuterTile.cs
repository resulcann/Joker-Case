using UnityEngine;

public class OuterTile : MonoBehaviour
{
    public void SetType(string type)
    {
        UpdateVisual(type);
    }

    private void UpdateVisual(string type)
    {
        var buildingPrefab = Resources.Load<GameObject>($"Prefabs/Buildings/{type}");
        if (buildingPrefab != null)
        {
            var building = Instantiate(buildingPrefab, transform);
            building.name = $"{type}_Instance";
        
            SetRotation(building);
        }
    }

    private void SetRotation(GameObject building)
    {
        var worldPosition = transform.position;
        
        var gridSize = TileController.Instance.GetOuterGridSize(); // Outer grid boyutu
        var tileSize = TileController.Instance.GetTileSize(); // Tile boyutu
        var halfGrid = ((gridSize / 2f) - 0.5f) * tileSize; // Yarı grid boyutu (dış katman)
        
        float tolerance = 0.01f;

        // Kenar ve köşe kontrolü
        var isLeft = Mathf.Abs(worldPosition.x - (-halfGrid)) < tolerance;
        var isRight = Mathf.Abs(worldPosition.x - (halfGrid)) < tolerance;
        var isBottom = Mathf.Abs(worldPosition.z - (-halfGrid)) < tolerance;
        var isTop = Mathf.Abs(worldPosition.z - (halfGrid)) < tolerance;

        // Sol köşe
        if (isLeft && isTop)
        {
            building.transform.localEulerAngles = new Vector3(0, 90, 0); // Sağa bak
        }
        else if (isLeft && isBottom)
        {
            building.transform.localEulerAngles = new Vector3(0, 90, 0); // Sağa bak
        }
        // Sağ köşe
        else if (isRight && isTop)
        {
            building.transform.localEulerAngles = new Vector3(0, -90, 0); // Sola bak
        }
        else if (isRight && isBottom)
        {
            building.transform.localEulerAngles = new Vector3(0, -90, 0); // Sola bak
        }
        // Sol kenar
        else if (isLeft)
        {
            building.transform.localEulerAngles = new Vector3(0, 90, 0); // Sağa bak
        }
        // Sağ kenar
        else if (isRight)
        {
            building.transform.localEulerAngles = new Vector3(0, -90, 0); // Sola bak
        }
        // Üst kenar
        else if (isTop)
        {
            building.transform.localEulerAngles = new Vector3(0, 180, 0); // Aşağı bak
        }
        // Alt kenar
        else if (isBottom)
        {
            building.transform.localEulerAngles = new Vector3(0, 0, 0); // Yukarı bak
        }
    }
}
