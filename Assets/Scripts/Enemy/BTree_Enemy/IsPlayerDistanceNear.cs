using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class IsPlayerDistanceNear : EnemyCondition
{
    public float distance;
    public override TaskStatus OnUpdate()
    {
        Debug.Log("distance:"+ (enemyManager.paramator.target.position - this.transform.position).sqrMagnitude);
        if ((enemyManager.paramator.target.position - this.transform.position).sqrMagnitude<=distance)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
