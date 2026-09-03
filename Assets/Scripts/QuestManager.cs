using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Oggetti richiesti")]
    public List<string> requiredItems = new List<string>();

    [Header("Oggetti da sbloccare")]
    public GameObject bookOfRecipes;

    [Header("UI")]
    public TextMeshProUGUI progressText;
    public Image progressFill;
    public TextMeshProUGUI fillText;
    public GameObject completionMessage;

    private List<string> collectedItems = new List<string>();
    private bool isBookUnlocked = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (bookOfRecipes != null)
            bookOfRecipes.SetActive(false);

        if (completionMessage != null)
            completionMessage.SetActive(false);

        UpdateProgressUI();
    }

    public void ItemCollected(string itemName)
    {
        if (!collectedItems.Contains(itemName) && requiredItems.Contains(itemName))
        {
            collectedItems.Add(itemName);
            UpdateProgressUI();

            if (collectedItems.Count >= requiredItems.Count && !isBookUnlocked)
            {
                UnlockBook();
            }
        }
    }

    void UnlockBook()
    {
        isBookUnlocked = true;

        if (bookOfRecipes != null)
            bookOfRecipes.SetActive(true);

        if (completionMessage != null)
        {
            completionMessage.SetActive(true);
            Invoke("HideCompletionMessage", 5f);
        }

        if (UIManager.Instance != null)
            UIManager.Instance.ShowMessage("Tutti gli oggetti sono stati raccolti! Trova il libro aperto per la prossima missione!");

        CheckAllCompleted();

        Debug.Log("Libro sbloccato!");
    }

    void CheckAllCompleted()
    {
        bool allItemsCollected = collectedItems.Count >= requiredItems.Count;

        bool allMinigamesCompleted = false;
        if (MinigameProgressManager.Instance != null)
        {
            allMinigamesCompleted =
                MinigameProgressManager.Instance.puzzleCompleted &&
                MinigameProgressManager.Instance.alchemyCompleted &&
                MinigameProgressManager.Instance.finalEnigmaCompleted;
        }

        if (allItemsCollected && allMinigamesCompleted)
        {
            if (FinalDemoManager.Instance != null)
            {
                FinalDemoManager.Instance.ShowFinalDemo();
            }
        }
    }

    void HideCompletionMessage()
    {
        if (completionMessage != null)
            completionMessage.SetActive(false);
    }

    void UpdateProgressUI()
    {
        int collected = collectedItems.Count;
        int total = requiredItems.Count;
        float progress = total > 0 ? (float)collected / total : 0f;

        if (progressText != null)
        {
            if (collected >= total)
            {
                progressText.text = "Missione completata! Trova il libro!";
                progressText.color = Color.green;
            }
            else
            {
                progressText.text = "Oggetti raccolti: " + collected + "/" + total;
                progressText.color = Color.white;
            }
        }

        if (progressFill != null)
        {
            progressFill.fillAmount = progress;

            if (progress >= 1f)
                progressFill.color = Color.green;
            else if (progress >= 0.5f)
                progressFill.color = Color.yellow;
            else
                progressFill.color = new Color(0.2f, 0.6f, 1f);
        }

        if (fillText != null)
            fillText.text = ((int)(progress * 100)) + "%";
    }

    public bool HasAllItems()
    {
        return collectedItems.Count >= requiredItems.Count;
    }
}