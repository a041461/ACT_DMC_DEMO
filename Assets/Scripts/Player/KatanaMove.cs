using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaMove : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed;
    public float delaytime;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = PlayerManager.Instance.currentTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        delaytime -= Time.deltaTime;
        if (delaytime > 0)
        {
            return;
        }
        var direction = targetPosition - transform.position;
        transform.Translate(direction.normalized * Time.deltaTime * speed,Space.World);
        var angle = Vector3.Angle(transform.forward, direction);
        var cross = Vector3.Cross(transform.forward, direction);
        var turn = cross.y > -0 ? 1f : -1f;
        transform.Rotate(transform.up, angle * Time.deltaTime * turn * speed, Space.World);
    }
}
