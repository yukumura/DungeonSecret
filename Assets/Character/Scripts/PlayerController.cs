using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    Animator animator;
    [SerializeField]
    GameObject virtualCamera;

    int isWalkingHash;
    int isRunningHash;

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

    //action variables    
    [SerializeField]
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
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Action.started += onAction;
        playerInput.CharacterControls.Action.canceled += onAction;
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

    private void CheckInteraction()
    {
        //if (isActionpressed)
        //{
        //    Debug.Log("Check interaction");

        //    RaycastHit hit;
        //    Vector3 sphere = transform.position + capsuleCollider.center / 2;


        //    if (Physics.SphereCast(sphere, capsuleCollider.height / 2, transform.forward, out hit, .5f, layerWithInteract))
        //    {
        //        GameObject gameObject = hit.transform.gameObject;
        //        Debug.Log(gameObject);
        //        //CheckIfHitMovableItem(gameObject);
        //        //CheckIfHitPickupableItem(gameObject);
        //        //CheckIfHitActionableItem(gameObject);
        //    }
        //}
        
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

    private void CheckIfHitMovableItem(GameObject gameObject)
    {
        Movable movable = gameObject.GetComponent<Movable>();
        if (movable != null)
        {
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

            Vector3 forceDirection = gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);


        }
    }

    private void CheckIfHitPickupableItem(GameObject gameObject)
    {
        Pickup item = gameObject.GetComponent<Pickup>();

        if (item != null && isActionpressed)
        {
            item.Pick();
        }
    }

    //private void CheckIfHitActionableItem(GameObject gameObject)
    //{
    //    Actionable item = gameObject.GetComponent<Actionable>();

    //    if (item != null)
    //    {
    //        if (!item.IsUsed)
    //        {
    //            item.ShowIconInGame();

    //            if (isActionpressed)
    //            {
    //                bool canPlayerDoAction = item.CheckIfPlayerHasRequiredItems();
    //                if (canPlayerDoAction)
    //                {
    //                    Debug.Log("Player can do action");
    //                    item.DoAction();
    //                }
    //                else
    //                {
    //                    Debug.Log("Missing items.");
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("Item already used");
    //        }
    //    }
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleRotation();
        HandleAnimation();
        HandleMovement();
        //CheckInteraction();
        HandleRotateCamera();
        //Disabled for the moment.
        //HandleJump();

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

    private void Update()
    {

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

        if(item != null)
        {
            item.ShowIconInGame();

            if (isActionpressed)
            {
                item.Action();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();

        if (item != null)
        {
            item.ShowIconInGame();

            if (isActionpressed)
            {
                item.Action();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();

        if (item != null)
        {
            item.HideIconInGame();
        }
    }
}
