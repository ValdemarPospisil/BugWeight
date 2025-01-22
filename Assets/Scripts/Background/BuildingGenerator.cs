using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingGenerator : MonoBehaviour
{
    public string tilemapName = "BuildingTilemap"; // Name of the Tilemap GameObject in the scene
    public TileBase outlineTile;    // Tile used for the outline of the building
    public TileBase fillTile;       // Tile used for the interior of the building

    [SerializeField]
    private int minHeight = 3;
    [SerializeField]
    private int maxHeight = 7;
    [SerializeField]
    private int minWidth = 3;
    [SerializeField]
    private int maxWidth = 7;

    private int buildingHeight;
    private int buildingWidth;

    private Tilemap buildingTilemap;
    private BoxCollider2D boxCollider;

    void Start()
    {
        // Find and reference the Tilemap in the scene
        GameObject tilemapObject = GameObject.Find(tilemapName);
        if (tilemapObject != null)
        {
            buildingTilemap = tilemapObject.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Tilemap not found in the scene.");
            return;
        }

        boxCollider = GetComponent<BoxCollider2D>();

        // Generate random dimensions for the building
        buildingHeight = Random.Range(minHeight, maxHeight);
        buildingWidth = Random.Range(minWidth, maxWidth);

        // Generate the building on the Tilemap
        GenerateBuilding();

        // Adjust the BoxCollider to fit the building
        AdjustCollider();
    }

    void GenerateBuilding()
    {
        Vector3Int basePosition = Vector3Int.FloorToInt(transform.position);

        // Create the outline of the building
        for (int y = 0; y < buildingHeight; y++)
        {
            for (int x = 0; x < buildingWidth; x++)
            {
                if (x == 0 || x == buildingWidth - 1 || y == 0 || y == buildingHeight - 1)
                {
                    buildingTilemap.SetTile(basePosition + new Vector3Int(x, y, 0), outlineTile);
                }
                else
                {
                    // Apply a pattern to the interior
                    if ((x + y) % 2 == 0)
                    {
                        buildingTilemap.SetTile(basePosition + new Vector3Int(x, y, 0), fillTile);
                    }
                    else
                    {
                        // Optional: Add a secondary fill pattern for visual interest
                        buildingTilemap.SetTile(basePosition + new Vector3Int(x, y, 0), null); // Empty or alternative tile
                    }
                }
            }
        }
    }

    void AdjustCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.size = new Vector2(buildingWidth, buildingHeight);
            boxCollider.offset = new Vector2(buildingWidth / 2f, buildingHeight / 2f);
        }
    }
}
