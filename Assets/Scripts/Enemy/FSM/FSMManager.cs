using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum StateType
{
    Attack,
    Chase,
    Dead,
    Hit,
    Idle,
    Walk,
}

public class FSMManager : MonoBehaviour
{
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    private IState currentState;
    public EnemyManager enemyManager;
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
       
        states.Add(StateType.Idle, new IdleStates(this));       
        states.Add(StateType.Chase, new ChaseStates(this));
        states.Add(StateType.Attack, new AttackStates(this));
        states.Add(StateType.Hit, new HitStates(this));
        states.Add(StateType.Dead, new DeadStates(this));

        


        TransStates(StateType.Idle);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    public void TransStates(StateType type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[type];
        currentState.OnEnter();
    }

    public void KillEnemy()
    {
        Destroy(this.gameObject);
    }
}
