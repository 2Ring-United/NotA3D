using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [HideInInspector] public PlayerInventory Inventory;
    public int MaxHealth = 10;
    [HideInInspector] public int currentHealth = 6;
    public float Speed = 5.0f;

    private int _defaultMaxHealth;
    private CharacterController _characterController;

    Vector3 move;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        Inventory = GetComponent<PlayerInventory>();
        _defaultMaxHealth = MaxHealth;

    }

    void Start()
    {

    }


    void Update()
    {
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _characterController.Move(move * Time.deltaTime * Speed);

    }


    

    public short UseWeapon<T, M>(T weapon, M targetedEnemy) 
    {
        if( weapon == null ) 
        { 
            Debug.Log("Weapon is null!"); return -10; 
        }

        /* if(targetedEnemy.gotHit)
         {
            Debug.Log("Enemy hit!");
            if(enemy.health <= 0)
             {
                EnemyDie(0);
                return 1;
             }
             return 1;
         }
        */
        return 0;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
            PlayerDeath();
    }

    public void PlayerDeath()
    {

        Destroy(this);
        Debug.Log("U died");
    }

}
