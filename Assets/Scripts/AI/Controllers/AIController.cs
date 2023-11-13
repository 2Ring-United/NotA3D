using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] AIState[] aIStates;

    [Header("Character Stats")]
    public float StoppingDistance = 0.5f;
    public float WalkSpeed;
    public float RunSpeed;
    public float Damage;
    public float AttackRange;
    public float AttackDotRange;
    public float AttackSpeed;
    NavMeshAgent navMeshAgent;
    [Header("Animator")]
    public Animator animator;

    AIState previousState;
    AIState currentState;
    AIState nextState;
    Transform currentTarget;
    Vector3 lastPosition;

    private bool isIdle = false;
    public float IdleTimer = 0f;
    public float AttackTimer = 0f;

    public Vector3 initialPosition;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
        lastPosition = initialPosition;
        navMeshAgent.speed = WalkSpeed;
        navMeshAgent.stoppingDistance = StoppingDistance;
        ChangeState();
    }

    virtual protected void Start()
    {

    }

    virtual protected void Update()
    {

        IdleTimer += Time.deltaTime;
        AttackTimer += Time.deltaTime;
        if (currentState != null)
        {
            currentState.UpdateState(this);
            ChangeState();
        }
    }

    public void ChangeState()
    {
        nextState = GetNextState();

        if (nextState == null) { return; }
        if (currentState != null && !currentState.CanExitState(this))
        {
            return;
        }
        if (nextState == currentState)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        if (nextState != null && nextState != currentState)
        {
            previousState = currentState;
            nextState.StartState(this);
            currentState = nextState;

        }
    }

    private AIState GetNextState()
    {
        List<AIState> states = new List<AIState>();
        StateWeight highestWeight = StateWeight.Lowest;

        // Find the highest state weight
        foreach (var state in aIStates)
        {
            if (state.CanChangeState(this))
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
            if (state.CanChangeState(this) && state.StateWeight == highestWeight)
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
    public AIState GetPreviousState()
    {
        return previousState;
    }
    public AIState GetCurrentState()
    {
        return currentState;
    }
    public AIState CheckNextState()
    {
        return nextState;
    }

    public Transform GetTarget()
    {
        return currentTarget;
    }
    public NavMeshAgent GetNavMeshAgent()
    {
        return navMeshAgent;
    }
    public bool IsIdle()
    {
        return isIdle;
    }
    public void SetIsIdle(bool isIdle)
    {
        this.isIdle = isIdle;
    }
    public Vector3 GetLastPosition()
    {
        return lastPosition;
    }
    public void SetLastPosition()
    {
        lastPosition = transform.position;
    }

    public bool CanAttack()
    {
        return AttackTimer > AttackSpeed;
    }
 
    public void FlipCharacterToLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1 );
    }
    public void FlipCharacterToRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

}
