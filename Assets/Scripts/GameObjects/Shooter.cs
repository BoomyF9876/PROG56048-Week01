using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Player shooter
/// </summary>
public sealed class Shooter : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The muzzle position.")]
    [SerializeField] private Transform muzzle;
    [Tooltip("The animator.")]
    [SerializeField] private Animator animator;

    [Header("Animation")]
    [Tooltip("The shoot trigger.")]
    [SerializeField] private string shootTrigger = "Shoot";

    [Header("Projectile")]
    [Tooltip("The bullet prefab.")]
    [SerializeField] private Rigidbody bulletPrefab;  
    [Tooltip("The fire cooldown.")]
    [SerializeField] private float fireCooldown = 0.25f;

    private int shootHash;
    private float nextFireTime;
    private IWeapon weapon;
    private void Awake()
    {
        if (muzzle == null) muzzle = transform;
        if (animator == null) animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogError($"[{nameof(Shooter)}] Animator not found.");
        shootHash = Animator.StringToHash(shootTrigger);
    }

    private void Update()
    {
        if(GameManager.Instance.IsGamePlaying() && Mouse.current.leftButton.wasPressedThisFrame) {
            if (Time.time < nextFireTime) return;
            if (bulletPrefab != null && muzzle != null)
            {
                Rigidbody bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
                nextFireTime = Time.time + fireCooldown;
                animator.SetTrigger(shootHash);
            }
        }
    }

    public void Shoot()
    {
        if (Time.time < nextFireTime) return;

        //if (powerUpManager != null && powerUpManager.GetCurrentWeapon() != null)
        //{
        //    powerUpManager.GetCurrentWeapon().Fire();

        //    nextFireTime = Time.time + fireCooldown;
        //    if (animator != null) animator.SetTrigger(shootHash);
        //}
    }
}
