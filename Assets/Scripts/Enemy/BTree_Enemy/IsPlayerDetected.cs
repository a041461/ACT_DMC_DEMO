using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class IsPlayerDetected : EnemyCondition
{
    // Start is called before the first frame update
    public override TaskStatus OnUpdate()
    {
        if (enemyManager.paramator.target != null)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
