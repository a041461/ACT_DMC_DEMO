using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState
{
    void OnEnter();

    void OnUpdate();

    void OnExit();
}


public class IdleStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private float timer;
    public IdleStates(FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.enemyManager.paramator;


    }
    public void OnEnter()
    {
        paramator.animator.Play("Idle");
        timer = paramator.idleTime;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            fsm.TransStates(StateType.Chase);
        }
    }
}

public class ChaseStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private float timer;

    public ChaseStates(FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.enemyManager.paramator;


    }
    public void OnEnter()
    {
        paramator.animator.Play("Run");
    }

    public void OnExit()
    {


    }

    public void OnUpdate()
    {
        //×·»÷Á÷³Ì
        var direction = paramator.target.position - fsm.transform.position;
        fsm.transform.Translate(direction.normalized * Time.deltaTime * 2f, Space.World);
        var angle = Vector3.Angle(fsm.transform.forward, direction);
        var cross = Vector3.Cross(fsm.transform.forward, direction);
        var turn = cross.y > 0 ? 1f : -1f;
        fsm.transform.Rotate(fsm.transform.up, angle  * turn , Space.World);

        if ((paramator.target.position - fsm.transform.position).sqrMagnitude<=paramator.chaseDistance)
        {
            fsm.TransStates(StateType.Attack);
        }
    }
}
public class DeadStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private float timer;
    private AnimatorStateInfo info;

    public DeadStates(FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.enemyManager.paramator;


    }
    public void OnEnter()
    {
        paramator.dead = true;
        paramator.animator.Play("Dead");
        BattleManager.Instance.DetecedCapsules();

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        info = paramator.animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 1f)
        {

            fsm.KillEnemy();
        }


    }
}

public class AttackStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private float timer;
    private AnimatorStateInfo info;
    public AttackStates(FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.enemyManager.paramator;


    }
    public void OnEnter()
    {
        int attckType = Random.Range(0, 2);
        if (attckType == 0)
            paramator.animator.Play("Attack");
        else
            paramator.animator.Play("Attack_Round");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        info = paramator.animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 0.95f)
        {
            
            paramator.idleTime = Random.Range(1f, 3f);
            fsm.TransStates(StateType.Idle);
        }

    }
}
public class HitStates : IState
{
    private FSMManager fsm;
    private Paramator paramator;
    private float timer;
    private AnimatorStateInfo info;
    public HitStates(FSMManager fsm)
    {
        this.fsm = fsm;
        this.paramator = fsm.enemyManager.paramator;


    }
    public void OnEnter()
    {
        paramator.animator.Play("Hit");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        info = paramator.animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 0.9f)
        {
            fsm.TransStates(StateType.Idle);
        }

    }
}

