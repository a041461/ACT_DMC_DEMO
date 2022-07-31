using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//´æ·ÅÊµÊ±×´Ì¬
public class PlayerManager : MonoBehaviour
{
    public bool isOnGround = false;
    public bool isOnLocked = false;
    public bool isAttacking = false;
    public int comboStep = 0;
    public int combo3Loop = 0;
    CapsuleCollider capsuleCollider;
    float radius;
    Vector3 pointBottom, pointTop;
    private AnimateHandler animateHandler;
    public static PlayerManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        capsuleCollider = GetComponent<CapsuleCollider>();
        animateHandler = GetComponent<AnimateHandler>();
        radius = capsuleCollider.radius * 0.9f;
    }

    // Update is called once per frame
    void Update()
    {
        OnGround();
    }

    void OnGround()
    {
        pointBottom = transform.position + transform.up * radius - transform.up * 0.1f;
        pointTop = transform.position + transform.up * capsuleCollider.height - transform.up * radius;
        LayerMask ignoreMask = ~(1 << 8);
        Collider[] collider = Physics.OverlapCapsule(pointBottom, pointTop, radius, ignoreMask);
        if (collider.Length != 0)
        {
            if (isOnGround == false)
            {
                animateHandler.IsOnGround(true);
                animateHandler.PlayTargetAnimation("Falling To Landing", true);
            }
                
            isOnGround = true;
        }
        else
        {
            if (isOnGround == true)
                animateHandler.IsOnGround(false);
            isOnGround = false;
        }
    }

}
