using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raggio di interazione")]
    public float interactionRange = 2.5f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI")]
    public GameObject interactionPrompt;

    private Camera playerCamera;
    private InteractableObject currentTarget;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (playerCamera == null)
            Debug.LogError("PlayerInteraction: Nessuna camera trovata sul Player!");
    }

    void Update()
    {
        if (playerCamera == null) return;

        if (IsAnyInputActive())
            return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                if (currentTarget != interactable)
                {
                    if (currentTarget != null)
                        currentTarget.Unhighlight();

                    currentTarget = interactable;
                    currentTarget.Highlight();

                    if (interactionPrompt != null)
                        interactionPrompt.SetActive(true);
                }

                if (Input.GetKeyDown(interactKey))
                {
                    currentTarget.Interact();

                    if (currentTarget == null || !currentTarget.gameObject.activeSelf)
                    {
                        currentTarget = null;
                        if (interactionPrompt != null)
                            interactionPrompt.SetActive(false);
                    }
                }
            }
            else if (currentTarget != null)
            {
                currentTarget.Unhighlight();
                currentTarget = null;
                if (interactionPrompt != null)
                    interactionPrompt.SetActive(false);
            }
        }
        else if (currentTarget != null)
        {
            currentTarget.Unhighlight();
            currentTarget = null;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }

    bool IsAnyInputActive()
    {
        SecretWordTrigger[] triggers = FindObjectsOfType<SecretWordTrigger>();
        foreach (SecretWordTrigger trigger in triggers)
        {
            if (trigger.IsInputActive())
                return true;
        }
        return false;
    }
}