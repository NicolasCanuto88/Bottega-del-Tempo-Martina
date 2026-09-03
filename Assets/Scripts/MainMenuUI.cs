using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject saveFilesPanel;
    public Transform saveFilesContent;
    public GameObject saveFileTemplate;

    [Header("Button References")]
    public Button newGameButton;
    public Button loadGameButton;
    public Button deleteButton;
    public Button exitButton;

    private SaveData selectedSave = null;

    void Start()
    {
        Debug.Log("MainMenuUI Start - Inizializzazione...");

        if (saveFilesContent == null)
        {
            Debug.LogWarning("saveFilesContent NON ASSEGNATO!");
        }
        else
        {
            Debug.Log("saveFilesContent assegnato: " + saveFilesContent.name);
        }

        if (saveFileTemplate == null)
        {
            Debug.LogWarning("saveFileTemplate NON ASSEGNATO!");
        }
        else
        {
            Debug.Log("saveFileTemplate assegnato: " + saveFileTemplate.name);
        }

        if (saveFilesPanel != null)
            saveFilesPanel.SetActive(false);

        if (saveFileTemplate != null)
            saveFileTemplate.SetActive(false);

        if (newGameButton != null)
            newGameButton.onClick.AddListener(NewGame);

        if (loadGameButton != null)
            loadGameButton.onClick.AddListener(LoadGame);

        if (deleteButton != null)
            deleteButton.onClick.AddListener(DeleteSave);

        if (exitButton != null)
            exitButton.onClick.AddListener(QuitGame);

        RefreshSaveListUI();
    }

    public void NewGame()
    {
        Debug.Log("Nuova partita!");

        // Trova il GameManager nella scena corrente
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.NewGame();
        }
        else
        {
            Debug.LogWarning("GameManager non trovato!");
        }
    }

    public void LoadGame()
    {
        if (selectedSave != null)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                Debug.Log("Caricamento salvataggio: " + selectedSave.saveName);
                gm.LoadGame(selectedSave);
            }
            else
            {
                Debug.LogWarning("GameManager non trovato!");
            }
        }
        else
        {
            Debug.LogWarning("Nessun salvataggio selezionato!");
        }
    }

    public void DeleteSave()
    {
        if (selectedSave != null)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                Debug.Log("Eliminazione salvataggio: " + selectedSave.saveName);
                gm.DeleteSave(selectedSave);
                RefreshSaveListUI();
                selectedSave = null;
            }
            else
            {
                Debug.LogWarning("GameManager non trovato!");
            }
        }
        else
        {
            Debug.LogWarning("Nessun salvataggio selezionato da eliminare!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Uscita dal gioco!");

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.QuitGame();
        }
        else
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    public void ToggleSaveFilesPanel()
    {
        if (saveFilesPanel != null)
        {
            bool isOpen = saveFilesPanel.activeSelf;
            saveFilesPanel.SetActive(!isOpen);
            if (!isOpen)
            {
                RefreshSaveListUI();
            }
        }
    }

    public void RefreshSaveListUI()
    {
        Debug.Log("RefreshSaveListUI chiamato!");

        if (saveFilesContent == null)
        {
            Debug.LogWarning("saveFilesContent e NULL!");
            return;
        }

        foreach (Transform child in saveFilesContent)
        {
            if (child.gameObject != saveFileTemplate)
                Destroy(child.gameObject);
        }

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm == null)
        {
            Debug.LogWarning("GameManager non trovato!");
            return;
        }

        List<SaveData> saves = gm.GetSaveFiles();
        Debug.Log("Trovati " + saves.Count + " salvataggi");

        if (saves.Count == 0)
        {
            Debug.Log("Nessun salvataggio trovato!");
            return;
        }

        foreach (SaveData save in saves)
        {
            GameObject newItem = Instantiate(saveFileTemplate, saveFilesContent);
            newItem.SetActive(true);

            TextMeshProUGUI text = newItem.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = save.saveName + "\n" + save.saveDate + " - " + save.currentScene + " - " + save.collectedItems.Count + " oggetti";
            }

            Button btn = newItem.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => SelectSave(save));
            }
        }
    }

    void SelectSave(SaveData save)
    {
        selectedSave = save;
        Debug.Log("Salvataggio selezionato: " + save.saveName);
    }
}