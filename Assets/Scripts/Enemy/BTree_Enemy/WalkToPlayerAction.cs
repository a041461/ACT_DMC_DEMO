using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;


public class WalkToPlayerAction : EnemyAction
{
    public float distance;
    public float walkSpeed;
    public float rotationSpeed;
    public float chaseTimeOut;
    private bool isNear;
    private float delta;

    public override void OnStart()
    {
        animator.ResetTrigger("ToIdle");
        animator.ResetTrigger("Scream");
        delta = 0;
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
        delta += Time.deltaTime;
        if (delta>=chaseTimeOut)
        {
            animator.SetTrigger("Scream");
            walkSpeed = 7;
            return TaskStatus.Failure;
        }
        if (isNear)
        {
            animator.SetTrigger("ToIdle");
            walkSpeed = 5;
            return TaskStatus.Success;
        }           
        return TaskStatus.Running;
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
