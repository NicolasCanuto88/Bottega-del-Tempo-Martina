using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class FormulaPuzzleGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject puzzlePanel;
    public Transform tileContainer;
    public GameObject tilePrefab;
    public List<SlotDrop> slots = new List<SlotDrop>();
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI clueText;
    public Button checkButton;
    public Button closeButton;

    [Header("Impostazioni")]
    public string secretWord = "Acido acetico";
    public List<Sprite> tileImages = new List<Sprite>();
    public int gridSize = 4;

    private List<TileDrag> tiles = new List<TileDrag>();
    private bool isCompleted = false;
    private bool isOpen = false;

    void Start()
    {
        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (checkButton != null)
            checkButton.onClick.AddListener(CheckPuzzle);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePuzzle);

        InitializePuzzle();
    }

    public void OpenPuzzle()
    {
        if (isOpen) return;

        isOpen = true;

        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        ResetPuzzle();
    }

    void InitializePuzzle()
    {
        if (tileImages.Count != 16)
        {
            Debug.LogWarning("Servono 16 immagini per il puzzle! Ne hai " + tileImages.Count);
            return;
        }

        List<int> shuffledIDs = new List<int>();
        for (int i = 0; i < 16; i++)
            shuffledIDs.Add(i);

        for (int i = 0; i < shuffledIDs.Count; i++)
        {
            int temp = shuffledIDs[i];
            int randomIndex = Random.Range(i, shuffledIDs.Count);
            shuffledIDs[i] = shuffledIDs[randomIndex];
            shuffledIDs[randomIndex] = temp;
        }

        foreach (int id in shuffledIDs)
        {
            GameObject tileObj = Instantiate(tilePrefab, tileContainer);
            tileObj.SetActive(true);

            TileDrag tile = tileObj.GetComponent<TileDrag>();
            if (tile != null)
            {
                tile.tileID = id;
                tile.tileImage = tileImages[id];

                Image img = tileObj.GetComponent<Image>();
                if (img != null)
                {
                    img.sprite = tileImages[id];
                    img.preserveAspect = true;
                }
            }

            tiles.Add(tile);
        }
    }

    void ResetPuzzle()
    {
        foreach (TileDrag tile in tiles)
        {
            if (tile != null)
                tile.ResetToStart();
        }

        foreach (SlotDrop slot in slots)
        {
            if (slot != null && slot.GetTile() != null)
                slot.RemoveTile();
        }

        isCompleted = false;
        resultText.text = "Componi l'immagine...";
        resultText.color = Color.white;

        if (clueText != null)
            clueText.text = "Metti i pezzi nell'ordine giusto per rivelare la formula!";
    }

    void CheckPuzzle()
    {
        if (isCompleted) return;

        bool allCorrect = true;
        bool allFilled = true;

        foreach (SlotDrop slot in slots)
        {
            if (slot == null) continue;

            if (slot.IsEmpty())
            {
                allFilled = false;
                break;
            }

            if (!slot.IsCorrect())
                allCorrect = false;
        }

        if (!allFilled)
        {
            resultText.text = "Attenzione: Metti tutti i pezzi!";
            resultText.color = Color.yellow;
            return;
        }

        if (allCorrect)
        {
            isCompleted = true;
            resultText.text = "IMMAGINE COMPLETA! La formula e': " + secretWord;
            resultText.color = Color.green;

            OnPuzzleCompleted();
        }
        else
        {
            resultText.text = "Alcuni pezzi non sono nell'ordine giusto!";
            resultText.color = Color.red;
            Invoke("ResetPuzzle", 2f);
        }
    }

    void OnPuzzleCompleted()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.ShowMessage("La formula segreta e': " + secretWord);

        if (MinigameProgressManager.Instance != null)
            MinigameProgressManager.Instance.CompletePuzzle();

        GameObject objectToUnlock = GameObject.Find("OggettoDaSbloccare");
        if (objectToUnlock != null)
            objectToUnlock.SetActive(true);

        Invoke("ClosePuzzle", 3f);
    }

    void ClosePuzzle()
    {
        isOpen = false;

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}