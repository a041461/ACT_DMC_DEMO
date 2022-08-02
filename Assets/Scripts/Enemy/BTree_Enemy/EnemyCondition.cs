using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCondition : Conditional
{
    protected EnemyManager enemyManager;
    protected Animator animator;
    protected Rigidbody rigidbody;

    public override void OnAwake()
    {
        enemyManager = GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

}
   

