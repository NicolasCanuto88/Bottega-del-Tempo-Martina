using UnityEngine;

public class FormulaPuzzleTrigger : MonoBehaviour
{
    [Header("Puzzle")]
    public FormulaPuzzleGame formulaPuzzle;

    void Start()
    {
        if (formulaPuzzle == null)
            formulaPuzzle = FindObjectOfType<FormulaPuzzleGame>();
    }

    public void TriggerPuzzle()
    {
        if (formulaPuzzle != null)
        {
            formulaPuzzle.OpenPuzzle();
        }
        else
        {
            Debug.LogWarning("FormulaPuzzleGame non trovato!");
        }
    }
}