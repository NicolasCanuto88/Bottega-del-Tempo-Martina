using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class AlchemyMinigame : MonoBehaviour
{
    [Header("Ingredienti")]
    public Button sulfurButton;
    public Button mercuryButton;
    public Button saltButton;
    public Button mixButton;

    [Header("UI")]
    public TextMeshProUGUI recipeText;
    public TextMeshProUGUI currentMixtureText;
    public GameObject successPanel;
    public GameObject failurePanel;

    [Header("Ricetta corretta")]
    public string[] correctRecipe = { "Zolfo", "Mercurio", "Sale" };

    private List<string> currentMixture = new List<string>();
    private bool isComplete = false;

    void Start()
    {
        // Nasconde i pannelli
        if (successPanel != null) successPanel.SetActive(false);
        if (failurePanel != null) failurePanel.SetActive(false);

        // Collega i bottoni
        if (sulfurButton != null) sulfurButton.onClick.AddListener(() => AddIngredient("Zolfo"));
        if (mercuryButton != null) mercuryButton.onClick.AddListener(() => AddIngredient("Mercurio"));
        if (saltButton != null) saltButton.onClick.AddListener(() => AddIngredient("Sale"));
        if (mixButton != null) mixButton.onClick.AddListener(MixPotion);

        UpdateUI();
    }

    void AddIngredient(string ingredient)
    {
        if (isComplete) return;

        currentMixture.Add(ingredient);
        UpdateUI();

        // Feedback sonoro (opzionale)
        Debug.Log("Aggiunto: " + ingredient);
    }

    void MixPotion()
    {
        if (isComplete) return;

        // Controlla se la ricetta è corretta
        bool isCorrect = true;

        if (currentMixture.Count != correctRecipe.Length)
        {
            isCorrect = false;
        }
        else
        {
            for (int i = 0; i < correctRecipe.Length; i++)
            {
                if (currentMixture[i] != correctRecipe[i])
                {
                    isCorrect = false;
                    break;
                }
            }
        }

        if (isCorrect)
        {
            // SUCCESSO!
            isComplete = true;
            if (successPanel != null) successPanel.SetActive(true);
            UIManager.Instance?.ShowMessage("Pozione creata con successo! Hai sbloccato un nuovo potere!");

            // Sblocca qualcosa (es. la porta finale, un nuovo oggetto)
            UnlockNextStep();
        }
        else
        {
            // FALLIMENTO!
            if (failurePanel != null) failurePanel.SetActive(true);
            UIManager.Instance?.ShowMessage("La miscela esplode! La ricetta non è corretta...");

            // Resetta la miscela dopo 2 secondi
            Invoke("ResetMixture", 2f);
        }
    }

    void ResetMixture()
    {
        currentMixture.Clear();
        UpdateUI();

        if (failurePanel != null) failurePanel.SetActive(false);
    }

    void UpdateUI()
    {
        if (currentMixtureText != null)
        {
            if (currentMixture.Count == 0)
                currentMixtureText.text = "Miscela: vuota";
            else
                currentMixtureText.text = "Miscela: " + string.Join(" + ", currentMixture);
        }

        if (recipeText != null)
        {
            recipeText.text = "RICETTA: " + string.Join(" + ", correctRecipe);
        }
    }

    void UnlockNextStep()
    {
        // Qui puoi attivare la prossima parte del gioco
        // Es. aprire una porta, attivare un portale, sbloccare un nuovo enigma

        Debug.Log("PROSSIMO LIVELLO SBLOCATO!");

        // Chiudi il minigioco dopo 3 secondi
        Invoke("CloseMinigame", 3f);
    }

    void CloseMinigame()
    {
        RecipeBook book = FindObjectOfType<RecipeBook>();
        if (book != null)
            book.CloseMiniGame();
    }
}