using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;


public class WalkToPlayerAction : EnemyAction
{
    public float distance;
    public float walkSpeed;
    public float rotationSpeed;
    private bool isNear;

    public override void OnStart()
    {
        animator.ResetTrigger("ToIdle");
    }

    private void StartWalk()
    {
        var direction = enemyManager.paramator.target.position - transform.position;
        transform.Translate(direction.normalized * Time.deltaTime* walkSpeed, Space.World);
        var angle = Vector3.Angle(transform.forward, direction);
        var cross = Vector3.Cross(transform.forward, direction);
        var turn = cross.y > -0 ? 1f : -1f;
        transform.Rotate(transform.up, angle * Time.deltaTime * turn* rotationSpeed, Space.World);
        IsNearBy();
    }

    public override TaskStatus OnUpdate()
    {
        StartWalk();
        return isNear ? TaskStatus.Success : TaskStatus.Running;
    }

    public void IsNearBy()
    {
        Debug.Log("distance:" + (enemyManager.paramator.target.position - this.transform.position).sqrMagnitude);
        if ((enemyManager.paramator.target.position - this.transform.position).sqrMagnitude <= distance)
        {
            isNear = true;
            
        }
           
    }

    public override void OnEnd()
    {
        isNear = false;
    }
}
