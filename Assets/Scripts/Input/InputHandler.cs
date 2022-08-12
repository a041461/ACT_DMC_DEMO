using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;


public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool b_Input;
    public bool rollFlag;
    public bool jumpFlag;
    public bool backForwardFlag;


    float m_HoldTime;
    float m_LateTime;
    public float m_BFEnterTime;

    float m_ShootHoldTime;


    bool m_LateTimeStart;

    PlayerLocomotion playerLocomotion;

    PlayerController inputActions;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }



    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerController();
            inputActions.PlayerMovement.Move.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }


    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
        JumpInput(delta);
        LockInput(delta);
        AttackInput(delta);
        ShootInput(delta);
        SetBackForwardFlag();
        if (m_LateTimeStart)
            m_LateTime += delta;
        if (m_LateTime > 100f)
            m_LateTime = 0f;
        if (m_HoldTime >= 0.7f)
        {
            playerLocomotion.SetHandLighting();
        }
        if (m_HoldTime >= 1f)
            playerLocomotion.SetSwordLighting(true);

    }

    public void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void SetBackForwardFlag()
    {

        if (m_BFEnterTime != 0)
            m_BFEnterTime += Time.deltaTime;
        if (m_BFEnterTime >= 1f)
            m_BFEnterTime = 0;
        if (m_BFEnterTime == 0)
            backForwardFlag = false;

        if (PlayerManager.Instance.isOnLocked)
        {
            if (vertical <= -0.8f)
            {
                m_BFEnterTime += Time.deltaTime;
            }
            else if (vertical >= 0.8f)
            {
                if (m_BFEnterTime <= 0.5f&&m_BFEnterTime>0)
                {
                    backForwardFlag = true;
                }               
                else
                {
                    m_BFEnterTime = 0;
                }

            }
        }
    }

    public void HandleRollInput(float delta)
    {
        //MoveInput(delta);
        if (inputActions.PlayerAction.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed)
        {
            playerLocomotion.HandleRollingAndSpriting(delta);
        }

    }

    public void JumpInput(float delta)
    {

        if (inputActions.PlayerAction.Jump.phase == UnityEngine.InputSystem.InputActionPhase.Performed)
        {
            playerLocomotion.HandleJump(delta);
        }
    }

    public void LockInput(float delta)
    {
        if (inputActions.PlayerAction.Lock.WasPressedThisFrame())
        {
            playerLocomotion.LockEnemy(delta);
        }
        else if (inputActions.PlayerAction.Lock.WasReleasedThisFrame())
        {
            playerLocomotion.UnLockEnemy(delta);
        }
    }

    public void AttackInput(float delta)
    {
        //按下以及长按逻辑
        if (inputActions.PlayerAction.Attack.WasPressedThisFrame())
        {
            //CameraHandle.Instance.CameraShake();

            if (m_LateTime >= 0.5f && m_LateTime <= 1f)
                playerLocomotion.AttackLaterCombo(delta);
            else if (PlayerManager.Instance.isOnLocked)
                playerLocomotion.AttackLock(delta);
            else
                playerLocomotion.AttackCombo(delta);
            m_LateTimeStart = false;
            m_LateTime = 0f;
        }
        if (inputActions.PlayerAction.Attack.IsPressed())
        {
            m_HoldTime += delta;
        }
        if (inputActions.PlayerAction.Attack.WasReleasedThisFrame())
        {
            //TODO 添加完美次元斩以及动画判定
            Debug.Log("m_durationTime:" + m_HoldTime);
            if (m_HoldTime >= 0.8f)
            {
                playerLocomotion.SetSwordLighting(false);
                playerLocomotion.AttackHold(delta);
            }
            m_HoldTime = 0f;
            m_LateTimeStart = true;
        }
    }

    public void ShootInput(float delta)
    {

        if (inputActions.PlayerAction.Fire.WasPressedThisFrame())
        {
            if (backForwardFlag)
            {
                playerLocomotion.LongHoldShoot_BF();
            }
            else
            {
                playerLocomotion.Shoot(delta);
            }
            
        }

        if (inputActions.PlayerAction.Fire.IsPressed())
        {
            m_ShootHoldTime += delta;
            if (m_ShootHoldTime >= 0.8f && !PlayerManager.Instance.m_ShootHoldFlag)
            {
                PlayerManager.Instance.m_ShootHoldFlag = true;
                playerLocomotion.ShootHold(delta);
            }
        }
        if (inputActions.PlayerAction.Fire.WasReleasedThisFrame())
        {

            Debug.Log("m_durationTime:" + m_ShootHoldTime);
            if (m_ShootHoldTime >= 0.8f)
            {
                PlayerManager.Instance.m_ShootHoldFlag = false;
                /*
                playerLocomotion.SetSwordLighting(false);
                playerLocomotion.AttackHold(delta);
                */
            }
            m_ShootHoldTime = 0f;
        }
    }

}


