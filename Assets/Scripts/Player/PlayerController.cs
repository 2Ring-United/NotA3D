using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


enum LookingDirection
{
    North,
    East,
    South,
    West
}

public class PlayerController : MonoBehaviour
{

    [Header("Components")]
    [HideInInspector] public PlayerInventory Inventory;
    [HideInInspector] public PlayerCollision Collision;
    private CharacterController _characterController;


    [Header("PlayerStats")]
    public int MaxHealth = 10;
    public float Speed = 5.0f;

    [HideInInspector] public float currentHealth = 6;
    [HideInInspector] public float currentArmor = 2;

    [HideInInspector]public float DefaultSpeed;
    [HideInInspector]public int DefaultMaxHealth;


    [Header("Weapon")]
    bool isUsingActiveWeapon = false;
    bool canAttack = true;
    float attackTimer = 0f;
    private float attackRadius = 3f;

    Vector3 move;
    LookingDirection lookDirection;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        Inventory = GetComponent<PlayerInventory>();
        Collision = GetComponentInChildren<PlayerCollision>();
        DefaultMaxHealth = MaxHealth;
        DefaultSpeed = Speed;  
    }


    void Start()
    {


    }


    void Update()
    {

        //Use Active Weapon
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Inventory.GetActiveItem() != null)
            {
                Inventory.GetActiveItem().ActivateItem(Inventory);
            }

        }

        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        _characterController.Move(move * Time.deltaTime * Speed);
        float attackHorizontal = Input.GetAxis("AttackHorizontal");
        float attackVertical = Input.GetAxis("AttackVertical");

        Vector2 direction = new Vector2(attackHorizontal, attackVertical);

        //Attack
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if(attackTimer >= Inventory.GetWeapon().AttackCooldownTime)
            {
                canAttack = true;
            }
            return;
        }
            

        if (AttackPressed(direction) && direction.magnitude != 0)
        {
            canAttack = false;
            attackTimer = 0f;


            if (!isUsingActiveWeapon)
            {
                UseWeapon(direction);
            }
            else
            {
                UseActiveWeapon(direction);
            }
        }

    }

    bool AttackPressed(Vector2 direction)
    {
        //For melee
        if (direction.y > 0)
        {
            lookDirection = LookingDirection.North;
            return true;
        }
        else if (direction.y < 0)
        {
            lookDirection = LookingDirection.South;
            return true;
        } 
        if (direction.x > 0)
        {
            lookDirection = LookingDirection.East;
            return true;
        }
        else if(direction.x < 0)
        {
            lookDirection = LookingDirection.West;
            return true;
        }

        return false;   
    }
    

    void UseWeapon(Vector2 offset) 
    {
        if( Inventory.GetWeapon() == null ) 
        { 
            Debug.Log("Weapon is null!");
            return;
        }


        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        Vector3 spherePos = new Vector3(transform.position.x + offset.x, transform.position.y, transform.position.z+ offset.y);

        //PlayerAnimator.PlayAnim("ATTACK_ANIM");
        
        foreach(Collider col in Physics.OverlapSphere(spherePos, attackRadius, layerMask))
        {
            Debug.Log("AAA");
            if(TryGetComponent<AIController>(out AIController controller))
            {
                controller.TakeDamage(Inventory.GetWeapon().Damage);
            }
        }
       

    }

    void UseActiveWeapon(Vector2 direction)
    {

    }


    public void TakeDamage(float damage)
    {

        if(damage > currentArmor)
        {
            damage -= currentArmor;
            currentArmor = 0;
        }
        else
        {
            currentArmor -= damage;
            return;
        }
        currentHealth -= damage;
        //UI.UpdateHealthState

        if (currentHealth <= 0)
            PlayerDeath();
    }

    public void PlayerDeath()
    {

        Destroy(this);
        Debug.Log("U died");
    }

    public void HealPlayer(float amount)
    {
        currentHealth += amount;
        if(currentHealth > MaxHealth) 
        { 
            currentHealth = MaxHealth;
        }
        //UI.UpdateHealthState
    }

    public void AddArmor(float amount)
    {
        currentArmor += amount;
        //UI.UpdateHealthState
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        float attackHorizontal = Input.GetAxis("AttackHorizontal");
        float attackVertical = Input.GetAxis("AttackVertical");

        Vector3 direction = new Vector3(transform.position.x + attackHorizontal, transform.position.y,transform.position.z+ attackVertical);
        Gizmos.DrawWireSphere(direction, attackRadius);
    }
}
