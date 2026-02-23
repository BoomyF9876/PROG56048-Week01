using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages procedural content generation for the game map.
/// Supports both pure random and Perlin noise-based generation methods.
/// </summary>
public class PCG : MonoBehaviour
{
    #region Serialized Fields

    [Header("Seed Configuration")]
    [Tooltip("The current seed value (auto-generated if fixSeed is 0).")]
    [SerializeField] private int seed;

    [Tooltip("Fixed seed value. Set to 0 for random seed each generation.")]
    [SerializeField] private int fixSeed;

    [Header("Perlin Noise Settings")]
    [Tooltip("Enable Perlin noise for smoother, more natural terrain generation.")]
    [SerializeField] private bool usePerlinNoise;

    [Tooltip("Scale of the Perlin noise. Lower = larger features, Higher = more detail.")]
    [SerializeField, Range(0, 20)] private float perlinScale = 0.1f;

    [Tooltip("Horizontal scroll offset for the noise map.")]
    [SerializeField, Range(0, 100)] private float hScroll = 0f;

    [Tooltip("Vertical scroll offset for the noise map.")]
    [SerializeField, Range(0, 100)] private float vScroll = 0f;

    [Header("Tile Configuration")]
    [Tooltip("List of tiles and their weighted spawn chances.")]
    [SerializeField] private List<Tile> tiles;

    #endregion

    #region Private Fields

    private GridManager gridManager;

    // Cached values for detection and generation
    private int cachedTotalChance;
    private float perlinOffsetX;
    private float perlinOffsetZ;
    private float lastHScroll;
    private float lastVScroll;

    #endregion

    #region Nested Types

    /// <summary>
    /// Defines a tile with its associated TileSO and weighted chance of occurrence.
    /// Higher chance values make the tile more likely to appear.
    /// </summary>
    [Serializable]
    public struct Tile
    {
        [Tooltip("The tile ScriptableObject defining this tile's properties.")]
        public TileSO tile;

        [Tooltip("Relative weight for tile selection. Higher = more common.")]
        [Min(1)]
        public int chance;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the procedural content generation with a reference to the GridManager.
    /// </summary>
    /// <param name="gridManager">The grid manager to use for placing generated content.</param>
    public void Initialize(GridManager gridManager)
    {
        if (gridManager == null)
        {
            Debug.LogError($"[{nameof(PCG)}] GridManager reference is null!");
            return;
        }

        this.gridManager = gridManager;
        GenerateMap();
    }

    /// <summary>
    /// Clears the current map and generates a new one based on the current settings.
    /// </summary>
    public void RegenerateMap()
    {
        if (gridManager == null)
        {
            Debug.LogError($"[{nameof(PCG)}] Cannot regenerate map: GridManager is not initialized!");
            return;
        }

        gridManager.ClearGrid();
        InitializeSeed();
        GenerateMap();
    }

    /// <summary>
    /// Checks for value changes in the Inspector and handles rounding and regeneration.
    /// </summary>
    private void OnValidate()
    {
        // Round values to specified intervals
        perlinScale = Mathf.Round(perlinScale * 10f) / 10f; // Interval of 0.1
        hScroll = Mathf.Round(hScroll); // Interval of 1
        vScroll = Mathf.Round(vScroll); // Interval of 1

        // If scrolling changes during play mode, freeze the current seed and regenerate
        if (Application.isPlaying && gridManager != null)
        {
            if (!Mathf.Approximately(hScroll, lastHScroll) || !Mathf.Approximately(vScroll, lastVScroll))
            {
                // Copy current seed to fixed seed to "freeze" the map while scrolling
                fixSeed = seed;
                RegenerateMap();
            }
        }

        lastHScroll = hScroll;
        lastVScroll = vScroll;
    }

    #endregion

    #region Private Methods - Generation

    /// <summary>
    /// Initializes the random number generator seed.
    /// Uses a fixed seed if specified; otherwise, generates a random seed.
    /// </summary>
    private void InitializeSeed()
    {
        seed = fixSeed == 0
            ? UnityEngine.Random.Range(0, int.MaxValue)
            : fixSeed;

        UnityEngine.Random.InitState(seed);
    }

