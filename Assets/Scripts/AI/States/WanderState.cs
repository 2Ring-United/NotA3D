using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = ("United/AI/WanderState"))]
public class WanderState : AIState
{
    [SerializeField] AnimationNames idleAnimation = AnimationNames.IDLE;
    [SerializeField] AnimationNames walkAniamtion = AnimationNames.WALK;
    [SerializeField] private float startDistance = 5f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float idleTime = 5f;


    public override void StartState(AIController controller)
    {
        if (!controller.animator.GetNextAnimatorStateInfo(0).IsName(walkAniamtion.ToString()))
        {
            controller.animator.CrossFade(idleAnimation.ToString(), 0.1f);
        }
        // Set the initial destination to the current position
        SetRandomDestination(controller);
        controller.SetLastPosition();
    }

    public override void UpdateState(AIController controller)
    {
        if (controller.IsIdle())
        {

            if (controller.IdleTimer >= idleTime)
            {
                // Idle time is over, set new random destination
                SetRandomDestination(controller);
                controller.SetIsIdle(false);
            }
        }
        else
        {
            
            // Check if the character has reached the current destination
            if (!controller.GetNavMeshAgent().pathPending && controller.GetNavMeshAgent().remainingDistance <= controller.GetNavMeshAgent().stoppingDistance)
            {
                if (!controller.GetNavMeshAgent().hasPath || controller.GetNavMeshAgent().velocity.sqrMagnitude <= 0.5f)
                {
                    // AIController has arrived at the destination and stopped moving
                    controller.IdleTimer = 0f;
                    controller.SetIsIdle(true);
                    if (!controller.animator.GetNextAnimatorStateInfo(0).IsName(idleAnimation.ToString()))
                    {
                        controller.animator.CrossFade(idleAnimation.ToString(), 0.1f);
                    }
                }
            }
            else if (controller.GetNavMeshAgent().velocity.magnitude < 0.1f)
            {
                // Check if the agent's position has changed significantly since the last check
                if (Vector3.Distance(controller.GetNavMeshAgent().transform.position, controller.GetLastPosition()) < 0.1f)
                {
                    Debug.Log("NavMeshAgent is stuck!");
                    controller.IdleTimer = 0f;
                    controller.SetIsIdle(true);


                    // You can add additional actions here if needed, like trying to set a new destination or reset the agent.
                }


            }
            controller.SetLastPosition();
        }
    }
    public override void ExitState(AIController controller)
    {
        // Reset the destination when exiting the state
        controller.GetNavMeshAgent().ResetPath();
    }

    private void SetRandomDestination(AIController controller)
    {
        // Generate a random point around the initial position
        Vector3 randomOffset = Random.insideUnitSphere * Random.Range(startDistance, maxDistance);
        Vector3 randomPosition = controller.GetLastPosition() + randomOffset;

        // Find the nearest point on the NavMesh to the random position
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, maxDistance, NavMesh.AllAreas))
        {
            controller.IdleTimer = 0f;
            controller.SetIsIdle(false);
            controller.GetNavMeshAgent().SetDestination(hit.position);
        }
        if (!controller.animator.GetNextAnimatorStateInfo(0).IsName(animationName.ToString()))
        {
            controller.animator.CrossFade(animationName.ToString(), 0.1f);
        }
    }

    public override bool CanChangeState(AIController controller)
    {
        return controller.GetTarget() == null;
    }
}
