using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Switches the level when the player enters the trigger.
/// </summary>
public class LevelSwitcher : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("The index of the current level")]
    [SerializeField] private int _currentLevelIndex;
    [Tooltip("The name of the material of the current level")]
    [SerializeField] private string _currentLevelGroundMaterial;
    [Tooltip("The index of the next level")]
    [SerializeField] private int _nextLevelIndex;

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

    /// <summary>
    /// Checks the material below the player
    /// </summary>
    /// <returns>The name of the material below the player</returns>
    private string CheckMaterialBelow(Vector3 position)
    {
        return "";
    }
}
