using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaMove : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed;
    public float delaytime;
    public float destorytime;
    private bool landed;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = PlayerManager.Instance.currentTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        delaytime -= Time.deltaTime;
        var direction = targetPosition - transform.position;    
        var angle = Vector3.Angle(transform.forward, direction);
        var cross = Vector3.Cross(transform.forward, direction);
        var turn = cross.y > -0 ? 1f : -1f;
        transform.Rotate(transform.up, angle * Time.deltaTime * turn * speed, Space.World);
        if (delaytime > 0)
        {
            return;
        }
        if (landed)
        {
            destorytime -= Time.deltaTime;
            if (destorytime <= 0)
                Destroy(this.gameObject);
            return;
        }
        transform.Translate(direction.normalized * Time.deltaTime * speed, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyManager enemyManager = other.GetComponentInParent<EnemyManager>();
            enemyManager.paramator.health--;
            landed = true;
            this.transform.parent = other.transform;
        }
    }
}
