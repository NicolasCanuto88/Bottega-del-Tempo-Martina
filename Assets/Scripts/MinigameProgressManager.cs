using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameProgressManager : MonoBehaviour
{
    public static MinigameProgressManager Instance;

    [Header("UI")]
    public Slider progressSlider;
    public TextMeshProUGUI progressText;

    [Header("Minigiochi")]
    public bool puzzleCompleted = false;
    public bool alchemyCompleted = false;
    public bool finalEnigmaCompleted = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void CompletePuzzle()
    {
        puzzleCompleted = true;
        UpdateUI();
        Debug.Log("Puzzle completato!");
    }

    public void CompleteAlchemy()
    {
        alchemyCompleted = true;
        UpdateUI();
        Debug.Log("Alchimia completata!");
    }

    public void CompleteFinalEnigma()
    {
        finalEnigmaCompleted = true;
        UpdateUI();
        Debug.Log("Enigma finale completato!");
    }

    public void ResetProgress()
    {
        puzzleCompleted = false;
        alchemyCompleted = false;
        finalEnigmaCompleted = false;
        UpdateUI();
    }

    void UpdateUI()
    {
        int completed = 0;
        if (puzzleCompleted) completed++;
        if (alchemyCompleted) completed++;
        if (finalEnigmaCompleted) completed++;

        int total = 3;
        float progress = (float)completed / total;

        if (progressSlider != null)
            progressSlider.value = progress;

        if (progressText != null)
        {
            if (completed >= total)
            {
                progressText.text = "Tutti i minigiochi completati!";
                progressText.color = Color.green;

                CheckAllCompleted();
            }
            else
            {
                progressText.text = "Minigiochi: " + completed + "/" + total;
                progressText.color = Color.white;
            }
        }
    }

    void CheckAllCompleted()
    {
        bool allItemsCollected = false;
        if (QuestManager.Instance != null)
        {
            allItemsCollected = QuestManager.Instance.HasAllItems();
        }

        bool allMinigamesCompleted = puzzleCompleted && alchemyCompleted && finalEnigmaCompleted;

        if (allItemsCollected && allMinigamesCompleted)
        {
            if (FinalDemoManager.Instance != null)
            {
                FinalDemoManager.Instance.ShowFinalDemo();
            }
        }
    }
}