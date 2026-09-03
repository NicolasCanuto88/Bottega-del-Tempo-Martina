using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;
    public Button pauseButton;
    public Button closeButton;
    public Button resumeButton;
    public Button saveButton;
    public Button loadButton;
    public Button restartButton;
    public Button mainMenuButton;

    private bool isPaused = false;

    void Start()
    {
        Debug.Log("========================================");
        Debug.Log("PauseManager Start - Controllo bottoni...");

        // PauseButton
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(TogglePause);
            Debug.Log("PauseButton collegato!");
        }
        else
        {
            Debug.LogWarning("PauseButton NON ASSEGNATO!");
        }

        // CloseButton
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(TogglePause);
            Debug.Log("CloseButton collegato!");
        }
        else
        {
            Debug.LogWarning("CloseButton NON ASSEGNATO!");
        }

        // ResumeButton
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(ResumeGame);
            Debug.Log("ResumeButton collegato!");
        }
        else
        {
            Debug.LogWarning("ResumeButton NON ASSEGNATO!");
        }

        // SaveButton
        if (saveButton != null)
        {
            saveButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(SaveGame);
            Debug.Log("SaveButton collegato!");
        }
        else
        {
            Debug.LogWarning("SaveButton NON ASSEGNATO!");
        }

        // LoadButton
        if (loadButton != null)
        {
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(LoadGame);
            Debug.Log("LoadButton collegato!");
        }
        else
        {
            Debug.LogWarning("LoadButton NON ASSEGNATO!");
        }

        // RestartButton
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
            Debug.Log("RestartButton collegato!");
        }
        else
        {
            Debug.LogWarning("RestartButton NON ASSEGNATO!");
        }

        // MainMenuButton
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            Debug.Log("MainMenuButton collegato!");
        }
        else
        {
            Debug.LogWarning("MainMenuButton NON ASSEGNATO!");
        }

        // PausePanel
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            Debug.Log("PausePanel nascosto all'inizio");
        }
        else
        {
            Debug.LogWarning("PausePanel NON ASSEGNATO!");
        }

        Debug.Log("PauseManager inizializzato!");
        Debug.Log("========================================");
    }

    public void TogglePause()
    {
        Debug.Log("TogglePause chiamato! isPaused = " + isPaused);

        isPaused = !isPaused;

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
            Debug.Log("PausePanel attivato? " + isPaused);
            Debug.Log("PausePanel activeSelf: " + pausePanel.activeSelf);
            Debug.Log("PausePanel position: " + pausePanel.transform.position);
        }
        else
        {
            Debug.LogWarning("ausePanel è NULL!");
            return;
        }

        if (isPaused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("GIOCO IN PAUSA");
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("GIOCO RIPRESO");
        }
    }

    public void ResumeGame()
    {
        Debug.Log("ResumeGame chiamato!");
        if (isPaused)
            TogglePause();
    }

    public void SaveGame()
    {
        Debug.Log("SaveGame chiamato!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveCurrentGame();
            Debug.Log("Salvataggio completato!");
        }
        else
        {
            Debug.LogWarning("GameManager.Instance è NULL!");
        }
    }

    public void LoadGame()
    {
        Debug.Log("LoadGame chiamato!");
        if (GameManager.Instance != null)
        {
            var saves = GameManager.Instance.GetSaveFiles();
            Debug.Log("Trovati " + saves.Count + " salvataggi");
            if (saves.Count > 0)
            {
                GameManager.Instance.LoadGame(saves[saves.Count - 1]);
                Debug.Log("Caricamento completato!");
            }
            else
            {
                Debug.LogWarning("Nessun salvataggio trovato!");
            }
        }
        else
        {
            Debug.LogWarning("GameManager.Instance è NULL!");
        }
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame chiamato!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartScene();
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GoToMainMenu()
    {
        Debug.Log("GoToMainMenu chiamato!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToMainMenu();
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}