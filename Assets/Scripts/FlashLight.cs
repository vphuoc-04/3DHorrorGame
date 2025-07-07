using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public GameObject flashlight_ground, inticon, flashlight_player;

    private bool playerInTrigger = false;

    void Start()
    {
        flashlight_player.SetActive(false); 
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            flashlight_ground.SetActive(false);     
            inticon.SetActive(false);               
            flashlight_player.SetActive(true);      
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            playerInTrigger = true;
            inticon.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            playerInTrigger = false;
            inticon.SetActive(false);
        }
    }
}
