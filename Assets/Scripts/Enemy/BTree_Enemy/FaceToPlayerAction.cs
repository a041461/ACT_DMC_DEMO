using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToPlayerAction : EnemyAction
{
    public float rotationSpeed;
    public float timeOut;
    private float delta;
    public override void OnStart()
    {
        var direction = enemyManager.paramator.target.position - transform.position;
        var angle = Vector3.Angle(transform.forward, direction);
        if(angle>5f)
            animator.Play("strafeLeft");
        animator.ResetTrigger("ToIdle");
        animator.ResetTrigger("ToFly");
        delta = 0;

    }
    public override TaskStatus OnUpdate()
    {
        var direction = enemyManager.paramator.target.position - transform.position;
        var angle = Vector3.Angle(transform.forward, direction);
        var cross = Vector3.Cross(transform.forward, direction);
        var turn = cross.y > -0 ? 1f : -1f;
        transform.Rotate(transform.up, angle * Time.deltaTime * turn* rotationSpeed, Space.World);
        delta += Time.deltaTime;
        if (delta >= timeOut)
        {
            transform.Rotate(transform.up, angle * turn , Space.World);
            animator.SetTrigger("ToFly");
            return TaskStatus.Success;
        }
        if (angle < 5f)
        {
            animator.SetTrigger("ToIdle");
            return TaskStatus.Success;
        }
            
        else
            return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        delta = 0;
    }
}
