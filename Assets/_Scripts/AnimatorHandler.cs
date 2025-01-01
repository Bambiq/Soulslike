using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{

    public Animator anim;
    int vertical;
    int horizontal;
    public bool canRotate;

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
    {
        var v = ClampMovementDir(verticalMovement);
        var h = ClampMovementDir(horizontalMovement);


        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void PlayerTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

    private static float ClampMovementDir(float dirMovement)
    {
        float moveDirClamped = 0;
        if (dirMovement > 0 && dirMovement < 0.55f)
        {
            moveDirClamped = 0.5f;
        }
        else if (dirMovement > 0.55f)
        {
            moveDirClamped = 1;
        }
        else if (dirMovement < 0 && dirMovement > -0.55f)
        {
            moveDirClamped = -0.5f;
        }
        else if (dirMovement < -0.55f)
        {
            moveDirClamped = -1;
        }
        else
        {
            moveDirClamped = 0;
        }

        return moveDirClamped;
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }
}