    /// <summary>
    /// Generates the map using either Perlin noise or random numbers based on settings.
    /// </summary>
    private void GenerateMap()
    {
        if (!ValidateTiles())
        {
            return;
        }

        // Pre-calculate total chance using LINQ
        cachedTotalChance = tiles.Sum(t => t.chance);

        if (usePerlinNoise)
        {
            GenerateMapWithPerlinNoise();
        }
        else
        {
            GenerateMapWithRandomNumbers();
        }

        gridManager.ResetAStar();
    }

    /// <summary>
    /// Validates that the tiles list is properly configured.
    /// </summary>
    /// <returns>True if tiles are valid; otherwise, false.</returns>
    private bool ValidateTiles()
    {
        if (tiles == null || tiles.Count == 0)
        {
            Debug.LogWarning($"[{nameof(PCG)}] No tiles configured for map generation!");
            return false;
        }

        // Check for null tile references
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].tile == null)
            {
                Debug.LogError($"[{nameof(PCG)}] Tile at index {i} has a null TileSO reference!");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Generates the map using Perlin noise for coherent terrain generation.
    /// </summary>
    private void GenerateMapWithPerlinNoise()
    {
        // Map perlinScale (0-20) to a usable noise scale range (0.1-2.0)
        float perlinScaleMapped = Mathf.Lerp(0.1f, 2.0f, perlinScale / 20f);

        // Generate random offsets for this generation pass (seeding the noise location)
        perlinOffsetX = UnityEngine.Random.value * 100f;
        perlinOffsetZ = UnityEngine.Random.value * 100f;

        int halfX = gridManager.GridXZ.x / 2;
        int halfZ = gridManager.GridXZ.z / 2;

        for (int x = -halfX; x < halfX; x++)
        {
            for (int z = -halfZ; z < halfZ; z++)
            {
                // Calculate Perlin noise value at this position
                float noiseX = (x + perlinOffsetX + hScroll) * perlinScaleMapped;
                float noiseZ = (z + perlinOffsetZ + vScroll) * perlinScaleMapped;
                float perlinValue = Mathf.PerlinNoise(noiseX, noiseZ);

                // Scale noise value (0-1) to tile chance range
                int scaledValue = (int)(perlinValue * cachedTotalChance);

                Vector3Int gridPosition = new Vector3Int(x, 0, z);
                PlaceTileAtPosition(gridPosition, scaledValue);
            }
        }
    }

    /// <summary>
    /// Generates the map using pure random selection (white noise).
    /// </summary>
    private void GenerateMapWithRandomNumbers()
    {
        int halfX = gridManager.GridXZ.x / 2;
        int halfZ = gridManager.GridXZ.z / 2;

        for (int x = -halfX; x < halfX; x++)
        {
            for (int z = -halfZ; z < halfZ; z++)
            {
                int randomValue = UnityEngine.Random.Range(0, cachedTotalChance);

                Vector3Int gridPosition = new Vector3Int(x, 0, z);
                PlaceTileAtPosition(gridPosition, randomValue);
            }
        }
    }

    /// <summary>
    /// Selects and places a tile at the specified grid position based on the weighted value.
    /// </summary>
    /// <param name="gridPosition">The grid position to place the tile at.</param>
    /// <param name="weightedValue">A value between 0 and totalChance to determine tile selection.</param>
    private void PlaceTileAtPosition(Vector3Int gridPosition, int weightedValue)
    {
        int cumulativeChance = 0;

        foreach (Tile tile in tiles)
        {
            cumulativeChance += tile.chance;

            if (weightedValue < cumulativeChance)
            {
                Transform newTile = Instantiate(tile.tile.Prefab, gridPosition, Quaternion.identity);
                newTile.parent = gridManager.TilesParent;
                gridManager.ObjectsOnGrid[gridPosition] = newTile;
                gridManager.TileTypesOnGrid[gridPosition] = tile.tile;
                return;
            }
        }
    }

    #endregion
}
