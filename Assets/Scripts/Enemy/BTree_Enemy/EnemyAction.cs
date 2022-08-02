using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class EnemyAction : Action
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
