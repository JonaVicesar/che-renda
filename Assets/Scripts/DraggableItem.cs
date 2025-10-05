using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;
    private CanvasGroup canvasGroup;
    private bool droppedOnValidZone = false;

    private Transform originalParent;
    private int originalSiblingIndex;

    [Header("Configuración")]
    public Transform placedModulesContainer; 
    public bool isInventoryItem = true; 

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        droppedOnValidZone = false;

        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        canvas = GetComponentInParent<Canvas>();
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;

        Debug.Log("Comenzando a arrastrar: " + gameObject.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        bool isOverDropZone = CheckIfOverDropZone(eventData); //zona valida

        if (!isOverDropZone)
        {
            //devolver a posicion orgiinal
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);
            rectTransform.anchoredPosition = originalPosition;
            Debug.Log("Devuelto a posición original");
        }
        else
        {
            if (isInventoryItem)
            {
                GameObject inventoryCopy = Instantiate(gameObject, originalParent);
                inventoryCopy.transform.SetSiblingIndex(originalSiblingIndex);

                DraggableItem copyScript = inventoryCopy.GetComponent<DraggableItem>();
                RectTransform copyRect = inventoryCopy.GetComponent<RectTransform>();
                copyRect.anchoredPosition = originalPosition;

                copyScript.isInventoryItem = true;
                copyScript.placedModulesContainer = placedModulesContainer;

                Debug.Log("Copia creada en el inventario");

                isInventoryItem = false;
            }

            if (placedModulesContainer != null)
            {
                transform.SetParent(placedModulesContainer);
            }
            else
            {
                transform.SetParent(canvas.transform);
            }

            Debug.Log("Módulo colocado en el espacio");
        }
    }

    private bool CheckIfOverDropZone(PointerEventData eventData)
    {
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<DropZone>() != null)
            {
                return true;
            }
        }
        return false;
    }

    public void SetDroppedOnValidZone(bool value)
    {
        droppedOnValidZone = value;
    }
}