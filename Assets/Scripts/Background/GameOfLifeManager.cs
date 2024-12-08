using UnityEngine;
using UnityEngine.Tilemaps;

public class GameOfLifeManager : MonoBehaviour {
    [SerializeField] private int width = 100;   // Grid width
    [SerializeField] private int height = 100;  // Grid height
    [SerializeField] private float updateInterval = 0.5f;

    [SerializeField] private Tilemap tilemap;   // Reference to the Tilemap
    [SerializeField] private Tile aliveTile;    // Tile for alive cells
    [SerializeField] private Tile deadTile;     // Tile for dead cells

    [SerializeField] private int maxCycles = 100;  // Max number of cycles before stopping
    [SerializeField] private float noiseDensity = 0.5f;  // Initial noise density

    private bool[,] grid;   // Current state of the grid
    private int currentCycle = 0;  // Track cycles

    void Start() {
        grid = new bool[width, height];
        InitializeGrid();
        InvokeRepeating("UpdateGrid", updateInterval, updateInterval);
    }

    void InitializeGrid() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                grid[x, y] = Random.value > noiseDensity;  // Random initialization
                UpdateCellVisual(x, y);
            }
        }
    }

    void UpdateGrid() {
        if (currentCycle >= maxCycles) {
            CancelInvoke("UpdateGrid");  // Stop the simulation
            Debug.Log("Simulation Stopped");
            return;
        }

        bool[,] newGrid = new bool[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int neighbors = CountNeighbors(x, y);
                if (grid[x, y]) {
                    newGrid[x, y] = neighbors == 2 || neighbors == 3;
                } else {
                    newGrid[x, y] = neighbors == 3;
                }
            }
        }

        grid = newGrid;
        UpdateVisuals();
        currentCycle++;  // Increment cycle count
    }

    int CountNeighbors(int x, int y) {
        int count = 0;
        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx, ny = y + dy;
                if (nx >= 0 && ny >= 0 && nx < width && ny < height && grid[nx, ny]) {
                    count++;
                }
            }
        }
        return count;
    }

    void UpdateVisuals() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                UpdateCellVisual(x, y);
            }
        }
    }

    void UpdateCellVisual(int x, int y) {
        Vector3Int tilePosition = new Vector3Int(x, y, 0);
        tilemap.SetTile(tilePosition, grid[x, y] ? aliveTile : deadTile);
    }
}
