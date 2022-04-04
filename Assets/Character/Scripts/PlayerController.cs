using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    Rigidbody rb;
    Animator animator;
    RuntimeAnimatorController ac;


    int isWalkingHash;
    int isRunningHash;
    int tryToOpenTheDoorHash;
    int pickingItemsFromGroundHash;
    int pickingItemsFromMiddleHash;
    int openingChestHash;
    int lookAroundHash;
    int getUpHash;

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
    [SerializeField]
    AudioClip[] steps;
    // movement speed
    [Header("Action Settings")]
    [SerializeField]
    bool isActionpressed;
    [SerializeField]
    bool canDoAction = false;

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
    //Menu
    bool isMenuPressed;


    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        ac = animator.runtimeAnimatorController;

        isWalkingHash = Animator.StringToHash(Helpers.PlayerIsWalkingAnimation);
        isRunningHash = Animator.StringToHash(Helpers.PlayerIsRunningAnimation);
        tryToOpenTheDoorHash = Animator.StringToHash(Helpers.TryToOpenTheDoorAnimation);
        pickingItemsFromGroundHash = Animator.StringToHash(Helpers.PickingItemsFromGroundAnimation);
        pickingItemsFromMiddleHash = Animator.StringToHash(Helpers.PickingItemsFromMiddleAnimation);
        openingChestHash = Animator.StringToHash(Helpers.OpeningChestAnimation);
        lookAroundHash = Animator.StringToHash(Helpers.LookAroundAnimation);
        getUpHash = Animator.StringToHash(Helpers.GetUpAnimation);
        

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
        playerInput.CharacterControls.Menu.started += onMenu;
        playerInput.CharacterControls.Menu.canceled += onMenu;

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
    void onMenu(InputAction.CallbackContext context)
    {
        isMenuPressed = context.ReadValueAsButton();
    }
    void onCameraCSwitch(InputAction.CallbackContext context)
    {
        isCameraCSwitchPressed = context.ReadValueAsButton();
    }
    void onCameraACSwitch(InputAction.CallbackContext context)
    {
        isCameraACSwitchPressed = context.ReadValueAsButton();
    }

    void HandleMovementAnimation()
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

    public void PlayItemAnimation(Helpers.ItemType itemType)
    {
        switch (itemType)
        {
            case Helpers.ItemType.PickupFromGround:
                animator.Play(pickingItemsFromGroundHash);
                StartCoroutine(SetCooldown(ac.animationClips.Where(x => x.name == Helpers.PickingItemsFromGroundAnimation).FirstOrDefault().length / 1.5f));
                break;
            case Helpers.ItemType.PickupFromMiddle:
                animator.Play(pickingItemsFromMiddleHash);
                StartCoroutine(SetCooldown(ac.animationClips.Where(x => x.name == Helpers.PickingItemsFromMiddleAnimation).FirstOrDefault().length));
                break;
            case Helpers.ItemType.ActionableDoor:
                animator.Play(tryToOpenTheDoorHash);
                StartCoroutine(SetCooldown(ac.animationClips.Where(x => x.name == Helpers.TryToOpenTheDoorAnimation).FirstOrDefault().length));
                break;
            case Helpers.ItemType.OpeningChest:
                animator.Play(openingChestHash);
                StartCoroutine(SetCooldown(ac.animationClips.Where(x => x.name == Helpers.OpeningChestAnimation).FirstOrDefault().length / 2));
                break;
            case Helpers.ItemType.Generic:
                //animator.Play(standardIdleHash);
                //StartCoroutine(SetCooldown(ac.animationClips.Where(x => x.name == Helpers.StandardIdleAnimation).FirstOrDefault().length));
                StartCoroutine(SetCooldown(0.1f));
                break; 
            case Helpers.ItemType.CloseChest:
                animator.Play(lookAroundHash);
                StartCoroutine(SetCooldown(ac.animationClips.Where(x => x.name == Helpers.LookAroundAnimation).FirstOrDefault().length));
                break;
            default:
                break;
        }
    }

    private void CheckInventory()
    {
        if (isInventoryPressed)
        {
            Inventory.Instance.Trigger();
        }
    }
    private void CheckMenu()
    {
        if (isMenuPressed)
        {
            GameManager.Instance.TriggerCommands();
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
        if (canDoAction && !GameManager.Instance.finishGame)
        {
            HandleRotation();
            HandleMovement();
            //Disabled for the moment.
            //HandleJump();
        }
    }

    void Update()
    {
        if (canDoAction && !GameManager.Instance.finishGame)
        {
            CheckInventory();
            CheckMenu();
            HandleRotateCamera();
            HandleMovementAnimation();
        }

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
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();

        if (item != null)
        {
            if (isActionpressed && canDoAction && !item.IsUsed)
            {
                canDoAction = false;
                GameManager.Instance.HideIconInGame();
                item.Trigger();
            }
        }
    }

    IEnumerator SetCooldown(float time)
    {
        StopMovement();
        yield return new WaitForSeconds(time);
        canDoAction = true;
    }

    private void StopMovement()
    {
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isRunningHash, false);
    }

    private void OnTriggerExit(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();

        if (item != null)
        {
            GameManager.Instance.HideIconInGame();
        }
    }

    public void StartUsePlayer()
    {
        canDoAction = true;        
    }

    public void GetUp()
    {
        animator.SetTrigger(getUpHash);
    }

    public void PlayStep()
    {
        int step = UnityEngine.Random.Range(0, steps.Count() - 1);
        SFXManager.Instance.Audio.PlayOneShot(steps[step], .5f);
    }
}
