using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaRain : MonoBehaviour
{
    private int delaytime;
    private bool landed;
    public float destoryTime = 1.5f;

    private void Start()
    {
        delaytime = Random.Range(50,100);
        
    }
    // Update is called once per frame
    void Update()
    {
        delaytime--;
        if(delaytime <=0&&!landed)
            transform.Translate(this.transform.forward*Time.deltaTime*20, Space.World);
        if (landed)
        {
            destoryTime -= Time.deltaTime;
            if (destoryTime <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        landed = true;
        if (other.CompareTag("Enemy"))
        {            
            EnemyManager enemyManager = other.GetComponentInParent<EnemyManager>();
            enemyManager.OnAttacked(2f);
            enemyManager.AttackSlow();
            this.transform.parent = other.transform;
        }
        
    }
}
