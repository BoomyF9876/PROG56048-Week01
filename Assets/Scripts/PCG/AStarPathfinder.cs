using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages pathfinding operations using the A* algorithm.
/// Supports both Manhattan and Chebyshev (diagonal) distance heuristics.
/// </summary>
public class AStarPathfinder : MonoBehaviour
{
    #region Enums

    /// <summary>
    /// Distance calculation modes for A* heuristic.
    /// </summary>
    public enum DistanceMode
    {
        /// <summary>Manhattan distance (4-directional, no diagonals).</summary>
        Manhattan = 1,

        /// <summary>Chebyshev distance (8-directional, with diagonals).</summary>
        Chebyshev = 2
    }

    #endregion

    #region Serialized Fields

    [Header("Configuration")]
    [Tooltip("Reference to the GridManager for grid data access.")]
    [SerializeField] private GridManager gridManager;

    [Tooltip("Distance calculation mode for pathfinding heuristic.")]
    [SerializeField] private DistanceMode distanceMode = DistanceMode.Manhattan;

    #endregion

    #region Private Fields

    private Dictionary<Vector3Int, AStarNode> nodesOnGrid = new Dictionary<Vector3Int, AStarNode>();

    // Pre-allocated arrays for neighbor calculations
    private static readonly Vector3Int[] CardinalDirections = new Vector3Int[]
    {
        new Vector3Int(-1, 0, 0),  // Left
        new Vector3Int(1, 0, 0),   // Right
        new Vector3Int(0, 0, 1),   // Forward
        new Vector3Int(0, 0, -1),  // Back
    };

    private static readonly Vector3Int[] DiagonalDirections = new Vector3Int[]
    {
        new Vector3Int(-1, 0, 1),  // Forward-Left
        new Vector3Int(1, 0, 1),   // Forward-Right
        new Vector3Int(-1, 0, -1), // Back-Left
        new Vector3Int(1, 0, -1),  // Back-Right
    };

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the nodes based on the grid managed by the GridManager.
    /// </summary>
    /// <param name="gridManager">The grid manager instance to use for node initialization.</param>
    public void InitializeNodes(GridManager gridManager)
    {
        ClearGrid();
        this.gridManager = gridManager;

        if (gridManager == null)
        {
            Debug.LogError($"[{nameof(AStarPathfinder)}] GridManager reference is null!");
            return;
        }

        CreateNodes();
        InitializeNodeNeighbours();
    }

    /// <summary>
    /// Finds a path from the start to the end position using the A* algorithm.
    /// </summary>
    /// <param name="start">The starting grid position.</param>
    /// <param name="end">The ending grid position.</param>
    public void FindPath(Vector3Int start, Vector3Int end)
    {
        // Validate positions
        if (!ValidatePathfindingRequest(start, end, out AStarNode startNode, out AStarNode endNode))
        {
            return;
        }

        ClearWaypoints();

        List<AStarNode> openSet = new List<AStarNode> { startNode };
        HashSet<AStarNode> closedSet = new HashSet<AStarNode>();

        while (openSet.Count > 0)
        {
            AStarNode currentNode = GetLowestFCostNode(openSet);

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            EvaluateNeighbours(currentNode, endNode, openSet, closedSet);
        }

        Debug.Log($"[{nameof(AStarPathfinder)}] No path found from {start} to {end}");
    }

    /// <summary>
    /// Resets the neighbor connections for all nodes.
    /// Call this after modifying the grid (e.g., after map regeneration).
    /// </summary>
    public void ResetNeighbours()
    {
        InitializeNodeNeighbours();
    }

    /// <summary>
    /// Clears all A* nodes from the grid.
    /// </summary>
    public void ClearGrid()
    {
        foreach (var node in nodesOnGrid.Values)
        {
            if (node != null)
            {
                Destroy(node.gameObject);
            }
        }
        nodesOnGrid.Clear();
    }

    /// <summary>
    /// Calculates the distance between two nodes using the configured distance mode.
    /// </summary>
    /// <param name="nodeA">The first node.</param>
    /// <param name="nodeB">The second node.</param>
    /// <returns>The calculated distance.</returns>
    public int GetDistance(AStarNode nodeA, AStarNode nodeB)
    {
        return GetDistance(nodeA, nodeB, distanceMode);
    }

    /// <summary>
    /// Calculates the distance between two nodes using a specified distance mode.
    /// </summary>
    /// <param name="nodeA">The first node.</param>
    /// <param name="nodeB">The second node.</param>
    /// <param name="mode">The distance calculation mode.</param>
    /// <returns>The calculated distance.</returns>
    public int GetDistance(AStarNode nodeA, AStarNode nodeB, DistanceMode mode)
    {
        int distanceX = Mathf.Abs(nodeA.GridPosition.x - nodeB.GridPosition.x);
        int distanceZ = Mathf.Abs(nodeA.GridPosition.z - nodeB.GridPosition.z);

        return mode switch
        {
            DistanceMode.Manhattan => distanceX + distanceZ,
            DistanceMode.Chebyshev => Mathf.Max(distanceX, distanceZ),
            _ => distanceX + distanceZ
        };
    }

    #endregion

    #region Private Methods - Initialization

    /// <summary>
    /// Creates A* nodes for each grid cell.
    /// </summary>
    private void CreateNodes()
    {
        int halfX = gridManager.GridXZ.x / 2;
        int halfZ = gridManager.GridXZ.z / 2;

        for (int x = -halfX; x < halfX; x++)
        {
            for (int z = -halfZ; z < halfZ; z++)
            {
                Vector3Int gridPosition = new Vector3Int(x, 0, z);
                Vector3 worldPosition = gridManager.Grid.CellToWorld(gridPosition);

                AStarNode node = Instantiate(
                    gridManager.AStarPrefab,
                    worldPosition,
                    Quaternion.identity
                ).GetComponent<AStarNode>();

                node.GridPosition = gridPosition;
                node.transform.parent = gridManager.AStarNodesParent;
                nodesOnGrid[gridPosition] = node;
            }
        }
    }

