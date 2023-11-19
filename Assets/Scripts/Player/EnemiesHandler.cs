using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesHanlder : MonoBehaviour
{
   public static int AllEnemiesCount = 0;

   protected float health;
   
   public bool IsAlive = false;

}
    

public class Zombie : EnemiesHanlder
{

    public Zombie()
    {
        EnemiesHanlder.AllEnemiesCount++;
        this.health = 10;
        this.IsAlive = true;
    }

    public void Destroy()
    {
        Destroy(this);  

        EnemiesHanlder.AllEnemiesCount--;

        Debug.Log("Destroyed foe: Zombie");

        this.IsAlive = false;

        throw new System.NotImplementedException();
    }

    public void Attack()
    {
        Debug.Log("Zombiee attacked");
        throw new System.NotImplementedException();
    }
}


public class SpawnEnemies : MonoBehaviour
{
  public GameObject zombiePrefab;

    void Update()
    {
        // Sprawdzanie czy klawisz "L" zosta³ naciœniêty
        if (Input.GetKeyDown(KeyCode.L))
        {


            GameObject go3 =  new GameObject("Zombiee", typeof(Rigidbody), typeof(BoxCollider));

            Debug.Log("Zombiee spawned");
        }
    }
}