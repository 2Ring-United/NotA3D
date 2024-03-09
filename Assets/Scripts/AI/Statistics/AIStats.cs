using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AI_Stats", menuName = "United/AI/Stats")]
public class AIStats : ScriptableObject
{
    public float MaxHealth = 10;
    public float StoppingDistance = 0.5f;
    public float WalkSpeed;
    public float RunSpeed;
    public float ChaseRange = 10f;
    public float Damage;
    public float AttackRange;
    public float AttackHitboxSize;
    public float AttackSpeed;
}
