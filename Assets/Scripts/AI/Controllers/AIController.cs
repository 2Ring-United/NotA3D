using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
public class AIController : MonoBehaviour
{
    [SerializeField] AIState[] aIStates;


    [Header("Character Stats")]
    public AIStats characterStats;
    private float _health;
    NavMeshAgent _navMeshAgent;
    [Header("Animator")]
    [SerializeField] Animator _animator;

    AIState _previousState;
    AIState _currentState;
    AIState _nextState;

    Transform _currentTarget;

    [SerializeField] public  Transform bulletSpawnPoint;

    private bool _isIdle = false;
    [HideInInspector] public float IdleTimer = 0f;
    [HideInInspector] public float AttackTimer = 0f;

    public Vector3 initialPosition;
    Vector3 _lastPosition;


    private void Awake()
    {
        _health = characterStats.MaxHealth;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
        _lastPosition = initialPosition;
        _navMeshAgent.speed = characterStats.WalkSpeed;
        _navMeshAgent.stoppingDistance = characterStats.StoppingDistance;
    }

    virtual protected void Start()
    {
        _currentTarget = FindObjectOfType<PlayerController>().transform;
        ChangeState();

    }

    virtual protected void Update()
    {

        IdleTimer += Time.deltaTime;
        AttackTimer += Time.deltaTime;
        
        if (_currentState != null)
        {
            _currentState.UpdateState(this);
            ChangeState();
        }
    }

    public void ChangeState()
    {
        _nextState = GetNextState();

        if (_nextState == null) { return; }
        if (_currentState != null && !_currentState.CanExitState(this))
        {
            return;
        }
        if (_nextState == _currentState)
        {
            return;
        }

        if (_currentState != null)
        {
            _currentState.ExitState(this);
        }

        if (_nextState != null && _nextState != _currentState)
        {
            _previousState = _currentState;
            _nextState.StartState(this);
            _currentState = _nextState;

        }
    }

    private AIState GetNextState()
    {
        List<AIState> states = new List<AIState>();
        StateWeight highestWeight = StateWeight.Lowest;

        // Find the highest state weight
        foreach (var state in aIStates)
        {
            if (state.CanChangeToState(this))
            {
                if (state.StateWeight > highestWeight)
                {
                    highestWeight = state.StateWeight;
                }
            }
        }

        // Collect all states with the highest weight
        foreach (var state in aIStates)
        {
            if (state.CanChangeToState(this) && state.StateWeight == highestWeight)
            {
                states.Add(state);
            }
        }

        // Choose a random state from the collected states
        if (states.Count > 0)
        {
            int randomIndex = Random.Range(0, states.Count);
            return states[randomIndex];
        }

        return null; // Return null if no state is found


    }

    public void TakeDamage(float value)
    {
        _health -= value;
        if(_health <= 0)
        {
            EnemySpawnManager.Instance.InvokeEnemyDeathEvent();
            Invoke("OnDeath", 0.2f);
        }
    }

    public void OnDeath()
    {
        //Spawn particles
        //Spawn drop
        //Sound
        //maybe object pooling if needed
        Destroy(gameObject);
    }
    public AIState GetPreviousState()
    {
        return _previousState;
    }
    public AIState GetCurrentState()
    {
        return _currentState;
    }
    public AIState CheckNextState()
    {
        return _nextState;
    }
    public Animator GetAnimator() 
    {
        return _animator;
    }
    public Transform GetTarget()
    {
        return _currentTarget;
    }
    public NavMeshAgent GetNavMeshAgent()
    {
        return _navMeshAgent;
    }
    public bool IsIdle()
    {
        return _isIdle;
    }
    public void SetIsIdle(bool isIdle)
    {
        this._isIdle = isIdle;
    }
    public Vector3 GetLastPosition()
    {
        return _lastPosition;
    }
    public void SetLastPosition()
    {
        _lastPosition = transform.position;
    }

    public bool CanAttack()
    {
        return AttackTimer > characterStats.AttackSpeed;
    }
 
    public void FlipCharacterToLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1 );
    }
    public void FlipCharacterToRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }


    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.yellow; 
        Vector3 lookingDir = (GetTarget().position - GetLastPosition()).normalized;
        Vector3 sphereSpawnPoint = Vector3.zero;
        float disToTarget = Vector3.Distance(transform.position, GetTarget().position);
        if (disToTarget >= characterStats.AttackRange) 
        {
            sphereSpawnPoint = transform.position + lookingDir * characterStats.AttackRange;

        }
        else
        {
            sphereSpawnPoint = transform.position + lookingDir * disToTarget;
        }
        sphereSpawnPoint.y = 0.5f;
        Gizmos.DrawWireSphere(sphereSpawnPoint, characterStats.AttackHitboxSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, characterStats.AttackRange);
    }
}
