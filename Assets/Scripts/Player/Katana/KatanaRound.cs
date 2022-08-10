using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaRound : MonoBehaviour
{
    public bool fire;
    public int destoryCount = 3;
    public int flyDirection = 1;
    public float flyTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fire)
        {
            this.transform.Translate(flyDirection * transform.forward.normalized * Time.deltaTime * 20, Space.World);
            flyTime -= Time.deltaTime;
            if (flyTime <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyManager enemyManager = other.GetComponentInParent<EnemyManager>();
            enemyManager.paramator.health--;
            destoryCount--;
            if (destoryCount <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
   
}
