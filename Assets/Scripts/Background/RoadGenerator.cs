using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;         // Reference to the Tilemap
    [SerializeField] private TileBase roadTile;       // Road tile
    [SerializeField] private TileBase grassTile;      // Grass tile
    [SerializeField] private TileBase borderTile;      // Grass tile
    [SerializeField] private TileBase buildingTile;    // Crop soil tile
    [SerializeField] private TileBase building2Tile;    // Crop soil tile
    [SerializeField] private TileBase building3Tile;    // Crop soil tile
    [SerializeField] private TileBase building4Tile;    // Crop soil tile
    [SerializeField] private TileBase outlineTile;    // Building outline tile
    [SerializeField] private int width = 50;          // Grid width
    [SerializeField] private int height = 50;         // Grid height
    [SerializeField] private int numberOfRoads = 10;  // Number of initial roads
    [SerializeField] private int maxRoadLength = 30;  // Maximum road length
    [SerializeField] private int minRoadLength = 10;  // Minimum road length
    [SerializeField] private float primaryChance = 0.8f; // Chance to continue in primary direction
    [SerializeField] private int roadWidth = 3;       // Width of the road
    [SerializeField] private int buildingMinWidth = 15; 
    [SerializeField] private int buildingMaxWidth = 30; 
    [SerializeField] private int buildingMinHeight = 15;
    [SerializeField] private int buildingMaxHeight = 30;
    [SerializeField] private int buildingAttempts = 10;

    private int[,] grid;            // 2D array to represent the grid (1 = road, 0 = grass)
    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(0, 1),  // Up
        new Vector2Int(0, -1), // Down
        new Vector2Int(-1, 0), // Left
        new Vector2Int(1, 0)   // Right
    };

    void Start()
    {
        grid = new int[width, height];
        GenerateRoads();
        ExpandRoads();
        //SpawnCropSoil(cropSoilMinWidth, cropSoilMaxWidth, cropSoilMinHeight, cropSoilMaxHeight, cropSoilAttempts);
        SpawnBuildings(buildingMinWidth, buildingMaxWidth, buildingMinHeight, buildingMaxHeight, buildingAttempts);
        RenderTilemap();
    }

    // Generate roads on the grid
    void GenerateRoads()
    {
        for (int i = 0; i < numberOfRoads; i++)
        {
            int startX = Random.Range(0, width);
            int startY = Random.Range(0, height);
            GenerateSingleRoad(new Vector2Int(startX, startY));
        }
    }

    void GenerateSingleRoad(Vector2Int start)
    {
        Vector2Int currentPos = start;
        Vector2Int primaryDirection = directions[Random.Range(0, directions.Length)];
        int roadLength = Random.Range(minRoadLength, maxRoadLength);

        for (int step = 0; step < roadLength; step++)
        {
            // Mark the current tile as road
            grid[currentPos.x, currentPos.y] = 1;

            // Decide the next direction
            float randomValue = Random.value;
            Vector2Int nextDirection = primaryDirection;

            if (randomValue > primaryChance) // Turn left or right
            {
                nextDirection = Random.value > 0.5f ? TurnLeft(primaryDirection) : TurnRight(primaryDirection);
                primaryDirection = nextDirection; // Update primary direction
            }

            // Move in the chosen direction with wrapping
            currentPos = WrapPosition(currentPos + nextDirection);

            // Stop if we've exceeded the bounds (rare, only due to wrapping bugs)
            if (currentPos.x < 0 || currentPos.x >= width || currentPos.y < 0 || currentPos.y >= height)
                break;
        }
    }

    // Expands all roads and marks borders
    void ExpandRoads() {
        int[,] expandedGrid = (int[,])grid.Clone();

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (grid[x, y] == 1) {
                    ExpandTile(expandedGrid, x, y, roadWidth);
                    MarkBorderTile(expandedGrid, x, y);
                }
            }
        }

        grid = expandedGrid;
    }

    // Expands the road around the given tile
    void ExpandTile(int[,] grid, int x, int y, int width) {
        for (int dy = -width; dy <= width; dy++) {
            for (int dx = -width; dx <= width; dx++) {
                if (IsInsideGrid(x + dx, y + dy)) {
                    grid[x + dx, y + dy] = 1;  // Expand road
                }
            }
        }
    }

    // Marks a 1-pixel-wide border around the expanded road
    void MarkBorderTile(int[,] grid, int x, int y) {
        for (int dy = -(roadWidth + 1); dy <= roadWidth + 1; dy++) {
            for (int dx = -(roadWidth + 1); dx <= roadWidth + 1; dx++) {
                int nx = x + dx;
                int ny = y + dy;

                if (IsInsideGrid(nx, ny) && grid[nx, ny] == 0) {
                    grid[nx, ny] = 2;  // Mark as border
                }
            }
        }
    }

    // Ensures the coordinates are within the grid bounds
    bool IsInsideGrid(int x, int y) {
        return x >= 0 && y >= 0 && x < width && y < height;
    }




    

    // Utility: Wrap position around the grid borders
    Vector2Int WrapPosition(Vector2Int position)
    {
        int wrappedX = (position.x + width) % width;
        int wrappedY = (position.y + height) % height;
        return new Vector2Int(wrappedX, wrappedY);
    }

    // Utility: Turn left (rotate counterclockwise)
    Vector2Int TurnLeft(Vector2Int direction)
    {
        if (direction == directions[0]) return directions[2]; // Up -> Left
        if (direction == directions[1]) return directions[3]; // Down -> Right
        if (direction == directions[2]) return directions[1]; // Left -> Down
        return directions[0];                                 // Right -> Up
    }

    // Utility: Turn right (rotate clockwise)
    Vector2Int TurnRight(Vector2Int direction)
    {
        if (direction == directions[0]) return directions[3]; // Up -> Right
        if (direction == directions[1]) return directions[2]; // Down -> Left
        if (direction == directions[2]) return directions[0]; // Left -> Up
        return directions[1];                                 // Right -> Down
    }

    void SpawnBuildings(int minWidth,int maxWidth,int minHeight, int maxHeight, int attempts) 
    {
        for (int i = 0; i < attempts; i++) {

            int areaWidth = Random.Range(minWidth, maxWidth + 1);
            int areaHeight = Random.Range(minHeight, maxHeight + 1);

            int startX = Random.Range(0, width - areaWidth);
            int startY = Random.Range(0, height - areaHeight);

            if (IsAreaFree(startX, startY, areaWidth, areaHeight)) {
                MarkBuildingOutline(startX, startY, areaWidth, areaHeight, 3); // Outline tile value
                FillBuildingInteriorWithVoronoi(startX, startY, areaWidth, areaHeight);
            }
        }
    }

    // Marks the outline of a rectangular area with a specific tile value, leaving a random door
    void MarkBuildingOutline(int startX, int startY, int areaWidth, int areaHeight, int tileValue) {
        // Determine door position and size
        int doorWidth = 5;
        int doorPosition = Random.Range(1, areaWidth - doorWidth - 1);

        // Top and bottom borders with door
        for (int x = startX; x < startX + areaWidth; x++) {
            if (x < startX + doorPosition || x >= startX + doorPosition + doorWidth) {
                grid[x, startY] = tileValue; // Top border
                grid[x, startY + areaHeight - 1] = tileValue; // Bottom border
            }
        }

        // Left and right borders
        for (int y = startY; y < startY + areaHeight; y++) {
            grid[startX, y] = tileValue; // Left border
            grid[startX + areaWidth - 1, y] = tileValue; // Right border
        }
    }

    void FillBuildingInteriorWithVoronoi(int startX, int startY, int areaWidth, int areaHeight) {
    int numSeeds = Random.Range(3, 6);
    List<Vector2Int> seeds = new List<Vector2Int>();
    Dictionary<Vector2Int, int> seedTileMap = new Dictionary<Vector2Int, int>();

    // Place seeds and assign each a random tile type
    for (int i = 0; i < numSeeds; i++) {
        int seedX = Random.Range(startX + 1, startX + areaWidth - 1);
        int seedY = Random.Range(startY + 1, startY + areaHeight - 1);
        Vector2Int seed = new Vector2Int(seedX, seedY);
        seeds.Add(seed);
        seedTileMap[seed] = Random.Range(4, 8); // Assume tile types 3 to 5 are different floor types
    }

    // Assign each tile in the area to the closest seed's tile type
    for (int y = startY + 1; y < startY + areaHeight - 1; y++) {
        for (int x = startX + 1; x < startX + areaWidth - 1; x++) {
            Vector2Int currentTile = new Vector2Int(x, y);
            Vector2Int closestSeed = FindClosestSeed(currentTile, seeds);
            grid[x, y] = seedTileMap[closestSeed];
        }
    }
}

    Vector2Int FindClosestSeed(Vector2Int tile, List<Vector2Int> seeds) {
        Vector2Int closestSeed = seeds[0];
        float minDistance = Vector2Int.Distance(tile, closestSeed);
        foreach (Vector2Int seed in seeds) {
            float distance = Vector2Int.Distance(tile, seed);
            if (distance < minDistance) {
                closestSeed = seed;
                minDistance = distance;
            }
        }
        return closestSeed;
    }


// Checks if a rectangular area is free of roads or other obstacles
    bool IsAreaFree(int startX, int startY, int areaWidth, int areaHeight) {
        for (int y = startY; y < startY + areaHeight; y++) {
            for (int x = startX; x < startX + areaWidth; x++) {
                if (!IsInsideGrid(x, y) || grid[x, y] != 0) {
                    return false;
                }
            }
        }
        return true;
    }


   // Render the grid to the Tilemap
    void RenderTilemap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), grassTile);
                }
                else if (grid[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), roadTile);
                }
                else if (grid[x, y] == 2)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), borderTile);
                }
                else if (grid[x, y] == 3)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), outlineTile);
                }
                else if (grid[x, y] == 4)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), buildingTile);
                }
                else if (grid[x, y] == 5)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), building2Tile);
                }
                else if (grid[x, y] == 6)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), building3Tile);
                }
                else if (grid[x, y] == 7)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), building4Tile);
                }
                else
                {
                    Debug.LogError("Invalid grid value: " + grid[x, y]);
                }
            }
        }
    }
}
