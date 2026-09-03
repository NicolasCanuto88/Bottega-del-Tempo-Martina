using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI References")]
    public GameObject inventoryPanel;
    public Button toggleButton;
    public Transform inventoryContent;
    public GameObject itemTemplate;
    public TextMeshProUGUI emptyMessage;

    [Header("Dati")]
    public List<string> items = new List<string>();

    private List<GameObject> itemUIs = new List<GameObject>();
    private bool isOpen = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Debug.Log("InventoryManager Start - Inizializzazione...");

        if (itemTemplate != null)
            itemTemplate.SetActive(false);

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
            Debug.Log("InventoryPanel nascosto all'inizio");
        }
        else
        {
            Debug.LogWarning("inventoryPanel non assegnato!");
        }

        if (toggleButton != null)
        {
            toggleButton.onClick.RemoveAllListeners();
            toggleButton.onClick.AddListener(ToggleInventory);
            Debug.Log("ToggleButton collegato!");
        }
        else
        {
            Debug.LogWarning("toggleButton non assegnato!");
        }

        UpdateInventoryUI();
    }

    void Update()
    {
        // Controlla se un campo input è attivo
        if (IsAnyInputFieldActive())
            return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Tasto I premuto!");
            ToggleInventory();
        }
    }

    bool IsAnyInputFieldActive()
    {
        // Controlla se il SecretWordPanel è attivo
        SecretWordTrigger trigger = FindObjectOfType<SecretWordTrigger>();
        if (trigger != null && trigger.IsInputActive())
            return true;

        return false;
    }

    public void ToggleInventory()
    {
        Debug.Log("ToggleInventory chiamato! isOpen = " + isOpen);

        isOpen = !isOpen;
        Debug.Log("Nuovo stato: " + isOpen);

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(isOpen);
            Debug.Log("Panel attivo: " + inventoryPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning("inventoryPanel e NULL!");
            return;
        }

        if (isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        UpdateInventoryUI();
        Debug.Log("Aggiunto: " + itemName);
    }

    public void RemoveItem(string itemName)
    {
        items.Remove(itemName);
        UpdateInventoryUI();
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }

    public void UpdateInventoryUI()
    {
        if (inventoryContent == null)
        {
            Debug.LogWarning("inventoryContent e NULL!");
            return;
        }

        foreach (GameObject ui in itemUIs)
        {
            Destroy(ui);
        }
        itemUIs.Clear();

        if (items.Count == 0)
        {
            if (emptyMessage != null)
                emptyMessage.gameObject.SetActive(true);
            return;
        }

        if (emptyMessage != null)
            emptyMessage.gameObject.SetActive(false);

        foreach (string item in items)
        {
            if (itemTemplate == null)
            {
                Debug.LogWarning("itemTemplate e NULL!");
                return;
            }

            GameObject newItem = Instantiate(itemTemplate, inventoryContent);
            newItem.SetActive(true);

            TextMeshProUGUI itemText = newItem.GetComponentInChildren<TextMeshProUGUI>();
            if (itemText != null)
            {
                itemText.text = item;
            }

            itemUIs.Add(newItem);
        }
    }
}