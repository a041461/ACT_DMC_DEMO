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

    float m_HoldTime;
    float m_LateTime;

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
        if (m_LateTimeStart)
            m_LateTime += delta;
        if (m_LateTime > 100f)
            m_LateTime = 0f;
    }

    public void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
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
            //TODO 添加完美次元斩以及动画判定
            if(m_LateTime >= 0.5f && m_LateTime <= 1f)
                playerLocomotion.AttackLaterCombo(delta);
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
            Debug.Log("m_durationTime:" + m_HoldTime);
            if (m_HoldTime >= 0.8f)
            {
                playerLocomotion.AttackHold(delta);
            }
            m_HoldTime = 0f;
            m_LateTimeStart = true;
        }
    }

    
}


