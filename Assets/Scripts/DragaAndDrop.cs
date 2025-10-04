using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystem;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [Serializable] private Canvas canvas;
    private RectTransform rectTransform;
    private canvasGroup canvasGroup;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<canvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        transform.position = Input.mousePosition;
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
    }
}