    /// <summary>
    /// Initializes neighbor connections for each node in the grid.
    /// </summary>
    private void InitializeNodeNeighbours()
    {
        foreach (var nodePair in nodesOnGrid)
        {
            Vector3Int position = nodePair.Key;
            AStarNode node = nodePair.Value;

            node.Neighbors.Clear();

            // Add cardinal direction neighbors
            AddNeighboursFromDirections(node, position, CardinalDirections);

            // Add diagonal direction neighbors
            AddNeighboursFromDirections(node, position, DiagonalDirections);
        }
    }

    /// <summary>
    /// Adds walkable neighbors from the specified direction offsets.
    /// </summary>
    private void AddNeighboursFromDirections(AStarNode node, Vector3Int position, Vector3Int[] directions)
    {
        foreach (var direction in directions)
        {
            Vector3Int neighbourPosition = position + direction;

            if (nodesOnGrid.TryGetValue(neighbourPosition, out AStarNode neighbour)
                && gridManager.IsTileWalkable(neighbourPosition))
            {
                node.Neighbors.Add(neighbour);
            }
        }
    }

    #endregion

    #region Private Methods - Pathfinding

    /// <summary>
    /// Validates that a pathfinding request can be performed.
    /// </summary>
    private bool ValidatePathfindingRequest(Vector3Int start, Vector3Int end,
        out AStarNode startNode, out AStarNode endNode)
    {
        startNode = null;
        endNode = null;

        if (!nodesOnGrid.TryGetValue(start, out startNode))
        {
            Debug.LogWarning($"[{nameof(AStarPathfinder)}] Start position {start} is not on the grid!");
            return false;
        }

        if (!nodesOnGrid.TryGetValue(end, out endNode))
        {
            Debug.LogWarning($"[{nameof(AStarPathfinder)}] End position {end} is not on the grid!");
            return false;
        }

        if (!gridManager.IsTileWalkable(start))
        {
            Debug.LogWarning($"[{nameof(AStarPathfinder)}] Start position {start} is not walkable!");
            return false;
        }

        if (!gridManager.IsTileWalkable(end))
        {
            Debug.LogWarning($"[{nameof(AStarPathfinder)}] End position {end} is not walkable!");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets the node with the lowest F cost from the open set.
    /// </summary>
    private AStarNode GetLowestFCostNode(List<AStarNode> openSet)
    {
        AStarNode lowestCostNode = openSet[0];

        for (int i = 1; i < openSet.Count; i++)
        {
            if (openSet[i].FCost < lowestCostNode.FCost ||
                (openSet[i].FCost == lowestCostNode.FCost && openSet[i].HCost < lowestCostNode.HCost))
            {
                lowestCostNode = openSet[i];
            }
        }

        return lowestCostNode;
    }

    /// <summary>
    /// Evaluates neighboring nodes of the current node for pathfinding.
    /// </summary>
    private void EvaluateNeighbours(AStarNode currentNode, AStarNode endNode,
        List<AStarNode> openSet, HashSet<AStarNode> closedSet)
    {
        foreach (AStarNode neighbour in currentNode.Neighbors)
        {
            if (closedSet.Contains(neighbour) || !gridManager.IsTileWalkable(neighbour.GridPosition))
            {
                continue;
            }

            int newCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);

            if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
            {
                neighbour.CalculateCosts(newCostToNeighbour, GetDistance(neighbour, endNode));
                neighbour.CameFromNode = currentNode;

                if (!openSet.Contains(neighbour))
                {
                    openSet.Add(neighbour);
                }
            }
        }
    }

    /// <summary>
    /// Retraces the path from the end node back to the start node.
    /// </summary>
    private List<AStarNode> RetracePath(AStarNode startNode, AStarNode endNode)
    {
        List<AStarNode> path = new List<AStarNode>();
        AStarNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.CameFromNode;
        }

        path.Add(startNode);
        path.Reverse();

        VisualizePath(path);
        return path;
    }

    #endregion

    #region Private Methods - Visualization

    /// <summary>
    /// Clears any existing waypoints from the grid.
    /// </summary>
    private void ClearWaypoints()
    {
        foreach (var waypoint in gridManager.WaypointsOnGrid.Values)
        {
            if (waypoint != null)
            {
                Destroy(waypoint.gameObject);
            }
        }
        gridManager.WaypointsOnGrid.Clear();
    }

    /// <summary>
    /// Visualizes the path with start, end, and waypoint markers.
    /// </summary>
    private void VisualizePath(List<AStarNode> path)
    {
        if (path.Count < 2)
        {
            return;
        }

        // Place start marker
        PlacePathMarker(path[0], gridManager.Start);

        // Place waypoint markers for intermediate nodes
        for (int i = 1; i < path.Count - 1; i++)
        {
            PlacePathMarker(path[i], gridManager.Waypoint);
        }

        // Place end marker
        PlacePathMarker(path[path.Count - 1], gridManager.End);
    }

    /// <summary>
    /// Places a path marker at the specified node's position.
    /// </summary>
    private void PlacePathMarker(AStarNode node, Transform markerPrefab)
    {
        Vector3 worldPosition = gridManager.Grid.CellToWorld(node.GridPosition);
        Transform marker = Instantiate(markerPrefab, worldPosition, Quaternion.identity);
        marker.parent = gridManager.AStarNodesParent;
        gridManager.WaypointsOnGrid[node.GridPosition] = marker;
    }

    #endregion
}
