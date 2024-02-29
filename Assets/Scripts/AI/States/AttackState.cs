using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = ("United/AI/AttackState"))]
public class AttackState : AIState
{
    [SerializeField] AnimationNames attackAnimation = AnimationNames.ATTACK;
    [SerializeField] private float attackTime = 2f;
    [SerializeField] private float damageDelay = 1f; // Delay for animation
    [SerializeField] private float distanceToTarget = 1f;
    public override void StartState(AIController controller)
    {
        if (controller.AttackTimer >= attackTime)
        {
            controller.AttackTimer = 0;
            StartAnimation(controller);
            if (controller.isRanged)
            {
                PerformRangedAttack(controller);
            }
            else
            {
                PerformMeleeAttack(controller);
            }
        }

    }

    public override void UpdateState(AIController controller)
    {
        if(controller.AttackTimer >= attackTime)
        {
            controller.AttackTimer = 0;
            StartAnimation(controller);
            if (controller.isRanged)
            {
                PerformRangedAttack(controller);
            }
            else
            {
                PerformMeleeAttack(controller);
            }
        }
    }
    public override void ExitState(AIController controller)
    {
        
        controller.GetNavMeshAgent().ResetPath();
    }

    private void StartAnimation(AIController controller)
    {
        Vector3 dir = (controller.GetTarget().position - controller.GetLastPosition()).normalized;
        if (dir.x < 0)
        {
            controller.FlipCharacterToLeft();
        }
        else
        {
            controller.FlipCharacterToRight();
        }
        if (!controller.animator.GetNextAnimatorStateInfo(0).IsName(attackAnimation.ToString()))
        {
            controller.animator.CrossFade(attackAnimation.ToString(), 0.1f);
        }
    }

    private void PerformMeleeAttack(AIController controller)
    {

    }
    private void PerformRangedAttack(AIController controller)
    {

    }
    /*
    private void SetRandomDestination(AIController controller)
    {
        // Generate a random point around the initial position
        Vector3 randomOffset = Random.insideUnitSphere * Random.Range(startDistance, maxDistance);
        Vector3 randomPosition = controller.initialPosition + randomOffset;
        Debug.Log(randomOffset + " | " + randomPosition);

        // Find the nearest point on the NavMesh to the random position
        NavMeshHit hit;
        Vector3 dir;
        if (NavMesh.SamplePosition(randomPosition, out hit, maxDistance, NavMesh.AllAreas))
        {
            dir = (hit.position - controller.GetLastPosition()).normalized;
            if (dir.x < 0)
            {
                controller.FlipCharacterToLeft();
            }
            else
            {
                controller.FlipCharacterToRight();
            }
            controller.IdleTimer = 0f;
            controller.SetIsIdle(false);
            controller.GetNavMeshAgent().SetDestination(hit.position);
        }
        if (!controller.animator.GetNextAnimatorStateInfo(0).IsName(walkAniamtion.ToString()))
        {
            controller.animator.CrossFade(walkAniamtion.ToString(), 0.1f);
        }
    }
    */
    public override bool CanChangeState(AIController controller)
    {
        return Vector3.Distance(controller.GetTarget().position, controller.transform.position) <= controller.AttackRange;
    }
}
