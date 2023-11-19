using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*Notatki 19.11.2023 Poczytaj ile hp ma tracic przy kolizji z bosem*/
public class PlayerController : MonoBehaviour
{
    public short PlayerHealth = 6;
    public float PlayerSpeed = 5.0f;

    private CharacterController _CharacterController;

    Vector3 move;

    void Start()
    {
        _CharacterController = GetComponent<CharacterController>();
    }


    void Update()
    {
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //Poruszanie postaci na WSAD i strzalki
        _CharacterController.Move(move * Time.deltaTime * PlayerSpeed);

        //Enemy musi miec komponent Collider, oraz Maske NormalEnemy,   //Promien 1f                            //Pierwszy, najblizszy enemy; brak kolizji = zwraca null
       Collider enemyCollider = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("NormalEnemy")).FirstOrDefault();

        if (enemyCollider != null)
        {
            TakeDamage(1);
        }
    }

    public short UseWeapon<T, M>(T weapon, M targetedEnemy) //Przekazanie dowolnej klasy, bo nie ma jeszcze klasy weapon; zwraca 1 przy uderzeniu i 0 przy missnieciu
    {
        if( weapon == null ) { Debug.Log("Weapon is null!"); return -10; }

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

    public void TakeDamage(short damage)
    {
        this.PlayerHealth -= damage;

        if(this.PlayerHealth <= 0)
            PlayerDeath();
    }

    public void PlayerDeath()
    {
        //Resetowanie Postaci do jej miejsca etc..
        Destroy(this);
        Debug.Log("U died");
    }

}
