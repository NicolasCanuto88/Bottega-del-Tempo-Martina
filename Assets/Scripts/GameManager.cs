using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Salvataggio")]
    public string saveFolderName = "Saves";
    public List<SaveData> saveFiles = new List<SaveData>();

    [Header("Scene")]
    public string mainMenuScene = "MainMenu";
    public string gameScene = "GameScene";

    private string savePath;
    private bool isPaused = false;
    public event Action<bool> OnPauseStateChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("GameManager creato in: " + gameObject.scene.name);
        }
        else
        {
            Debug.Log("GameManager duplicato distrutto in: " + gameObject.scene.name);
            Destroy(gameObject);
            return;
        }

        savePath = Path.Combine(Application.persistentDataPath, saveFolderName);
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        RefreshSaveList();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scena caricata: " + scene.name);

        if (scene.name == "GameScene")
        {
            StartCoroutine(CreateWelcomePanelAfterDelay());
        }
    }

    System.Collections.IEnumerator CreateWelcomePanelAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);
        CreateWelcomePanel();
    }

    void CreateWelcomePanel()
    {
        Debug.Log("CreateWelcomePanel chiamato!");

        WelcomePanel existing = FindObjectOfType<WelcomePanel>(true);
        if (existing != null)
        {
            Debug.Log("WelcomePanel gia esistente");
            return;
        }

        GameObject panelUI = null;
        GameObject canvas = GameObject.Find("GameUI");

        if (canvas != null)
        {
            Transform[] allTransforms = canvas.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in allTransforms)
            {
                if (t.name == "WelcomePanel")
                {
                    panelUI = t.gameObject;
                    Debug.Log("WelcomePanel trovato in GameUI (anche disattivato)!");
                    break;
                }
            }
        }

        if (panelUI == null)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject go in allObjects)
            {
                if (go.name == "WelcomePanel" && go.scene.name == "GameScene")
                {
                    panelUI = go;
                    Debug.Log("WelcomePanel trovato con Resources.Find!");
                    break;
                }
            }
        }

        if (panelUI == null)
        {
            Debug.LogError("WelcomePanel UI non trovato! Verifica che esista nella scena.");
            return;
        }

        Debug.Log("Panel UI trovato: " + panelUI.name);

        WelcomePanel wp = gameObject.AddComponent<WelcomePanel>();
        wp.welcomePanel = panelUI;

        Button startBtn = null;
        if (canvas != null)
        {
            Transform[] allTransforms = canvas.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in allTransforms)
            {
                if (t.name == "StartButton")
                {
                    startBtn = t.GetComponent<Button>();
                    break;
                }
            }
        }

        if (startBtn == null)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject go in allObjects)
            {
                if (go.name == "StartButton" && go.scene.name == "GameScene")
                {
                    startBtn = go.GetComponent<Button>();
                    if (startBtn != null)
                    {
                        Debug.Log("StartButton trovato con Resources.Find!");
                        break;
                    }
                }
            }
        }

        if (startBtn != null)
        {
            wp.startButton = startBtn;
            Debug.Log("WelcomePanel configurato!");
        }
        else
        {
            Debug.LogError("StartButton non trovato!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void NewGame()
    {
        Debug.Log("NewGame chiamato!");

        SaveData newSave = new SaveData();
        newSave.saveName = "Partita_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
        newSave.saveDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        newSave.currentScene = gameScene;
        newSave.playerPosition = Vector3.zero;
        newSave.collectedItems = new List<string>();

        saveFiles.Add(newSave);
        SaveDataToFile(newSave);
        RefreshSaveList();

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameScene);

        Debug.Log("Scena caricata: " + gameScene);
    }

    public void LoadGame(SaveData saveData)
    {
        if (saveData == null) return;

        Debug.Log("Caricamento salvataggio: " + saveData.saveName);

        SceneManager.LoadScene(saveData.currentScene);
        StartCoroutine(ApplySaveDataAfterLoad(saveData));
    }

    private System.Collections.IEnumerator ApplySaveDataAfterLoad(SaveData saveData)
    {
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = saveData.playerPosition;
        }

        if (InventoryManager.Instance != null)
        {
            foreach (string item in saveData.collectedItems)
            {
                InventoryManager.Instance.AddItem(item);
            }
        }

        if (QuestManager.Instance != null)
        {
            foreach (string item in saveData.collectedItems)
            {
                QuestManager.Instance.ItemCollected(item);
            }
        }

        Debug.Log("Salvataggio caricato!");
    }

    public void SaveCurrentGame()
    {
        Debug.Log("Salvataggio in corso...");

        if (saveFiles.Count == 0)
        {
            Debug.Log("Creazione nuovo salvataggio...");
            SaveData newSave = new SaveData();
            newSave.saveName = "Partita_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
            newSave.saveDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            newSave.currentScene = SceneManager.GetActiveScene().name;
            SaveCurrentGameData(newSave);
            saveFiles.Add(newSave);
            SaveDataToFile(newSave);
            Debug.Log("Nuovo salvataggio creato!");
        }
        else
        {
            Debug.Log("Aggiornamento salvataggio esistente...");
            SaveData currentSave = saveFiles[saveFiles.Count - 1];
            currentSave.saveDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            currentSave.currentScene = SceneManager.GetActiveScene().name;
            SaveCurrentGameData(currentSave);
            SaveDataToFile(currentSave);
            Debug.Log("Salvataggio aggiornato!");
        }

        RefreshSaveList();
        Debug.Log("Gioco salvato!");
    }

    private void SaveCurrentGameData(SaveData saveData)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            saveData.playerPosition = player.transform.position;
        }

        if (InventoryManager.Instance != null)
        {
            saveData.collectedItems = new List<string>(InventoryManager.Instance.items);
        }
    }

    public void DeleteSave(SaveData saveData)
    {
        if (saveData == null) return;

        string filePath = Path.Combine(savePath, saveData.saveName + ".json");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        saveFiles.Remove(saveData);
        RefreshSaveList();
        Debug.Log("Salvataggio eliminato!");
    }

    public void QuitGame()
    {
        Debug.Log("Uscita dal gioco!");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        OnPauseStateChanged?.Invoke(isPaused);
    }

    public void RestartScene()
    {
        Debug.Log("Riavvio scena...");
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Debug.Log("Torno al menu principale...");
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void RefreshSaveList()
    {
        saveFiles.Clear();

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
            return;
        }

        string[] files = Directory.GetFiles(savePath, "*.json");
        foreach (string file in files)
        {
            try
            {
                string json = File.ReadAllText(file);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                if (data != null)
                {
                    saveFiles.Add(data);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Errore nel caricare il file: " + e.Message);
            }
        }
    }

    private void SaveDataToFile(SaveData data)
    {
        string filePath = Path.Combine(savePath, data.saveName + ".json");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public List<SaveData> GetSaveFiles()
    {
        return saveFiles;
    }
}

[System.Serializable]
public class SaveData
{
    public string saveName;
    public string saveDate;
    public string currentScene;
    public Vector3 playerPosition;
    public List<string> collectedItems = new List<string>();
}