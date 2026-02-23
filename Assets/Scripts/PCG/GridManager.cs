using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the grid and handles placement and interaction of various grid objects.
/// Responsible for grid data storage, tile placement, and coordinating with the pathfinder.
/// </summary>
public class GridManager : MonoBehaviour
{
    #region Constants

    /// <summary>
    /// Unity's default plane mesh has a diameter of 10 units (radius of 5 from center).
    /// This constant is used when calculating grid dimensions from plane scale.
    /// </summary>
    private const float UNITY_PLANE_SIZE = 10f;

    #endregion

    #region Serialized Fields

    [Header("Prefabs")]
    [Tooltip("The selector object used to indicate the current grid position.")]
    [SerializeField] private Transform selector;

    [Tooltip("The obstacle prefab used to represent obstacles on the grid.")]
    [SerializeField] private Transform obstaclePrefab;

    [Tooltip("The waypoint prefab used to represent waypoints on the grid.")]
    [SerializeField] private Transform waypointPrefab;

    [Tooltip("The start prefab used to represent the start position on the grid.")]
    [SerializeField] private Transform startPrefab;

    [Tooltip("The end prefab used to represent the end position on the grid.")]
    [SerializeField] private Transform endPrefab;

    [Tooltip("The A* prefab used to represent the A* nodes on the grid.")]
    [SerializeField] private Transform aStarPrefab;

    [Header("Grid Configuration")]
    [Tooltip("The plane used to define the grid boundaries.")]
    [SerializeField] private Transform plane;

    [Header("Parent Transforms")]
    [Tooltip("Parent transform for instantiated tiles.")]
    [SerializeField] private Transform tilesParent;

    [Tooltip("Parent transform for instantiated A* nodes.")]
    [SerializeField] private Transform aStarNodesParent;

    [Header("References")]
    [Tooltip("The PCG component used to generate the grid content.")]
    [SerializeField] private PCG pcg;

    #endregion

    #region Private Fields

    private Grid grid;
    private Vector3Int gridXZ;
    private Transform placeholderSelector;
    private AStarPathfinder pathfinder;

    private readonly Dictionary<Vector3Int, Transform> objectsOnGrid = new Dictionary<Vector3Int, Transform>();
    private readonly Dictionary<Vector3Int, Transform> waypointsOnGrid = new Dictionary<Vector3Int, Transform>();
    private readonly Dictionary<Vector3Int, TileSO> tileTypesOnGrid = new Dictionary<Vector3Int, TileSO>();

    private Vector3Int startPosition = Vector3Int.zero;
    private Vector3Int endPosition = Vector3Int.zero;

    #endregion

    #region Public Properties

    /// <summary>Gets the obstacle prefab.</summary>
    public Transform Obstacle => obstaclePrefab;

    /// <summary>Gets the waypoint prefab.</summary>
    public Transform Waypoint => waypointPrefab;

    /// <summary>Gets the start marker prefab.</summary>
    public Transform Start => startPrefab;

    /// <summary>Gets the end marker prefab.</summary>
    public Transform End => endPrefab;

    /// <summary>Gets the A* node prefab.</summary>
    public Transform AStarPrefab => aStarPrefab;

    /// <summary>Gets the Unity Grid component.</summary>
    public Grid Grid => grid;

    /// <summary>Gets the grid dimensions (X and Z represent columns and rows).</summary>
    public Vector3Int GridXZ => gridXZ;

    /// <summary>Gets the parent transform for tiles.</summary>
    public Transform TilesParent => tilesParent;

    /// <summary>Gets the parent transform for A* nodes.</summary>
    public Transform AStarNodesParent => aStarNodesParent;

    /// <summary>Gets the dictionary of objects placed on the grid.</summary>
    public Dictionary<Vector3Int, Transform> ObjectsOnGrid => objectsOnGrid;

    /// <summary>Gets the dictionary of tile types at each grid position.</summary>
    public Dictionary<Vector3Int, TileSO> TileTypesOnGrid => tileTypesOnGrid;

