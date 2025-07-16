using System.Collections;
using UnityEngine;

public class Laptop : MonoBehaviour
{
    public GameObject intText;
    public Transform cameraTargetPosition;
    public float cameraMoveSpeed = 2f;

    private bool playerInTrigger = false;
    private bool isUsingLaptop = false;
    private Camera mainCam;

    private FPSController playerController;

    void Start()
    {
        mainCam = Camera.main;
        if (intText != null) intText.SetActive(false);

        playerController = GameObject.FindWithTag("Player").GetComponent<FPSController>();
    }

    void Update()
    {
        if (playerInTrigger && !isUsingLaptop && Input.GetKeyDown(KeyCode.E))
        {
            isUsingLaptop = true;
            if (intText != null) intText.SetActive(false);

            if (playerController != null)
            {
                playerController.canMove = false; 
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if (isUsingLaptop)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, cameraTargetPosition.position, Time.deltaTime * cameraMoveSpeed);
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, cameraTargetPosition.rotation, Time.deltaTime * cameraMoveSpeed);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(ExitLaptop());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Trigger with: " + other.name);
            playerInTrigger = true;
            if (!isUsingLaptop && intText != null)
            {
                intText.SetActive(true);
                Debug.Log("intText được bật");
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            playerInTrigger = false;
            if (intText != null)
                intText.SetActive(false);
        }
    }

    IEnumerator ExitLaptop()
    {
        yield return new WaitForSeconds(0.2f);

        isUsingLaptop = false;

        if (playerController != null)
        {
            playerController.canMove = true; 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
