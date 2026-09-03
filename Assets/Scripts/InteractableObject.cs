using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Testo dell'oggetto")]
    [TextArea(3, 5)]
    public string message = "Questo e' un oggetto misterioso...";

    [Header("Colore di evidenziazione")]
    public Color highlightColor = Color.yellow;
    private Color originalColor;
    private Renderer objectRenderer;
    private bool isHighlighted = false;

    [Header("Opzioni")]
    public bool canPickUp = false;
    public GameObject objectToActivate;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    public void Highlight()
    {
        if (!isHighlighted && objectRenderer != null)
        {
            objectRenderer.material.color = highlightColor;
            isHighlighted = true;
        }
    }

    public void Unhighlight()
    {
        if (isHighlighted && objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
            isHighlighted = false;
        }
    }

    public void Interact()
    {
        if (IsAnyInputActive())
            return;

        if (UIManager.Instance != null)
            UIManager.Instance.ShowMessage(message);
        else
            Debug.Log("Messaggio: " + message);

        SecretWordTrigger secretTrigger = GetComponent<SecretWordTrigger>();
        if (secretTrigger != null)
        {
            secretTrigger.OpenInputPanel();
            return;
        }

        if (canPickUp)
        {
            if (InventoryManager.Instance != null)
                InventoryManager.Instance.AddItem(gameObject.name);
            else
                Debug.Log("Raccolto: " + gameObject.name);
            gameObject.SetActive(false);
        }

        if (objectToActivate != null)
            objectToActivate.SetActive(true);
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