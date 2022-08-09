using System;
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
    public GameObject[] swordLine;
    public Collider attackCollider;
    private Collider[] colliderList;

    public void Update()
    {
        colliderList = Physics.OverlapSphere(transform.position, 10f, 1 << LayerMask.NameToLayer("Player"));
        if (colliderList.Length > 0)
            paramator.target = colliderList[0].transform;
    }

    // Start is called before the first frame update
    public void SetSwordActive()
    {
        if (sword != null && whisp != null)
        {
            sword.SetActive(true);
            whisp.SetActive(true);
        }
    }

    public void OnParticleCollision(GameObject other)
    {
        paramator.health--;
        //PlayerManager.Instance.AttackPause(1f);
    }

    public void OpenSwordLine()
    {
        if(swordLine!=null)
            swordLine[UnityEngine.Random.Range(0, swordLine.Length)].SetActive(true);
    }

    public void OpenAttackCollision(bool enable = true)
    {
        attackCollider.enabled = true;
    }

}
