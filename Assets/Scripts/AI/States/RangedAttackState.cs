using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = ("United/AI/RangedAttackState"))]
public class RangedAttackState : AIState
{
    [SerializeField] private GameObject _P_Bullet;
    [SerializeField] private float spawnBulletDelay = 1f; // Delay for animation
    Vector3 lookingDir;
    private IEnumerator attack;

    public override void StartState(AIController controller)
    {
        attack = PerformRangedAttack(controller);
        controller.AttackTimer = 0;
        StartAnimation(controller);
        controller.StartCoroutine(attack);
        controller.SetLastPosition();

    }

    public override void UpdateState(AIController controller)
    {
        if(controller.CanAttack())
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

    private IEnumerator PerformRangedAttack(AIController controller)
    {
        yield return new WaitForSeconds(spawnBulletDelay);

        GameObject bullet =  Instantiate(_P_Bullet, controller.bulletSpawnPoint.position, Quaternion.identity);
        //Apply force inside bullet script

        yield return null;
    }
  
    public override bool CanChangeToState(AIController controller)
    {
        float distanceToTarget = Vector3.Distance(controller.GetTarget().position, controller.transform.position);

        return (distanceToTarget <= controller.characterStats.AttackRange && controller.CanAttack());
    }
}
