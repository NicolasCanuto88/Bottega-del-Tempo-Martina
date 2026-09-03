using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class FinalDemoManager : MonoBehaviour
{
    public static FinalDemoManager Instance;

    [Header("UI")]
    public GameObject finalPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;
    public Button mainMenuButton;

    private bool isFinalShown = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (finalPanel != null)
            finalPanel.SetActive(false);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void ShowFinalDemo()
    {
        if (isFinalShown) return;
        isFinalShown = true;

        if (finalPanel != null)
        {
            finalPanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Finale Demo mostrato!");
        }
    }

    void GoToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.GoToMainMenu();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}