using UnityEngine;
using UnityEngine.Tilemaps;

public class DiamondSquareGenerator : MonoBehaviour {
    public int size = 129;    // Must be 2^n + 1
    public float roughness = 2f;
    public float heightScale = 5f;

    public Tilemap tilemap;
    public Tile highTile;
    public Tile midTile;
    public Tile lowTile;

    private float[,] heightMap;

    void Start() {
        GenerateHeightMap();
        ApplyTilemap();
    }

    void GenerateHeightMap() {
        heightMap = new float[size, size];

        // Initialize corners
        heightMap[0, 0] = Random.Range(0f, heightScale);
        heightMap[0, size - 1] = Random.Range(0f, heightScale);
        heightMap[size - 1, 0] = Random.Range(0f, heightScale);
        heightMap[size - 1, size - 1] = Random.Range(0f, heightScale);

        int stepSize = size - 1;

        while (stepSize > 1) {
            int halfStep = stepSize / 2;

            // Diamond Step
            for (int x = 0; x < size - 1; x += stepSize) {
                for (int y = 0; y < size - 1; y += stepSize) {
                    float avg = (
                        heightMap[x, y] +
                        heightMap[x + stepSize, y] +
                        heightMap[x, y + stepSize] +
                        heightMap[x + stepSize, y + stepSize]
                    ) / 4f;

                    heightMap[x + halfStep, y + halfStep] = avg + Random.Range(-roughness, roughness);
                }
            }

            // Square Step
            for (int x = 0; x < size; x += halfStep) {
                for (int y = (x + halfStep) % stepSize; y < size; y += stepSize) {
                    float avg = (
                        heightMap[(x - halfStep + size - 1) % (size - 1), y] +
                        heightMap[(x + halfStep) % (size - 1), y] +
                        heightMap[x, (y - halfStep + size - 1) % (size - 1)] +
                        heightMap[x, (y + halfStep) % (size - 1)]
                    ) / 4f;

                    heightMap[x, y] = avg + Random.Range(-roughness, roughness);

                    if (x == 0) heightMap[size - 1, y] = heightMap[x, y];
                    if (y == 0) heightMap[x, size - 1] = heightMap[x, y];
                }
            }

            stepSize /= 2;
            roughness *= 0.5f;  // Reduce roughness as step size decreases
        }
    }

    void ApplyTilemap() {
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                Tile tileToPlace = SelectTile(heightMap[x, y]);
                tilemap.SetTile(tilePosition, tileToPlace);
            }
        }
    }

    Tile SelectTile(float height) {
        if (height > heightScale * 0.6f) return highTile;  // Mountains
        if (height > heightScale * 0.3f) return midTile;   // Grass
        return lowTile;  // Water or low ground
    }
}
