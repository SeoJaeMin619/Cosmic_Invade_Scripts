using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool isDragging = false;
    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform canvasRectTransform;
    private float X;
    private float Y;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        X = rectTransform.position.x - eventData.position.x;
        Y = rectTransform.position.y - eventData.position.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, new Vector3(eventData.position.x + X, eventData.position.y + Y), eventData.pressEventCamera, out Vector2 localPoint);
            rectTransform.localPosition = localPoint;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}