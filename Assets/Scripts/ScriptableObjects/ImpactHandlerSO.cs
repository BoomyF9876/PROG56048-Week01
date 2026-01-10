using UnityEngine;

[CreateAssetMenu(fileName = "ImpactHandlerSO", menuName = "ScriptableObjects/ImpactHandlerSO")]
public class ImpactHandlerSO : ScriptableObject
{
    [Header("Impact Effect")]
    [Tooltip("Impact effect to be instantiated on collision")]
    [SerializeField] private GameObject impactEffect;
    [Tooltip("Life span of the impact effect")]
    [SerializeField] private float impactEffectLifeSpan = 10f;

    public GameObject ImpactEffect => impactEffect;

    /// <summary>
    /// Applies impact effect to the target
    /// </summary>
    /// <param name="target">The target to apply the impact effect to</param>
    /// <param name="hitPoint">The point of impact</param>
    /// <param name="hitNormal">The normal of the impact</param>
    public void ApplyImpactEffect(GameObject target, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (impactEffect == null || target == null) return;
        GameObject impact = Instantiate(impactEffect, hitPoint + (hitNormal * 0.001f), Quaternion.LookRotation(hitNormal));
        impact.transform.parent = target.transform;
        Destroy(impact, impactEffectLifeSpan);
    }
}
