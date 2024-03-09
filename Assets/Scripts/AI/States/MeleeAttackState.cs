using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = ("United/AI/MeleeAttackState"))]
public class MeleeAttackState : AIState
{
    
    [SerializeField] private float damageDelay = 1f; // Delay for animation
    private IEnumerator attack;
    Vector3 lookingDir;
    AIController _controller;
    public override void StartState(AIController controller)
    {
        _controller = controller;
        controller.AttackTimer = 0;
        StartAnimation(controller);
        attack = PerformMeleeAttack(controller);
        PerformMeleeAttack(controller);
        controller.SetLastPosition();
        
    }

    public override void UpdateState(AIController controller)
    {
        if (controller.CanAttack())
        {
            controller.AttackTimer = 0;
            StartAnimation(controller);

            controller.StartCoroutine(attack);
            controller.SetLastPosition();

        }
    }
    public override void ExitState(AIController controller)
    {

        controller.GetNavMeshAgent().ResetPath();
    }

    private void StartAnimation(AIController controller)
    {
        lookingDir = (controller.GetTarget().position - controller.GetLastPosition()).normalized;
        if (lookingDir.x < 0)
        {
            controller.FlipCharacterToLeft();
        }
        else
        {
            controller.FlipCharacterToRight();
        }
        if (!controller.GetAnimator().GetNextAnimatorStateInfo(0).IsName(animationName.ToString()))
        {
            controller.GetAnimator().CrossFade(animationName.ToString(), 0.1f);
        }
    }

    private IEnumerator PerformMeleeAttack(AIController controller)
    {
        
        
        yield return new WaitForSeconds(damageDelay);

        Vector3 sphereSpawnPoint = Vector3.zero;
        float distanceToTarget = Vector3.Distance(controller.GetLastPosition(), controller.GetTarget().position);

        //If target out of range perform attack at max range
        if (distanceToTarget >= controller.characterStats.AttackRange)
        {
            sphereSpawnPoint = controller.GetLastPosition() + lookingDir * controller.characterStats.AttackRange;
        }
        //otherwise perform attack closer
        else
        {
            sphereSpawnPoint = controller.GetLastPosition() + lookingDir * distanceToTarget;
        }

        //Check for colliders
        Collider[] colliders = Physics.OverlapSphere(sphereSpawnPoint, controller.characterStats.AttackHitboxSize, LayerMask.NameToLayer("Player") << 1, QueryTriggerInteraction.UseGlobal); 
        if(colliders.Length > 0) 
        {
            if (colliders[0].TryGetComponent(out PlayerController player))
            {
                player.TakeDamage(controller.characterStats.Damage);
            }
        }
            

        yield return null;
    }


    public override bool CanChangeToState(AIController controller)
    {
        float distanceToTarget = Vector3.Distance(controller.GetTarget().position, controller.transform.position);

        //If can attack and in range then true
        return (distanceToTarget <= controller.characterStats.AttackRange && controller.CanAttack());
    }
}
