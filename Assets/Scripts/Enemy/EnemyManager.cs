using System;
using System.Collections;
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
    public int stage;
}
public class EnemyManager : MonoBehaviour
{
    public Paramator paramator;
    public bool secondForm;
    public GameObject sword;
    public GameObject whisp;
    public GameObject[] swordLine;
    public Collider fistAttackCollider;
    public Collider swordAttackCollider;
    public GameObject whispAttackCollider;
    public Collider screamCollider;
    private Collider[] colliderList;
    private Animator anim;


    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }
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
            swordLine[UnityEngine.Random.Range(0, swordLine.Length-1)].SetActive(true);
    }

    public void CameraShake()
    {
        for(float time = 0; time <= 1f; time += Time.deltaTime)
        {
            CameraHandle.Instance.CameraShake();
        }
        
    }
    public void AttackSlow()
    {
        StartCoroutine(IAttackSlow());
    }
    IEnumerator IAttackSlow()
    {       
        if (anim != null)
            anim.speed = 0.1f;
        float pauseTime = 3f;
        yield return new WaitForSecondsRealtime(pauseTime);
        anim.speed = 1;
    }
    #region CollisionPart

    public void OpenAttackCollision()
    {
        if(paramator.stage > 0)
        {
            swordAttackCollider.enabled = true;
        }
        else
        {
            fistAttackCollider.enabled = true;
        }
    }
    public void CloseAttackCollision()
    {
        swordAttackCollider.enabled = false;
        fistAttackCollider.enabled = false;
    }

    public void OpenWhispAttackCollision()
    {
        Collider[] whispAttackColliders =  whispAttackCollider.GetComponentsInChildren<Collider>(true);
        for(int i = 0; i < whispAttackColliders.Length-1; i++)
        {
            whispAttackColliders[i].enabled = true;
        }
    }
    public void CloseWhispAttackCollision()
    {
        Collider[] whispAttackColliders = whispAttackCollider.GetComponentsInChildren<Collider>(true);
        for (int i = 0; i < whispAttackColliders.Length-1; i++)
        {
            whispAttackColliders[i].enabled = false;
        }
    }

    public void OpenScreamCollision()
    {
        screamCollider.enabled = true;

    }

    public void CloserScreamCollision()
    {
        screamCollider.enabled = false;
    }
    #endregion
}
