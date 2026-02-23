using System;
using UnityEngine;

/// <summary>
/// ScriptableObject that defines tile properties used in procedural map generation.
/// Tiles can have multiple movement types (e.g., a bridge could be both Walkable and Flyable).
/// </summary>
[CreateAssetMenu(menuName = "PCG/Tile Definition", fileName = "NewTile")]
public class TileSO : ScriptableObject
{
    #region Enums

    /// <summary>
    /// Defines the movement capabilities available on a tile.
    /// Multiple types can be combined for tiles with multiple traversal options.
    /// </summary>
    [Flags]
    public enum TileTypes
    {
        /// <summary>No movement allowed on this tile.</summary>
        None = 0,

        /// <summary>Tile can be traversed on foot.</summary>
        Walkable = 1 << 0,

        /// <summary>Tile can be traversed by flying units.</summary>
        Flyable = 1 << 1,

        /// <summary>Tile can be traversed by swimming units.</summary>
        Swimmable = 1 << 2
    }

    #endregion

    #region Serialized Fields

    [Header("Tile Identity")]
    [Tooltip("Display name for this tile type.")]
    [SerializeField] private string tileName;

    [Header("Tile Properties")]
    [Tooltip("Movement types allowed on this tile.")]
    [SerializeField] private TileTypes[] tileType;

    [Tooltip("Visual prefab to instantiate for this tile.")]
    [SerializeField] private Transform prefab;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the display name of this tile.
    /// </summary>
    public string TileName => tileName;

    /// <summary>
    /// Gets the prefab used to visually represent this tile in the game world.
    /// </summary>
    public Transform Prefab => prefab;

    /// <summary>
    /// Gets the array of tile types defining the movement properties of this tile.
    /// </summary>
    public TileTypes[] TileType => tileType;

    #endregion

    #region Public Methods

    /// <summary>
    /// Checks if this tile supports a specific movement type.
    /// </summary>
    /// <param name="type">The movement type to check.</param>
    /// <returns>True if the tile supports the specified movement type.</returns>
    public bool HasType(TileTypes type)
    {
        return Array.Exists(tileType, t => t == type);
    }

    /// <summary>
    /// Checks if this tile is walkable.
    /// </summary>
    public bool IsWalkable => HasType(TileTypes.Walkable);

    /// <summary>
    /// Checks if this tile is flyable.
    /// </summary>
    public bool IsFlyable => HasType(TileTypes.Flyable);

    /// <summary>
    /// Checks if this tile is swimmable.
    /// </summary>
    public bool IsSwimmable => HasType(TileTypes.Swimmable);

    #endregion
}
