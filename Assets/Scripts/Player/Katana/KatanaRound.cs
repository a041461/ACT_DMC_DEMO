using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaRound : MonoBehaviour
{
    public int destoryCount = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
