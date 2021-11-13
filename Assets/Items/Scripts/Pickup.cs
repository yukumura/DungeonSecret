using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    Sprite iconInInventory;
    [SerializeField]
    GameObject iconPickup;
    [SerializeField]
    bool isShowIconPickup;
    public bool IsShowIconPickup { get { return isShowIconPickup; } }
    public Sprite IconInventory { get { return iconInInventory; } }
    public GameObject IconPickup { get { return iconPickup; } }

    Coroutine hideIconPickupRoutine;
    GameObject instantiatedIconPickup;
    int secondsToFade = 1;

    [SerializeField]
    private Inventory inventory;


    void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<Inventory>();
    }

    public void ShowPickup()
    {
        if (!isShowIconPickup)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
            instantiatedIconPickup = Instantiate(iconPickup, position, Quaternion.identity);
            isShowIconPickup = true;
            hideIconPickupRoutine = StartCoroutine(HideIconPickup());
        }

        if (hideIconPickupRoutine != null)
        {
            StopCoroutine(hideIconPickupRoutine);
            hideIconPickupRoutine = null;
        }
    }

    private void CheckStartCoroutineHideIconPickup()
    {
        if (hideIconPickupRoutine == null && isShowIconPickup)
        {
            hideIconPickupRoutine = StartCoroutine(HideIconPickup());
        }
    }

    IEnumerator HideIconPickup()
    {
        yield return new WaitForSeconds(secondsToFade);
        if (instantiatedIconPickup != null)
        {
            Destroy(instantiatedIconPickup);
            isShowIconPickup = false;
        }
    }

    public void Pick()
    {
        Slot slot = inventory.GetFirstAvailableSlot();
        if (slot != null)
        {
            slot.SetItem(iconInInventory);
            ClearReference();
        }
    }

    private void ClearReference()
    {
        if (hideIconPickupRoutine != null)
        {
            StopCoroutine(hideIconPickupRoutine);
        }

        Destroy(instantiatedIconPickup);
        Destroy(gameObject);
    }

    void Update()
    {
        CheckStartCoroutineHideIconPickup();
    }

    //to do: check if it's better use trigger enter instead character controller OnControllerColliderHit
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}
