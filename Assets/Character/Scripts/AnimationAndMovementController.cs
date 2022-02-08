using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;
    [SerializeField]
    GameObject virtualCamera;

    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int jumpCountHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;
    bool isMovementPressed;
    bool isRunPressed;

    // constants
    float rotationFactorPerFrame = 15f;
    [SerializeField]
    float runMultiplier = 3.0f;
    int zero = 0;

    // movement speed
    [SerializeField]
    float movementSpeed = 3.0f;

    // gravity variables
    float gravity = -9.8f;
    float groundedGravity = -0.5f;

    // jumping variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 2.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
    bool isJumpAnimating = false;
    int jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    Coroutine currentJumpResetCoroutine = null;

    //action variables    
    bool isActionpressed;
    [SerializeField]
    float forceMagnitude = 1.0f;

    //camera switch
    bool isCameraCSwitchPressed;
    bool isCameraACSwitchPressed;
    bool useNegativeInput;
    bool useIso = true;
    bool isRotatingCamera;
    Quaternion cameraRotationDestination;
    [SerializeField]
    float cameraRotateSpeed = 10f;

    [SerializeField]
    LayerMask layerWithInteract;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;
        playerInput.CharacterControls.Action.started += onAction;
        playerInput.CharacterControls.Action.canceled += onAction;
        playerInput.CharacterControls.SwitchCameraC.started += onCameraCSwitch;
        playerInput.CharacterControls.SwitchCameraC.canceled += onCameraCSwitch;
        playerInput.CharacterControls.SwitchCameraAC.started += onCameraACSwitch;
        playerInput.CharacterControls.SwitchCameraAC.canceled += onCameraACSwitch;

        cameraRotationDestination = virtualCamera.transform.rotation;

        SetupJumpVariables();
    }

    void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }

    void HandleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            if (jumpCount < 3 && currentJumpResetCoroutine != null)
            {
                StopCoroutine(currentJumpResetCoroutine);
            }
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            jumpCount += 1;
            animator.SetInteger(jumpCountHash, jumpCount);
            currentMovement.y = initialJumpVelocities[jumpCount];
            appliedMovement.y = initialJumpVelocities[jumpCount];
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    IEnumerator JumpResetRoutine()
    {
        yield return new WaitForSeconds(.5f);
        jumpCount = 0;
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        // the change in position our character should point to
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = zero;
        positionToLookAt.z = currentMovement.z;

        //the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            //creates a new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        
        currentMovementInput = useNegativeInput ? -context.ReadValue<Vector2>() : context.ReadValue<Vector2>();

        //// Movement for front camera
        //currentMovement.x = currentMovementInput.x;
        //currentMovement.z = currentMovementInput.y;
        //currentRunMovement.x = currentMovementInput.x * runMultiplier;
        //currentRunMovement.z = currentMovementInput.y * runMultiplier;
        //isMovementPressed = currentMovementInput.x != zero || currentMovementInput.y != zero;

        // Movement for side camera
        Vector3 toConvert = new Vector3(currentMovementInput.x, 0, currentMovementInput.y);
        Vector3 convert = useIso ? toConvert.ToIso() : toConvert.ToIso2();
        currentMovement = convert;
        currentRunMovement = convert * runMultiplier;
        isMovementPressed = currentMovementInput.x != zero || currentMovementInput.y != zero;
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onAction(InputAction.CallbackContext context)
    {
        isActionpressed = context.ReadValueAsButton();
    }
    void onCameraCSwitch(InputAction.CallbackContext context)
    {
        isCameraCSwitchPressed = context.ReadValueAsButton();
    }
    void onCameraACSwitch(InputAction.CallbackContext context)
    {
        isCameraACSwitchPressed = context.ReadValueAsButton();
    }

    void HandleAnimation()
    {
        // get parameter value from animator
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }
        if (isMovementPressed && isRunPressed && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void HandleGravity()
    {
        // disable isJumpPressed condition to have always same jump
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;

        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
                currentJumpResetCoroutine = StartCoroutine(JumpResetRoutine());
                if (jumpCount == 3)
                {
                    jumpCount = 0;
                    animator.SetInteger(jumpCountHash, jumpCount);
                }
            }
            currentMovement.y = groundedGravity;
            appliedMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * .5f, -20.0f);
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * .5f, -20.0f);
        }
    }

    private void CheckInteraction()
    {
        RaycastHit hit;
        Vector3 sphere = transform.position + characterController.center;

        if (Physics.SphereCast(sphere, characterController.height / 2, transform.forward, out hit, .5f, layerWithInteract))
        {
            GameObject gameObject = hit.transform.gameObject;
            //CheckIfHitMovableItem(gameObject);
            //CheckIfHitPickupableItem(gameObject);
            //CheckIfHitActionableItem(gameObject);
        }
    }

    private void RotateCCamera()
    {
        if (Mathf.Approximately(virtualCamera.transform.rotation.eulerAngles.y, 45f))
        {
            cameraRotationDestination = Quaternion.Euler(45f, 315f, 0f);

            useNegativeInput = false;
            useIso = true;
        }
        else if(Mathf.Approximately(virtualCamera.transform.rotation.eulerAngles.y, 135f))
        {
            cameraRotationDestination = Quaternion.Euler(45f, 45f, 0f);

            useNegativeInput = true;
            useIso = false;
        }
        else if (Mathf.Approximately(virtualCamera.transform.rotation.eulerAngles.y, 225f))
        {
            cameraRotationDestination = Quaternion.Euler(45f, 135f, 0f);
            useNegativeInput = true;
            useIso = true;
        }
        else if(Mathf.Approximately(virtualCamera.transform.rotation.eulerAngles.y, 315f))
        {
            cameraRotationDestination = Quaternion.Euler(45f, 225f, 0f);

            useNegativeInput = false;
            useIso = false;
        }        
    }

    private void RotateACCamera()
    {
        if (Mathf.Approximately(virtualCamera.transform.rotation.eulerAngles.y, 45f))
        {

            cameraRotationDestination = Quaternion.Euler(45f, 135f, 0f);
            useNegativeInput = true;
            useIso = true;
        }
        else if (Mathf.Approximately(virtualCamera.transform.rotation.eulerAngles.y, 135f))
        {
            cameraRotationDestination = Quaternion.Euler(45f, 225f, 0f);

            useNegativeInput = false;
            useIso = false;
        }
        else if (Mathf.Approximately(virtualCamera.transform.rotation.eulerAngles.y, 225f))
        {
            cameraRotationDestination = Quaternion.Euler(45f, 315f, 0f);

            useNegativeInput = false;
            useIso = true;
        }
        else if (Mathf.Approximately(virtualCamera.transform.rotation.eulerAngles.y, 315f))
        {
            cameraRotationDestination = Quaternion.Euler(45f, 45f, 0f);

            useNegativeInput = true;
            useIso = false;
        }
    }
    void Update()
    {
        HandleRotation();
        HandleAnimation();

        if (isRunPressed)
        {
            appliedMovement.x = currentRunMovement.x;
            appliedMovement.z = currentRunMovement.z;
        }
        else
        {
            appliedMovement.x = currentMovement.x;
            appliedMovement.z = currentMovement.z;
        }

        characterController.Move(appliedMovement * Time.deltaTime * movementSpeed);

        HandleGravity();
        //Disabled for the moment.
        //HandleJump();
        CheckInteraction();
        HandleRotateCamera();
    }

    private void HandleRotateCamera()
    {
        if (isCameraCSwitchPressed && !isRotatingCamera)
        {
            RotateCCamera();
        }
        else if(isCameraACSwitchPressed && !isRotatingCamera)
        {
            RotateACCamera();
        }

        if (virtualCamera.transform.rotation.eulerAngles.normalized != cameraRotationDestination.eulerAngles.normalized)
        {
            isRotatingCamera = true;
            virtualCamera.transform.rotation = virtualCamera.transform.rotation = Quaternion.Slerp(virtualCamera.transform.rotation, cameraRotationDestination, Time.deltaTime * cameraRotateSpeed);
        }
        else
        {
            isRotatingCamera = false;
            virtualCamera.transform.rotation = cameraRotationDestination;
        }
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }
    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
