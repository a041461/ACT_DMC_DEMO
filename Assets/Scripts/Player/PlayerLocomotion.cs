
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerLocomotion : MonoBehaviour
{
    public Transform cameraObject;
    [HideInInspector]
    public InputHandler inputHandler;
    Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public AnimateHandler animateHandler;

    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Stats")]
    [SerializeField]
    float movementSpeed = 5;
    [SerializeField]
    float rotationSpeed = 10;
    [SerializeField]
    float rollSpeed = 10;
    [SerializeField]
    float jumpSpeed = 25;

    PlayerManager playerManager;

    public CameraHandle cameraHandle;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        //cameraObject = Camera.main.transform;
        myTransform = transform;
        animateHandler = GetComponent<AnimateHandler>();
        animateHandler.Initialize();
        playerManager = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        inputHandler.TickInput(delta);
        HandleMovement(delta);
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (cameraHandle != null)
        {
            cameraHandle.FollowTarget(delta);
            cameraHandle.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    public void HandleRotation(float delta, float rs)
    {
        if (!animateHandler.canRotate)
        {
            return;
        }
        inputHandler.MoveInput(delta);
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize();
        if(!playerManager.isOnGround)
            targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = myTransform.forward;
        }


        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;

    }
    

    public void HandleMovement(float delta)
    {

        if (animateHandler.anim_lock || !playerManager.isOnGround || playerManager.isAttacking)
        {
            HandleRotation(delta, rotationSpeed / 10);
            return;
        }
        //处理收刀动作以及中断收刀处理
        if (!playerManager.isAttacking 
            && inputHandler.moveAmount>0.1f 
            )
        {
            if(animateHandler.IsTag("Combo"))
                animateHandler.anim.SetTrigger("UnEquip");
            animateHandler.PlayTargetAnimation("Locomotion",false);
        }
        
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        //moveDirection.y = 0;

        float speed = movementSpeed;
        if (playerManager.isOnLocked)
            speed *= 0.5f;
        if (Mathf.Abs(inputHandler.moveAmount) <= 0.5f)
            speed *= 0.3f;
        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        animateHandler.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal);

        HandleRotation(delta, rotationSpeed);
    }

    public void HandleRollingAndSpriting(float delta)
    {

        if (animateHandler.anim_lock || !playerManager.isOnGround)
            return;

        inputHandler.rollFlag = false;
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection *= rollSpeed;
        //关闭攻击标识
        playerManager.isAttacking = false;
        if (inputHandler.moveAmount > 0)
        {
            animateHandler.PlayTargetAnimation("Step_F", true);
            if (animateHandler.canRotate)
            {
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;
        }
        else
        {
            animateHandler.PlayTargetAnimation("Step_B", true);
            moveDirection = cameraObject.forward * -1f;
            moveDirection.Normalize();
            moveDirection.y = 0;
            moveDirection *= rollSpeed;
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

        }

    }

    
    public void HandleJump(float delta)
    {
        if (animateHandler.anim_lock || !playerManager.isOnGround)
            return;
        //关闭攻击标识
        playerManager.isAttacking = false;
        inputHandler.jumpFlag = false;
        moveDirection = cameraObject.forward * inputHandler.vertical * 0.2f;
        moveDirection += cameraObject.right * inputHandler.horizontal * 0.2f;
        moveDirection += new Vector3(0, 1, 0);
        moveDirection.Normalize();
        moveDirection *= jumpSpeed;
        animateHandler.PlayTargetAnimation("Jump", true);
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;
        /*
         * moveDirection.x = 0;
        Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
        myTransform.rotation = rollRotation;
        */
    }
    #endregion
    #region Lock
    public void LockEnemy(float delta)
    {
        animateHandler.canRotate = false;
        playerManager.isOnLocked = true;
        animateHandler.PlayTargetAnimation("UnEquipToEquip",false);
        animateHandler.ChangeLockLoayer(1f);

    }

    public void UnLockEnemy(float delta)
    {
        animateHandler.canRotate = true;
        playerManager.isOnLocked = false;
        animateHandler.anim.SetTrigger("UnEquip");
        //animateHandler.PlayTargetAnimation("UnEquipToEquip", false);
        animateHandler.ChangeLockLoayer(0f);
    }

    #endregion

    #region Attack
    public void AttackCombo(float delta)
    {
        //需要清除当前速度
        rigidbody.velocity = Vector3.zero;
        if ((animateHandler.IsName("Combo3_Start") || animateHandler.IsName("Combo3_Loop") )
            && playerManager.combo3Loop>=0)
        {
            animateHandler.anim.SetBool("Loop", true);
        }
        if (playerManager.isAttacking)
        {
            animateHandler.SetTrigger("Attack",true,true);
            playerManager.comboStep++;
        }
        else
        {
            playerManager.isAttacking = true;
            animateHandler.PlayTargetAnimation("Combo01_01", true);
            playerManager.comboStep++;
        }
    }

    public void AttackLaterCombo(float delta)
    {
        //需要清除当前速度
        rigidbody.velocity = Vector3.zero;
        switch (playerManager.comboStep)
        {
            case 2:
            case 3:
                animateHandler.SetTrigger("AttackLater", true, true);
                playerManager.comboStep++;
                break;

            default:
                AttackCombo(delta);
                break;
        }
            
        
    }

    public void AttackHold(float delta)
    {
        rigidbody.velocity = Vector3.zero;
        animateHandler.PlayTargetAnimation("SpecialAttack_Start", true,true);
    }


    #endregion

    private void OnAnimatorMove()
    {
        if (playerManager.isAttacking)
        {
            rigidbody.velocity = new Vector3(animateHandler.anim.velocity.x,rigidbody.velocity.y, animateHandler.anim.velocity.z);
        }
    }
}


