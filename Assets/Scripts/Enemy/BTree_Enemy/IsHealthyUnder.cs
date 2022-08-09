using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHealthyUnder : EnemyCondition
{
    public int UnderHealth;

    public override TaskStatus OnUpdate()
    {
        if (enemyManager.paramator.health<=UnderHealth)
        {
            enemyManager.paramator.stage = 1;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

}
