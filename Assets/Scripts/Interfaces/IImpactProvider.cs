using UnityEngine;

public interface IImpactProvider
{
    public void ApplyImpactEffect(GameObject target, Vector3 hitPoint, Vector3 hitNormal);
}
