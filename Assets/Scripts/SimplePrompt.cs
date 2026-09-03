using UnityEngine;
using TMPro;

public class SimplePrompt : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public float rayDistance = 3f;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerCamera == null) return;

        // Lancia il raggio
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        bool lookingAtInteractable = false;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.GetComponent<InteractableObject>() != null)
            {
                lookingAtInteractable = true;
            }
        }

        // Mostra o nascondi il prompt
        if (promptText != null)
        {
            promptText.gameObject.SetActive(lookingAtInteractable);
        }

        // Debug: disegna il raggio
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, lookingAtInteractable ? Color.green : Color.red);
    }
}