using TMPro;
using UnityEngine;

public class DamageNumber: MonoBehaviour
{
    private TextMeshPro textMesh;

    public void Initialize(int damage, Color color)
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.color = color;
        textMesh.text = damage.ToString();
    }
}
