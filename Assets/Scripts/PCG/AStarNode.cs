using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Represents a single node in the A* pathfinding grid.
/// Stores pathfinding costs and neighbor references for navigation.
/// </summary>
public class AStarNode : MonoBehaviour
{
    #region Serialized Fields

    [Header("Debug Display")]
    [Tooltip("TextMeshPro component for displaying G cost.")]
    [SerializeField] private TextMeshPro gCostDisplay;

    [Tooltip("TextMeshPro component for displaying F cost.")]
    [SerializeField] private TextMeshPro fCostDisplay;

    [Tooltip("TextMeshPro component for displaying H cost.")]
    [SerializeField] private TextMeshPro hCostDisplay;

    #endregion

    #region Public Properties

    /// <summary>
    /// The position of this node in grid coordinates.
    /// </summary>
    public Vector3Int GridPosition { get; set; }

    /// <summary>
    /// G Cost: The actual cost from the start node to this node.
    /// </summary>
    public int GCost { get; private set; }

    /// <summary>
    /// H Cost: The heuristic (estimated) cost from this node to the end node.
    /// </summary>
    public int HCost { get; private set; }

    /// <summary>
    /// F Cost: The total cost (G + H). Used to determine which node to evaluate next.
    /// </summary>
    public int FCost => GCost + HCost;

    /// <summary>
    /// Reference to the node this one came from in the path.
    /// Used for retracing the final path.
    /// </summary>
    public AStarNode CameFromNode { get; set; }

    /// <summary>
    /// List of neighboring nodes that can be traversed from this node.
    /// </summary>
    public List<AStarNode> Neighbors { get; private set; } = new List<AStarNode>();

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets the G and H costs for this node and updates the debug display.
    /// </summary>
    /// <param name="gCost">The cost from the start node to this node.</param>
    /// <param name="hCost">The estimated cost from this node to the end node.</param>
    public void CalculateCosts(int gCost, int hCost)
    {
        GCost = gCost;
        HCost = hCost;
        UpdateDebugDisplay();
    }

    /// <summary>
    /// Resets the node's pathfinding state for a new search.
    /// </summary>
    public void ResetNode()
    {
        GCost = 0;
        HCost = 0;
        CameFromNode = null;
        UpdateDebugDisplay();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Updates the TextMeshPro debug displays with current cost values.
    /// </summary>
    private void UpdateDebugDisplay()
    {
        if (gCostDisplay != null)
        {
            gCostDisplay.text = GCost.ToString();
        }

        if (hCostDisplay != null)
        {
            hCostDisplay.text = HCost.ToString();
        }

        if (fCostDisplay != null)
        {
            fCostDisplay.text = FCost.ToString();
        }
    }

    #endregion
}
