using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class AlchemyCombinationGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject gamePanel;
    public TMP_Dropdown element1Dropdown;
    public TMP_Dropdown element2Dropdown;
    public TextMeshProUGUI resultText;
    public Button combineButton;
    public Button closeButton;

    [Header("Sistema di Sfida")]
    public TextMeshProUGUI progressText;
    public int combinazioniRichieste = 3;
    public int tentativiMassimi = 4;
    private int combinazioniCorrette = 0;
    private int tentativiUsati = 0;
    private bool gameOver = false;
    private bool vittoria = false;

    [Header("Dati")]
    public List<SimpleElement> elements = new List<SimpleElement>();
    public List<SimpleReaction> reactions = new List<SimpleReaction>();

    void Start()
    {
        if (gamePanel != null)
            gamePanel.SetActive(false);

        if (combineButton != null)
            combineButton.onClick.AddListener(CombineElements);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseGame);

        FillDropdowns();
        ResetGame();
    }

    public void OpenGame()
    {
        if (gamePanel != null)
        {
            gamePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        ResetGame();
        resultText.text = "Trova 3 combinazioni chimiche!";
        resultText.color = Color.white;
    }

    void ResetGame()
    {
        combinazioniCorrette = 0;
        tentativiUsati = 0;
        gameOver = false;
        vittoria = false;
        UpdateProgressUI();
        resultText.text = "Trova 3 combinazioni chimiche!";
        resultText.color = Color.white;

        element1Dropdown.value = 0;
        element2Dropdown.value = 0;
    }

    void FillDropdowns()
    {
        List<string> elementNames = new List<string>();
        foreach (SimpleElement e in elements)
        {
            elementNames.Add(e.elementName);
        }

        element1Dropdown.ClearOptions();
        element1Dropdown.AddOptions(elementNames);

        element2Dropdown.ClearOptions();
        element2Dropdown.AddOptions(elementNames);
    }

    void UpdateProgressUI()
    {
        if (progressText != null)
        {
            progressText.text = string.Format("Combinazioni: {0}/{1}  |  Tentativi: {2}/{3}",
                combinazioniCorrette,
                combinazioniRichieste,
                tentativiUsati,
                tentativiMassimi);
        }
    }

    void CombineElements()
    {
        if (gameOver)
        {
            resultText.text = "Il calderone e' freddo... Ricomincia il gioco!";
            resultText.color = Color.red;
            return;
        }

        if (vittoria)
        {
            resultText.text = "Hai gia' vinto! Hai creato tutte le combinazioni!";
            resultText.color = Color.green;
            return;
        }

        int index1 = element1Dropdown.value;
        int index2 = element2Dropdown.value;

        if (index1 == index2)
        {
            resultText.text = "Attenzione: Scegli due elementi diversi!";
            resultText.color = Color.yellow;
            return;
        }

        SimpleElement elem1 = elements[index1];
        SimpleElement elem2 = elements[index2];

        SimpleReaction foundReaction = null;
        foreach (SimpleReaction reaction in reactions)
        {
            bool match1 = (reaction.element1 == elem1.elementName && reaction.element2 == elem2.elementName);
            bool match2 = (reaction.element1 == elem2.elementName && reaction.element2 == elem1.elementName);

            if (match1 || match2)
            {
                foundReaction = reaction;
                break;
            }
        }

        tentativiUsati++;

        if (foundReaction != null)
        {
            combinazioniCorrette++;
            resultText.text = string.Format("{0} + {1} = {2}!  ({3}/{4})",
                elem1.elementName,
                elem2.elementName,
                foundReaction.result,
                combinazioniCorrette,
                combinazioniRichieste);
            resultText.color = Color.green;

            UIManager.Instance.ShowMessage(string.Format("Nuovo elemento creato: {0}!",
                foundReaction.result));

            if (combinazioniCorrette >= combinazioniRichieste)
            {
                Vittoria();
            }
        }
        else
        {
            resultText.text = string.Format("{0} + {1} = NESSUNA REAZIONE!  ({2}/{3} tentativi)",
                elem1.elementName,
                elem2.elementName,
                tentativiUsati,
                tentativiMassimi);
            resultText.color = Color.red;

            if (tentativiUsati >= tentativiMassimi)
            {
                Sconfitta();
            }
        }

        UpdateProgressUI();
    }

    void Vittoria()
    {
        vittoria = true;
        resultText.text = "VITTORIA! Hai creato tutte le combinazioni chimiche!";
        resultText.color = Color.green;
        UIManager.Instance.ShowMessage("Hai completato tutte le combinazioni alchemiche!");

        // NOTIFICA IL COMPLETAMENTO
        if (MinigameProgressManager.Instance != null)
            MinigameProgressManager.Instance.CompleteAlchemy();

        // CONTROLLA SE TUTTO E' COMPLETATO
        CheckAllCompleted();

        Invoke("CloseGame", 3f);
    }

    void CheckAllCompleted()
    {
        bool allItemsCollected = false;
        if (QuestManager.Instance != null)
        {
            allItemsCollected = QuestManager.Instance.HasAllItems();
        }

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

    void Sconfitta()
    {
        gameOver = true;
        resultText.text = "Il calderone si e' spento! Hai esaurito i tentativi. Ricomincia!";
        resultText.color = Color.red;
        UIManager.Instance.ShowMessage("Il calderone si e' spento! Ritenta...");

        Invoke("CloseGame", 3f);
    }

    void CloseGame()
    {
        if (gamePanel != null)
            gamePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (gameOver)
        {
            ResetGame();
        }
    }
}

[System.Serializable]
public class SimpleElement
{
    public string elementName;
}

[System.Serializable]
public class SimpleReaction
{
    public string element1;
    public string element2;
    public string result;
}