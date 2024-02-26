using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;



public enum AttackDirection
{
    North,
    South,
    East,
    West
}

[Serializable] public class AttackDirectionDict : SerializableDictionary<AttackDirection, GameObject> { }

public class PlayerController : MonoBehaviour
{
    public AttackDirectionDict AttackDirectionToWeaponSlot;

    [HideInInspector] 
    public PlayerInventory Inventory;
    
    public int MaxHealth = 10;
    [HideInInspector] public int currentHealth = 6;
    public float Speed = 5.0f;

    private int _defaultMaxHealth;
    private CharacterController _characterController;

    Vector3 move;
    Vector3 _prevMove;

    Ray _lastAttackRay;

    private bool _attackInProgress;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        Inventory = GetComponent<PlayerInventory>();
        _defaultMaxHealth = MaxHealth;

    }

    void Start()
    {

    }

    IEnumerator attackCoroutine(AttackDirection dir)
    {
        _attackInProgress = true;
        AttackDirectionToWeaponSlot[dir].gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Inventory.CurrentWeapon.sprite;
        RaycastHit hit;
        _lastAttackRay = new Ray(AttackDirectionToWeaponSlot[dir].gameObject.transform.position, AttackDirectionToWeaponSlot[dir].gameObject.transform.forward);
    
        if (Physics.Raycast(_lastAttackRay, out hit, 1.5f))
        {
            if (hit.collider.gameObject.CompareTag("Game.Enemy"))
            {
                UseWeapon(Inventory.CurrentWeapon, hit.collider.gameObject);
            }
        }

        yield return new WaitForSeconds(0.5f);
        AttackDirectionToWeaponSlot[dir].gameObject.GetComponentInChildren<SpriteRenderer>().sprite = null;
        _attackInProgress = false;
    }

    void Attack(AttackDirection dir)
    {
        if (_attackInProgress)
            return;

        StartCoroutine(attackCoroutine(dir));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Inventory.UseWeapon(Inventory.CurrentWeapon, null);

            if (_prevMove.x > 0)
            {
                Attack(AttackDirection.East);
            }
            else if (_prevMove.x < 0)
            {
                Attack(AttackDirection.West);
            }
            else if (_prevMove.z > 0)
            {
                Attack(AttackDirection.North);
            }
            else if (_prevMove.z < 0)
            {
                Attack(AttackDirection.South);
            }

        }

        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (move != Vector3.zero)
        {
            _prevMove = move;
        }

        _characterController.Move(move * Time.deltaTime * Speed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_lastAttackRay);
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
