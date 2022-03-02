using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    Rigidbody rb;
    Animator animator;

    int isWalkingHash;
    int isRunningHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;
    bool isMovementPressed;
    bool isRunPressed;

    [Header("Player Movement Settings")]
    [SerializeField]
    float runMultiplier = 3.0f;
    [SerializeField]
    float movementSpeed = 3.0f;
    float rotationFactorPerFrame = 15f;
    int zero = 0;
    // movement speed
    [Header("Action Settings")]
    [SerializeField]
    bool isActionpressed;
    [SerializeField]
    float cooldownBetweenActions = .5f;
    bool canDoActionAgain = true;

    [Header("Camera Settings")]
    [SerializeField]
    GameObject virtualCamera;
    [SerializeField]
    float cameraRotateSpeed = 10f;
    bool isCameraCSwitchPressed;
    bool isCameraACSwitchPressed;
    bool useNegativeInput;
    bool useIso = true;
    bool isRotatingCamera;
    Quaternion cameraRotationDestination;

    //Inventory
    bool isInventoryPressed;


    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash(Helpers.PlayerIsWalkingAnimation);
        isRunningHash = Animator.StringToHash(Helpers.PlayerIsRunningAnimation);

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Action.started += onAction;
        playerInput.CharacterControls.Action.canceled += onAction;
        playerInput.CharacterControls.Inventory.started += onInventory;
        playerInput.CharacterControls.Inventory.canceled += onInventory;
        playerInput.CharacterControls.SwitchCameraC.started += onCameraCSwitch;
        playerInput.CharacterControls.SwitchCameraC.canceled += onCameraCSwitch;
        playerInput.CharacterControls.SwitchCameraAC.started += onCameraACSwitch;
        playerInput.CharacterControls.SwitchCameraAC.canceled += onCameraACSwitch;

        cameraRotationDestination = virtualCamera.transform.rotation;
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

        // Movement for side camera
        Vector3 toConvert = new Vector3(currentMovementInput.x, 0f, currentMovementInput.y);
        Vector3 convert = useIso ? toConvert.ToIso() : toConvert.ToIso2();
        currentMovement = convert;
        currentRunMovement = convert * runMultiplier;
        isMovementPressed = currentMovementInput.x != zero || currentMovementInput.y != zero;
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onAction(InputAction.CallbackContext context)
    {
        isActionpressed = context.ReadValueAsButton();
    }    
    void onInventory(InputAction.CallbackContext context)
    {
        isInventoryPressed = context.ReadValueAsButton();
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

    private void CheckInventory()
    {
        if (isInventoryPressed)
        {
            Inventory.Instance.Trigger();
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
        else if (Mathf.Approximately(virtualCamera.transform.rotation.eulerAngles.y, 135f))
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
        else if (Mathf.Approximately(virtualCamera.transform.rotation.eulerAngles.y, 315f))
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
    void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
        //Disabled for the moment.
        //HandleJump();

    }

    void Update()
    {
        CheckInventory();
        HandleRotateCamera();
        HandleAnimation();
    }

    private void HandleMovement()
    {
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

        Vector3 moveVector = appliedMovement * movementSpeed;
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
    }

    private void HandleRotateCamera()
    {
        if (isCameraCSwitchPressed && !isRotatingCamera)
        {
            RotateCCamera();
        }
        else if (isCameraACSwitchPressed && !isRotatingCamera)
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

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();

        if (item != null)
        {
            item.ShowIcon();

            //if (isActionpressed)
            //{
            //    item.Trigger();
            //    GameManager.Instance.HideIconInGame();
            //}
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();

        if (item != null)
        {
            if (isActionpressed && canDoActionAgain)
            {
                item.Trigger();
                GameManager.Instance.HideIconInGame();
                canDoActionAgain = false;
                StartCoroutine(SetCooldown(cooldownBetweenActions));
            }
        }
    }

    IEnumerator SetCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canDoActionAgain = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();

        if (item != null)
        {
            GameManager.Instance.HideIconInGame();
        }
    }
}
