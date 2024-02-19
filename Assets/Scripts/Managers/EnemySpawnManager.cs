using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnManager : MonoBehaviour
{
    public static UnityEvent OnEnemyDeath;


    public static EnemySpawnManager Instance;

    public List<Transform> SpawnPoints;


    public void InvokeEnemyDeathEvent()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath.Invoke();
            Debug.Log("OnEnemyDeath Event invoked");
        }
    }
}
