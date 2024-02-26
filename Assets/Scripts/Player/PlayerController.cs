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

public class PlayerController : MonoBehaviour, IDataPersistence
{
    public int MaxHealth = 10;
    [HideInInspector] public PlayerInventory Inventory;
    [HideInInspector] public int currentHealth = 6;
    [HideInInspector] public int currentArmor = 2;

    public float Speed = 5.0f;
    private float attackRadius = 3f;
    private int _defaultMaxHealth;
    private CharacterController _characterController;
    [HideInInspector] bool isUsingActiveWeapon = false;
    [HideInInspector] bool canAttack = true;
    [HideInInspector] float attackCD = 1f;
    [HideInInspector] float attackTimer = 1f;
    Vector3 move;
    LookingDirection lookDirection;

    public void LoadData(GameData gameData)
    {
        currentHealth = gameData.PlayerHealth;
        currentArmor = gameData.PlayerArmor;
    }

    public void SaveData(GameData gameData)
    {
        gameData.PlayerHealth = currentHealth;
        gameData.PlayerArmor = currentArmor;
    } 

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        Inventory = GetComponent<PlayerInventory>();
        _defaultMaxHealth = MaxHealth;

    }

    void Start()
    {
        //change to event OnEquip
        attackCD = Inventory.GetWeapon().AttackCooldownTime;
    }


    void Update()
    {
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        _characterController.Move(move * Time.deltaTime * Speed);
        float attackHorizontal = Input.GetAxis("AttackHorizontal");
        float attackVertical = Input.GetAxis("AttackVertical");

        Vector2 direction = new Vector2(attackHorizontal, attackVertical);

        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if(attackTimer >= attackCD)
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
        //For meele
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

        Debug.Log("Using weapon");


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

    public void TakeDamage(int damage)
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

        if(currentHealth <= 0)
            PlayerDeath();
    }

    public void PlayerDeath()
    {

        Destroy(this);
        Debug.Log("U died");
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
