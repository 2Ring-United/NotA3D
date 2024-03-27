using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnManager : MonoBehaviour
{
    public static UnityEvent OnEnemyDeath;

    
    public static EnemySpawnManager _instance;
    public static EnemySpawnManager Instance { get { return _instance; } private set { } }


    [SerializeField] List<GameObject> P_Enemies;

    [SerializeField] List<Transform> spawnPoints;

    [SerializeField] List<GameObject> spawnedEnemies;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        spawnedEnemies = new List<GameObject>();
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        foreach(Transform t in spawnPoints)
        {
            int rand = Random.Range(0, 101);
            if(rand <= 50 ) 
            {
                rand = Random.Range(0, P_Enemies.Count);
                spawnedEnemies.Add(Instantiate(P_Enemies[rand], t));
            }
            
        }
    }

    public void InvokeEnemyDeathEvent()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath.Invoke();
            Debug.Log("OnEnemyDeath Event invoked");
        }
    }
}
