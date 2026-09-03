using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;
    public float messageDuration = 3f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;

        if (messagePanel != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowMessageCoroutine());
        }

        Debug.Log("Messaggio UI: " + message);
    }

    IEnumerator ShowMessageCoroutine()
    {
        messagePanel.SetActive(true);
        yield return new WaitForSeconds(messageDuration);
        messagePanel.SetActive(false);
    }
}