using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    public float velocity = 5;
    Transform cameraObject;
    InputHandler inputHandler;
    Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public AnimatorHandler animatorHandler;

    public new Rigidbody rigidbody;

    [Header("Stats")]
    [SerializeField]
    float movementSpeed = 5;
    [SerializeField]
    float rorationSpedd = 5;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        inputHandler.TickInput(delta);
        HandleMovement(delta);
        HandleRollingAndSprint(delta);
    }
    #region Movement

    Vector3 normalVector;
    Vector3 targetPosition;

    private void HandleRotation(float delta)
    {
        Vector3 targerDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targerDir = cameraObject.forward * inputHandler.vertical;
        targerDir += cameraObject.right * inputHandler.horizontal;

        targerDir.Normalize();
        targerDir.y = 0;

        if (targerDir == Vector3.zero)
            targerDir = myTransform.forward;

        float rs = rorationSpedd;

        Quaternion tr = Quaternion.LookRotation(targerDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;
        #endregion
    }
    
    public void HandleMovement(float delta)
    {
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;
        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);

        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRollingAndSprint(float delta)
    {
        if (animatorHandler.anim.GetBool("isInteracting"))
            return;

        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0)
            {
                animatorHandler.PlayerTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else
            {
                animatorHandler.PlayerTargetAnimation("Backstep", true);
            }
        }
    }
}
