using System.Collections;
using UnityEngine;

public class Laptop : MonoBehaviour
{
    public GameObject intText;
    public GameObject escText;
    public GameObject laptopScreen;
    public Transform cameraTargetPosition;
    public float cameraMoveSpeed = 2f;
    public float fadeDuration = 1f;

    private bool playerInTrigger = false;
    private bool isUsingLaptop = false;
    private Camera mainCam;
    private FPSController playerController;
    private CanvasGroup screenCanvasGroup;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    void Start()
    {
        mainCam = Camera.main;

        if (intText != null) intText.SetActive(false);
        if (escText != null) escText.SetActive(false);

        playerController = GameObject.FindWithTag("Player").GetComponent<FPSController>();

        if (laptopScreen != null)
        {
            screenCanvasGroup = laptopScreen.GetComponent<CanvasGroup>();
            if (screenCanvasGroup == null)
            {
                screenCanvasGroup = laptopScreen.AddComponent<CanvasGroup>();
            }
            screenCanvasGroup.alpha = 0;
            laptopScreen.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInTrigger && !isUsingLaptop && Input.GetKeyDown(KeyCode.E))
        {
            isUsingLaptop = true;
            if (intText != null) intText.SetActive(false);

            originalCameraPosition = mainCam.transform.position;
            originalCameraRotation = mainCam.transform.rotation;

            StartCoroutine(StartLaptopSequence());
        }

        if (isUsingLaptop && Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(ExitLaptop());
        }

        if (escText != null)
        {
            escText.SetActive(isUsingLaptop);
        }
    }

    IEnumerator StartLaptopSequence()
    {
        if (playerController != null)
        {
            playerController.canMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        while (Vector3.Distance(mainCam.transform.position, cameraTargetPosition.position) > 0.05f)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, cameraTargetPosition.position, Time.deltaTime * cameraMoveSpeed);
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, cameraTargetPosition.rotation, Time.deltaTime * cameraMoveSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(FadeInScreen());
    }

    IEnumerator FadeInScreen()
    {
        laptopScreen.SetActive(true);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            screenCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
    }

    IEnumerator FadeOutScreen()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            screenCanvasGroup.alpha = 1 - Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
        laptopScreen.SetActive(false);
    }

    IEnumerator ExitLaptop()
    {
        yield return StartCoroutine(FadeOutScreen());

        while (Vector3.Distance(mainCam.transform.position, originalCameraPosition) > 0.05f)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, originalCameraPosition, Time.deltaTime * cameraMoveSpeed);
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, originalCameraRotation, Time.deltaTime * cameraMoveSpeed);
            yield return null;
        }

        isUsingLaptop = false;

        if (playerController != null)
        {
            playerController.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            playerInTrigger = true;
            if (!isUsingLaptop && intText != null)
            {
                intText.SetActive(true);
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
}
