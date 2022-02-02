using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField]
    protected GameObject iconInGame;
    
    [SerializeField]
    protected bool isShowIconInGame;

    protected Coroutine hideIconInGameRoutine;
    protected GameObject instantiatedIconInGame;
    protected int secondsToFade = 1;

    [SerializeField]
    protected Inventory inventory;

    void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<Inventory>();
    }


    public virtual void Action()
    {
        Debug.Log("item");
    }

    public void ShowIconInGame()
    {
        if (!isShowIconInGame)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
            instantiatedIconInGame = Instantiate(iconInGame, position, Quaternion.identity);
            isShowIconInGame = true;
        }
    }

    public void HideIconInGame()
    {
        hideIconInGameRoutine = StartCoroutine(HideIconInGameRoutine());
    }

    protected IEnumerator HideIconInGameRoutine()
    {
        yield return new WaitForSeconds(secondsToFade);
        if (instantiatedIconInGame != null)
        {
            Destroy(instantiatedIconInGame);
            isShowIconInGame = false;
        }
    }

    protected void ClearReference()
    {
        if (hideIconInGameRoutine != null)
        {
            StopCoroutine(hideIconInGameRoutine);
        }

        Destroy(instantiatedIconInGame);
        Destroy(gameObject);
    }

    void Update()
    {
    }
}
