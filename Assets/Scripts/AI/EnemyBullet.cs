using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    private float damage = 1;
    

    public void SetDamage(float value)
    {
        damage = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnHitPlayer();
            return;
        }
        
        OnHitGround();
        
    }


    private void OnHitPlayer()
    {
        GameManager.Instance.PlayerController.TakeDamage(damage);
        //spawn sound
        //spawn particles
        Destroy(gameObject);
    }
    private void OnHitGround()
    {
        //spawn sound
        //spawn particles
        Destroy(gameObject);

    }
}
