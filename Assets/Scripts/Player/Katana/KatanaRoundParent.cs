using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaRoundParent : MonoBehaviour
{

    Transform[] mytransforms;
    public float splahTime =10;
    public float freezeTime = 1;
    public float flyTime = 2;
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
        delta += Time.deltaTime;
        if (delta >= splahTime)
        {
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
        
       for(int i = 1; i < mytransforms.Length; i++)
        {
            mytransforms[i].Translate(mytransforms[i].forward.normalized*Time.deltaTime*20, Space.World);
        }
        flyTime -= Time.deltaTime;
        if (flyTime <= 0)
        {
            ResetShootHoldAndDestory();
        }
    }
}
