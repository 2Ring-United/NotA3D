using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerController PlayerController;
    public Collider HitboxCollider;


    private void Awake()
    {
        PlayerController = GetComponentInParent<PlayerController>();
        HitboxCollider = GetComponent<CapsuleCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            float dmg = collision.collider.GetComponent<AIController>().Damage;
            PlayerController.TakeDamage(dmg);
        }
    }

    public void SetCollsionEnabled(bool enabled)
    {
        HitboxCollider.enabled = enabled;
    }

}
