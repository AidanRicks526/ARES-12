using UnityEngine;

public class Box_Open : MonoBehaviour
{
    public GameObject objectToEnable;
    public float activeTime = 4f;

    private bool playerInRange = false;
    private bool isActive = false; 

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isActive)
        {
            OpenBox();
        }
    }

    void OpenBox()
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
            isActive = true;
            StartCoroutine(DisableAfterTime());
        }

    }

    System.Collections.IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(activeTime);

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false);
        }

        isActive = false; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}