    /// <summary>Gets the dictionary of waypoints on the grid.</summary>
    public Dictionary<Vector3Int, Transform> WaypointsOnGrid => waypointsOnGrid;

    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes the grid, pathfinder, and PCG on awake.
    /// </summary>
    private void Awake()
    {
        InitializeGrid();
        InitializePathfinder();
        InitializePCG();
    }

    /// <summary>
    /// Handles user input for object placement and pathfinding.
    /// </summary>
    private void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// Editor-only validation to update grid when inspector values change.
    /// </summary>
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (plane != null && grid != null)
        {
            AdjustGridSize();
        }
#endif
    }

    #endregion

    #region Initialization

    private void InitializeGrid()
    {
        grid = GetComponentInChildren<Grid>();

        if (grid == null)
        {
            Debug.LogError($"[{nameof(GridManager)}] No Grid component found in children!");
            return;
        }

        placeholderSelector = Instantiate(selector, Vector3.zero, Quaternion.identity);
        placeholderSelector.parent = tilesParent;
        AdjustGridSize();
    }

    private void InitializePathfinder()
    {
        pathfinder = gameObject.AddComponent<AStarPathfinder>();
        pathfinder.InitializeNodes(this);
    }

    private void InitializePCG()
    {
        pcg?.Initialize(this);
    }

    #endregion

    #region Input Handling

    private void HandleInput()
    {
        if (!MouseUtil.Instance.TryGetMousePosition(out Vector3 worldPosition))
        {
            return;
        }

        Vector3Int gridPosition = grid.WorldToCell(worldPosition);
        placeholderSelector.position = grid.CellToWorld(gridPosition);

        // Set start position (I + Left Click)
        if (Keyboard.current.iKey.isPressed && Mouse.current.leftButton.wasPressedThisFrame)
        {
            startPosition = gridPosition;
            return;
        }

        // Set end position (O + Left Click)
        if (Keyboard.current.oKey.isPressed && Mouse.current.leftButton.wasPressedThisFrame)
        {
            endPosition = gridPosition;
            return;
        }

        // Handle object placement/removal
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleObjectPlacement(gridPosition);
        }

        // Trigger pathfinding (P key)
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            pathfinder.FindPath(startPosition, endPosition);
        }
    }

    /// <summary>
    /// Handles the placement or removal of objects on the grid based on modifier keys.
    /// - Shift + Click: Add obstacle
    /// - Alt + Click: Add waypoint
    /// - Ctrl + Click: Remove existing object
    /// </summary>
    /// <param name="gridPosition">The grid position where the action occurs.</param>
    private void HandleObjectPlacement(Vector3Int gridPosition)
    {
        bool shiftPressed = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        bool ctrlPressed = Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed;
        bool altPressed = Keyboard.current.leftAltKey.isPressed || Keyboard.current.rightAltKey.isPressed;

        bool gridModified = false;

        if (TryGetObjectAtPosition(gridPosition, out var dictionary))
        {
            if (ctrlPressed)
            {
                RemoveObject(gridPosition, dictionary);
                gridModified = true;
            }
        }
        else
        {
            if (shiftPressed)
            {
                AddObject(gridPosition, obstaclePrefab, objectsOnGrid);
                gridModified = true;
            }
            else if (altPressed)
            {
                AddObject(gridPosition, waypointPrefab, waypointsOnGrid);
                gridModified = true;
            }
        }

        // Update A* pathfinding when grid is modified
        if (gridModified)
        {
            ResetAStar();
        }
    }

    #endregion

    #region Grid Operations

    /// <summary>
    /// Adjusts the grid cell size based on the transform's scale.
    /// </summary>
    private void AdjustGridSize()
    {
        Vector3 scale = transform.localScale;
        grid.cellSize = new Vector3(1.0f / scale.x, 0.0f, 1.0f / scale.z);
        gridXZ = CalculateGridSizeFromScale();
    }

    /// <summary>
    /// Calculates the grid dimensions based on the plane's scale and the grid's cell size.
    /// </summary>
    /// <returns>The grid size as Vector3Int (X = columns, Z = rows).</returns>
    public Vector3Int CalculateGridSizeFromScale()
    {
        float planeScaleX = plane.localScale.x;
        float planeScaleZ = plane.localScale.z;

        float cellSizeX = grid.cellSize.x;
        float cellSizeZ = grid.cellSize.z;

        int columns = Mathf.RoundToInt((UNITY_PLANE_SIZE * planeScaleX) / cellSizeX);
        int rows = Mathf.RoundToInt((UNITY_PLANE_SIZE * planeScaleZ) / cellSizeZ);

        return new Vector3Int(columns, 0, rows);
    }

    /// <summary>
    /// Clears all objects, waypoints, and tile types from the grid.
    /// </summary>
    public void ClearGrid()
    {
        ClearDictionary(objectsOnGrid);
        ClearDictionary(waypointsOnGrid);
        tileTypesOnGrid.Clear();
    }

    /// <summary>
    /// Resets the A* pathfinder's neighbor connections.
    /// Call this after modifying the grid to update pathfinding data.
    /// </summary>
    public void ResetAStar()
    {
        pathfinder?.ResetNeighbours();
    }

    /// <summary>
    /// Checks if a tile at the given position is walkable.
    /// A tile is walkable if:
    /// 1. It has a walkable TileSO type, AND
    /// 2. There is no obstacle placed on it
    /// </summary>
    /// <param name="position">The grid position to check.</param>
    /// <returns>True if the tile is walkable and has no obstacles; otherwise, false.</returns>
    public bool IsTileWalkable(Vector3Int position)
    {
        // Check if there's an obstacle at this position (obstacles block movement)
        // if (objectsOnGrid.ContainsKey(position))
        // {
        //     return false;
        // }

        // Check if the tile type allows walking
        if (tileTypesOnGrid.TryGetValue(position, out TileSO tileSO))
        {
            return Array.Exists(tileSO.TileType, type => type == TileSO.TileTypes.Walkable);
        }

        return false;
    }

    #endregion

    #region Object Management

    /// <summary>
    /// Checks if a position on the grid is occupied by an object.
    /// </summary>
    /// <param name="gridPosition">The position to check.</param>
    /// <param name="dictionary">The dictionary where the object was found, if any.</param>
    /// <returns>True if an object exists at the position; otherwise, false.</returns>
    private bool TryGetObjectAtPosition(Vector3Int gridPosition, out Dictionary<Vector3Int, Transform> dictionary)
    {
        if (objectsOnGrid.ContainsKey(gridPosition))
        {
            dictionary = objectsOnGrid;
            return true;
        }

        if (waypointsOnGrid.ContainsKey(gridPosition))
        {
            dictionary = waypointsOnGrid;
            return true;
        }

        dictionary = null;
        return false;
    }

    /// <summary>
    /// Adds an object to a specified position on the grid.
    /// </summary>
    private void AddObject(Vector3Int gridPosition, Transform prefab, Dictionary<Vector3Int, Transform> objectDictionary)
    {
        Transform newObj = Instantiate(prefab, placeholderSelector.position, Quaternion.identity);
        objectDictionary.Add(gridPosition, newObj);
        newObj.parent = transform;
    }

    /// <summary>
    /// Removes an object from a specified position on the grid.
    /// </summary>
    private void RemoveObject(Vector3Int gridPosition, Dictionary<Vector3Int, Transform> objectDictionary)
    {
        if (objectDictionary.TryGetValue(gridPosition, out Transform objToRemove))
        {
            objectDictionary.Remove(gridPosition);
            Destroy(objToRemove.gameObject);
        }
    }

    /// <summary>
    /// Helper method to clear and destroy all objects in a dictionary.
    /// </summary>
    private void ClearDictionary(Dictionary<Vector3Int, Transform> dictionary)
    {
        foreach (var item in dictionary.Values)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        dictionary.Clear();
    }

    #endregion
}
