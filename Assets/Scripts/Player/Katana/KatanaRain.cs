using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaRain : MonoBehaviour
{
    private int delaytime;
    private bool landed;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        landed = true;
        if (other.CompareTag("Enemy"))
        {            
            EnemyManager enemyManager = other.GetComponentInParent<EnemyManager>();
            enemyManager.paramator.health--;
            enemyManager.AttackSlow();
            this.transform.parent = other.transform;
        }
        
    }
}
