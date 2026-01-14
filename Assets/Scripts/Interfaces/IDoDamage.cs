using UnityEngine;
/// <summary>
/// Interface for objects that can do damage.
/// </summary>
public interface IDoDamage {
    GameObject ImpactEffect { get; }
    int Damage { get; }
    public void ApplyImpactEffect(GameObject target, Vector3 hitPoint, Vector3 hitNormal);
}
