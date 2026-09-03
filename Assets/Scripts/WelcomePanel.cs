using UnityEngine;
using UnityEngine.UI;

public class WelcomePanel : MonoBehaviour
{
    [Header("UI")]
    public GameObject welcomePanel;
    public Button startButton;

    void Start()
    {
        Debug.Log("WelcomePanel Start - " + gameObject.scene.name);

        if (welcomePanel != null)
        {
            welcomePanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Benvenuto mostrato!");
        }
        else
        {
            Debug.LogWarning("welcomePanel e NULL!");
        }

        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(HideWelcome);
            Debug.Log("StartButton collegato!");
        }
        else
        {
            Debug.LogWarning("startButton non assegnato!");
        }
    }

    public void HideWelcome()
    {
        Debug.Log("HideWelcome chiamato!");
        if (welcomePanel != null)
        {
            welcomePanel.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Benvenuto chiuso!");
        }
    }
}