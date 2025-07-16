using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject door_closed, door_opened, intText;
    public AudioSource open, close;
    public bool opened = false;
    private bool playerInTrigger = false;

    void Update()
    {
        if (playerInTrigger && !opened && Input.GetKeyDown(KeyCode.E))
        {
            door_closed.SetActive(false);
            door_opened.SetActive(true);
            intText.SetActive(false);
            StartCoroutine(repeat());
            opened = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            playerInTrigger = true;
            if (!opened)
                intText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            playerInTrigger = false;
            intText.SetActive(false);
        }
    }

    IEnumerator repeat()
    {
        yield return new WaitForSeconds(4.0f);
        opened = false;
        door_closed.SetActive(true);
        door_opened.SetActive(false);
    }
}
