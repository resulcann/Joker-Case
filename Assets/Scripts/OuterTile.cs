using UnityEngine;

public class OuterTile : MonoBehaviour
{
    private string _type;

    public string Type
    {
        get => _type;
        set
        {
            _type = value;
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        var buildingPrefab = Resources.Load<GameObject>($"Prefabs/Buildings/{Type}");
        if (buildingPrefab != null)
        {
            var building = Instantiate(buildingPrefab, transform);
            building.name = $"{Type}_Instance";

            SetRotation(building);
        }
    }

    private void SetRotation(GameObject building) // building are rotating to center
    {
        var worldPosition = transform.position;

        var gridSize = MapGenerator.Instance.MapData.grid.gridSize + 2; // Outer grid size
        var tileSize = MapGenerator.Instance.MapData.grid.tileSize; // Tile size
        var halfGrid = ((gridSize / 2f) - 0.5f) * tileSize; // half grid size

        const float tolerance = 0.01f;

        // edge and corner control
        var isLeft = Mathf.Abs(worldPosition.x - (-halfGrid)) < tolerance;
        var isRight = Mathf.Abs(worldPosition.x - (halfGrid)) < tolerance;
        var isBottom = Mathf.Abs(worldPosition.z - (-halfGrid)) < tolerance;
        var isTop = Mathf.Abs(worldPosition.z - (halfGrid)) < tolerance;

        // left corner
        if (isLeft && isTop)
        {
            building.transform.localEulerAngles = new Vector3(0, 90, 0); // look right
        }
        else if (isLeft && isBottom)
        {
            building.transform.localEulerAngles = new Vector3(0, 90, 0); // look right
        }
        // right corner
        else if (isRight && isTop)
        {
            building.transform.localEulerAngles = new Vector3(0, -90, 0); // look left
        }
        else if (isRight && isBottom)
        {
            building.transform.localEulerAngles = new Vector3(0, -90, 0); // look left
        }
        // left edge
        else if (isLeft)
        {
            building.transform.localEulerAngles = new Vector3(0, 90, 0); // look right
        }
        // right edge
        else if (isRight)
        {
            building.transform.localEulerAngles = new Vector3(0, -90, 0); // look left
        }
        // top edge
        else if (isTop)
        {
            building.transform.localEulerAngles = new Vector3(0, 180, 0); // look bottom
        }
        // bottom edge
        else if (isBottom)
        {
            building.transform.localEulerAngles = new Vector3(0, 0, 0); // look top
        }
    }
}
