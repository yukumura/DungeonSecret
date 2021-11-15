using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{  
    [SerializeField]
    protected GameObject iconInGame;
    public GameObject IconInGame { get { return iconInGame; } }
    
    [SerializeField]
    protected bool isShowIconInGame;
    public bool IsShowIconInGame { get { return isShowIconInGame; } }

    protected Coroutine hideIconInGameRoutine;
    protected GameObject instantiatedIconInGame;
    protected int secondsToFade = 1;

    [SerializeField]
    protected Inventory inventory;

    void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<Inventory>();
    }

    public void ShowIconInGame()
    {
        if (!isShowIconInGame)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
            instantiatedIconInGame = Instantiate(iconInGame, position, Quaternion.identity);
            isShowIconInGame = true;
            hideIconInGameRoutine = StartCoroutine(HideIconInGame());
        }

        if (hideIconInGameRoutine != null)
        {
            StopCoroutine(hideIconInGameRoutine);
            hideIconInGameRoutine = null;
        }

        CheckStartCoroutineHideIconInGame();
    }

    protected void CheckStartCoroutineHideIconInGame()
    {
        if (hideIconInGameRoutine == null && isShowIconInGame)
        {
            hideIconInGameRoutine = StartCoroutine(HideIconInGame());
        }
    }

    protected IEnumerator HideIconInGame()
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
        //CheckStartCoroutineHideIconInGame();
    }

}
