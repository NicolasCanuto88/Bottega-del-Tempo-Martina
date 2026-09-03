using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Tessera")]
    public int tileID;
    public Sprite tileImage;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;
    private Transform startParent;
    private Image image;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();

        // Se non c'è CanvasGroup, lo aggiunge
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Salva il parent iniziale
        startParent = transform.parent;
        startPosition = rectTransform.anchoredPosition;

        if (image != null && tileImage != null)
        {
            image.sprite = tileImage;
            image.preserveAspect = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
        startParent = transform.parent;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }

        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            GameObject target = eventData.pointerCurrentRaycast.gameObject;
            SlotDrop slot = target.GetComponent<SlotDrop>();
            if (slot != null && slot.IsEmpty())
            {
                slot.PlaceTile(this);
                transform.SetParent(slot.transform);
                rectTransform.anchoredPosition = Vector2.zero;
                return;
            }
        }

        // Torna alla posizione iniziale
        if (startParent != null)
        {
            transform.SetParent(startParent);
            rectTransform.anchoredPosition = startPosition;
        }
    }

    public void ResetToStart()
    {
        // Controlla che startParent non sia null
        if (startParent != null)
        {
            transform.SetParent(startParent);
            rectTransform.anchoredPosition = startPosition;
        }
        else
        {
            // Se startParent è null, usa il parent corrente
            startParent = transform.parent;
            startPosition = rectTransform.anchoredPosition;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void SetImage(Sprite newImage)
    {
        tileImage = newImage;
        if (image != null)
            image.sprite = newImage;
    }
}