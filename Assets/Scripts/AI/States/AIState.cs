using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateWeight
{
    Lowest,
    Low,
    Medium,
    High,
    Highest
}
public abstract class AIState : ScriptableObject
{
    public AnimationNames animationName;
    public StateWeight StateWeight;
    //[SerializeField] float stateTime = 0f;
    public abstract void StartState(AIController controller);

    public abstract void UpdateState(AIController controller);

    public abstract void ExitState(AIController controller);

    public virtual bool CanExitState(AIController controller)
    {
        return true;
    }
    public virtual bool CanChangeToState(AIController controller)
    {
        return true;
    }
}
