using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    [Header("Minigioco")]
    public GameObject alchemyMiniGamePanel;  // Il pannello UI del minigioco
    public string requiredRecipe = "Zolfo + Mercurio + Sale"; // Ricetta corretta

    private bool isGameUnlocked = false;

    void Start()
    {
        if (alchemyMiniGamePanel != null)
            alchemyMiniGamePanel.SetActive(false);
    }

    public void OpenBook()
    {
        if (QuestManager.Instance != null && QuestManager.Instance.HasAllItems())
        {
            // Sblocca il minigioco
            isGameUnlocked = true;

            if (alchemyMiniGamePanel != null)
            {
                alchemyMiniGamePanel.SetActive(true);
                UIManager.Instance?.ShowMessage("Ora componi la pozione seguendo la ricetta!");

                // Blocca il movimento del giocatore
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else
        {
            UIManager.Instance?.ShowMessage("Il libro è chiuso. Devi trovare tutti gli ingredienti prima!");
        }
    }

    public void CloseMiniGame()
    {
        if (alchemyMiniGamePanel != null)
            alchemyMiniGamePanel.SetActive(false);

        // Riattiva il movimento
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}