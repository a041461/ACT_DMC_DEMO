using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateHandler : MonoBehaviour
{
    public Animator anim;
    public InputHandler InputHandler;  
    int vertical;
    int horizontal;
    public bool canRotate;
    public bool anim_lock;

    int lockLayerIndex;

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        InputHandler = GetComponentInParent<InputHandler>();    
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
        lockLayerIndex = anim.GetLayerIndex("LockLayer");
    }

    private void Update()
    {
        anim_lock = anim.GetBool("anim_lock");
    }

    public void UpdateAnimatorValues(float verticalMovement, float HorizontalMovement)
    {
        /*
        #region Vertical
        float v = 0;

        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
        #endregion

        #region Horizontal
        float h = 0;
        if (HorizontalMovement > 0 && HorizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (HorizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if (HorizontalMovement < 0 && HorizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (HorizontalMovement < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }
        #endregion
        */
        anim.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, HorizontalMovement, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnim, bool animLock,bool rootMotion = false)
    {
        anim.applyRootMotion = rootMotion;
        anim_lock = animLock;
        anim.SetBool("anim_lock", anim_lock);
        //anim.CrossFade(targetAnim,0.2f);
        anim.Play(targetAnim);
    } 

    public void SetTrigger(string trigger,bool animLock,bool rootMotion = false)
    {
        anim.applyRootMotion = rootMotion;
        anim_lock = animLock;
        anim.SetBool("anim_lock", anim_lock);
        anim.SetTrigger(trigger);
    }
       
    public void UnlockAnimLock()
    {
        anim_lock = false;
        anim.SetBool("anim_lock", false);
    }

    public void IsOnGround(bool isOnGround)
    {
        anim.SetBool("isOnGround", isOnGround);
    }

    public void ChangeLockLoayer(float weight)
    {
        anim.SetLayerWeight(lockLayerIndex, weight);
    }

    public bool IsTag(string tagname)
    {
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(1);
        return stateinfo.IsTag(tagname);
    }
    
    public bool IsName(string name)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(1);
        return stateInfo.IsName(name);
    }
}

