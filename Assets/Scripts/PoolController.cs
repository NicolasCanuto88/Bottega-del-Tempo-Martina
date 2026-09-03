using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class WelcomeManager : MonoBehaviour
{
    public GameObject welcomePanel;
    public TextMeshProUGUI welcomeText;
    public TextMeshProUGUI subtitleText;
    public Button startButton;

    private bool isFirstTime = true;

    void Start()
    {
        // Controlla se è la prima volta che si gioca
        if (PlayerPrefs.GetInt("FirstTime", 1) == 1)
        {
            ShowWelcome();
            PlayerPrefs.SetInt("FirstTime", 0);
            PlayerPrefs.Save();
        }
        else
        {
            if (welcomePanel != null)
                welcomePanel.SetActive(false);
        }

        if (startButton != null)
            startButton.onClick.AddListener(HideWelcome);
    }

    void ShowWelcome()
    {
        if (welcomePanel != null)
        {
            welcomePanel.SetActive(true);
            Time.timeScale = 0f; // Mette in pausa il gioco durante il benvenuto
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HideWelcome()
    {
        if (welcomePanel != null)
        {
            welcomePanel.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}