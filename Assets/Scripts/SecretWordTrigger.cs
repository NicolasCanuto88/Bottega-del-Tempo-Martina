using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SecretWordTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject inputPanel;
    public TMP_InputField inputField;
    public TextMeshProUGUI feedbackText;
    public Button confirmButton;
    public Button closeButton;

    [Header("Parola Segreta")]
    public string secretWord = "ACIDO ACETICO";

    [Header("Da Sbloccare")]
    public AlchemyCombinationGame alchemyGame;

    private bool isUnlocked = false;
    private bool isInputActive = false;

    void Start()
    {
        if (inputPanel != null)
            inputPanel.SetActive(false);

        if (alchemyGame != null)
            alchemyGame.gamePanel.SetActive(false);

        if (confirmButton != null)
            confirmButton.onClick.AddListener(CheckSecretWord);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseInputPanel);
    }

    public void OpenInputPanel()
    {
        if (isUnlocked)
        {
            if (alchemyGame != null)
                alchemyGame.OpenGame();
            return;
        }

        if (inputPanel != null)
        {
            inputPanel.SetActive(true);
            isInputActive = true;
            inputField.text = "";
            feedbackText.text = "Inserisci la formula segreta...";
            feedbackText.color = Color.white;

            // FORZA IL CURSORE VISIBILE E METTI IN PAUSA
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            inputField.Select();
            inputField.ActivateInputField();
        }
    }

    void CheckSecretWord()
    {
        string input = inputField.text.ToUpper().Trim();

        if (input == secretWord.ToUpper())
        {
            isUnlocked = true;
            feedbackText.text = "Formula corretta! Chiudi questo pannello per proseguire";
            feedbackText.color = Color.black;

            if (alchemyGame != null)
                alchemyGame.OpenGame();

            UIManager.Instance.ShowMessage("Il calderone si illumina!");

            Invoke("CloseInputPanel", 2f);
        }
        else
        {
            feedbackText.text = "Formula sbagliata! Riprova...";
            feedbackText.color = Color.red;
            inputField.text = "";

            inputField.Select();
            inputField.ActivateInputField();
        }
    }

    void CloseInputPanel()
    {
        if (inputPanel != null)
            inputPanel.SetActive(false);

        isInputActive = false;

        // RIPRISTINA IL CURSORE
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public bool IsInputActive()
    {
        return isInputActive;
    }
}