using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Paramator
{
    public float health;
    public Animator animator;
    public float idleTime;
    public Vector3[] walkPoint;
    public float walkDistance;
    public float walkSpeed;
    public Transform target;
    public float chaseSpeed;
    public Transform AttackPoint;
    public float AttackArea;
    public LayerMask AttackLayer;
    public bool dead;
}
public class EnemyManager : MonoBehaviour
{
    public Paramator paramator;
    public bool secondForm;
    public GameObject sword;
    public GameObject whisp;

    // Start is called before the first frame update
   public void SetSwordActive()
    {
        sword.SetActive(true);
        whisp.SetActive(true);
    }
}
