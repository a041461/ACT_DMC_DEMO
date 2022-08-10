using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaRoundParent : MonoBehaviour
{

    Transform[] mytransforms;
    public float splahTime =10;
    public float freezeTime = 1;
    public float flyTime = 2;
    public int flyDirection = 1;
    float delta;
    // Update is called once per frame
    
    void Update()
    {
       
        mytransforms = GetComponentsInChildren<Transform>();
        if(mytransforms.Length == 1)
        {
            ResetShootHoldAndDestory();
            return;
        }
        if (delta == -1f)
        {
            return;
        }
        delta += Time.deltaTime;
        if (delta >= splahTime)
        {
            delta = -1f;
            FreezeKatana();
        }
    }

    private void ResetShootHoldAndDestory()
    {
        PlayerManager.Instance.m_ShootHoldFlag = false;
        Destroy(this.gameObject);
    }

    public void FreezeKatana()
    {
        delta = splahTime;
        Animator anim = GetComponent<Animator>();
        anim.speed = 0;
        freezeTime -= Time.deltaTime;
        if (freezeTime <= 0)
        {
            SplashAllKatana();
        }
    }

    private void SplashAllKatana()
    {
        EnemyManager enemyManager = this.GetComponentInParent<EnemyManager>();
        if (enemyManager != null)
        {
            this.transform.DetachChildren();
            enemyManager.paramator.health -= 12;
            enemyManager.GetComponent<Rigidbody>().velocity = new Vector3(0,10,0);
        }
            
       for(int i = 1; i < mytransforms.Length; i++)
        {
            mytransforms[i].GetComponent<KatanaRound>().fire = true;
            mytransforms[i].GetComponent<KatanaRound>().flyDirection = flyDirection;
        }
        flyTime -= Time.deltaTime;
        if (flyTime <= 0)
        {
            ResetShootHoldAndDestory();
        }
    }
}
