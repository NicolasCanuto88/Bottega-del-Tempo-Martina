using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotDrop : MonoBehaviour, IDropHandler
{
    [Header("Slot")]
    public int slotIndex;          // 0-15
    public int correctTileID;      // ID della tessera che DEVE andare qui

    private Image slotImage;
    private TileDrag currentTile;
    private Color originalColor;

    void Start()
    {
        slotImage = GetComponent<Image>();
        originalColor = slotImage.color;
    }

    public void PlaceTile(TileDrag tile)
    {
        currentTile = tile;

        // Se la tessera è corretta verde, altrimenti rosso
        if (IsCorrect())
        {
            slotImage.color = new Color(0.2f, 0.8f, 0.2f, 1f); // Verde
        }
        else
        {
            slotImage.color = new Color(0.8f, 0.2f, 0.2f, 1f); // Rosso
        }
    }

    public void RemoveTile()
    {
        currentTile = null;
        slotImage.color = originalColor;
    }

    public bool IsEmpty()
    {
        return currentTile == null;
    }

    public TileDrag GetTile()
    {
        return currentTile;
    }

    public bool IsCorrect()
    {
        if (currentTile == null) return false;
        return currentTile.tileID == correctTileID;
    }

    public void OnDrop(PointerEventData eventData)
    {
        TileDrag tile = eventData.pointerDrag?.GetComponent<TileDrag>();
        if (tile != null && IsEmpty())
        {
            PlaceTile(tile);
            tile.transform.SetParent(transform);
            tile.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}