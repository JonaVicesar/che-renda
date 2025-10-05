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

    [Header("Datos del Módulo")]
    public ModuleData moduleData;

    private bool isRegisteredInStation = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Obtener ModuleData si no está asignado
        if (moduleData == null)
        {
            ModuleTooltipTrigger tooltip = GetComponent<ModuleTooltipTrigger>();
            if (tooltip != null)
            {
                moduleData = tooltip.moduleData;
            }
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

        // Si ya estaba colocado, quitar del registro mientras se arrastra
        if (isRegisteredInStation && EnergyManager.Instance != null && moduleData != null)
        {
            EnergyManager.Instance.UnregisterModule(moduleData);
            isRegisteredInStation = false;
        }

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

        bool isOverDropZone = CheckIfOverDropZone(eventData);

        if (!isOverDropZone)
        {
            // Devolver a posición original
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);
            rectTransform.anchoredPosition = originalPosition;

            // Si venía de la estación, volver a registrar
            if (!isInventoryItem && EnergyManager.Instance != null && moduleData != null)
            {
                EnergyManager.Instance.RegisterModule(moduleData);
                isRegisteredInStation = true;
            }

            Debug.Log("Devuelto a posición original");
        }
        else
        {
            // Verificar si hay suficiente energía antes de colocar
            if (moduleData != null && EnergyManager.Instance != null)
            {
                if (!EnergyManager.Instance.CanPlaceModule(moduleData))
                {
                    Debug.LogWarning("¡No hay suficiente energía para colocar este módulo!");

                    // Mostrar mensaje al jugador (opcional)
                    ShowEnergyWarning();

                    // Devolver al inventario
                    transform.SetParent(originalParent);
                    transform.SetSiblingIndex(originalSiblingIndex);
                    rectTransform.anchoredPosition = originalPosition;
                    return;
                }
            }

            // Si viene del inventario, crear copia
            if (isInventoryItem)
            {
                GameObject inventoryCopy = Instantiate(gameObject, originalParent);
                inventoryCopy.transform.SetSiblingIndex(originalSiblingIndex);

                DraggableItem copyScript = inventoryCopy.GetComponent<DraggableItem>();
                RectTransform copyRect = inventoryCopy.GetComponent<RectTransform>();
                copyRect.anchoredPosition = originalPosition;

                copyScript.isInventoryItem = true;
                copyScript.placedModulesContainer = placedModulesContainer;
                copyScript.moduleData = moduleData;

                Debug.Log("Copia creada en el inventario");

                isInventoryItem = false;
            }

            // Mover al contenedor de módulos colocados
            if (placedModulesContainer != null)
            {
                transform.SetParent(placedModulesContainer);
            }
            else
            {
                transform.SetParent(canvas.transform);
            }

            // Registrar en el sistema de energía
            if (EnergyManager.Instance != null && moduleData != null)
            {
                EnergyManager.Instance.RegisterModule(moduleData);
                isRegisteredInStation = true;
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

    private void ShowEnergyWarning()
    {
        // Aquí puedes mostrar un mensaje visual al jugador
        Debug.LogWarning($"¡Energía insuficiente! Necesitas generar más energía antes de colocar {moduleData.moduleName}");

        // Opcional: crear un sistema de notificaciones
        // NotificationManager.Instance?.ShowWarning("¡Energía insuficiente!");
    }

    public void SetDroppedOnValidZone(bool value)
    {
        droppedOnValidZone = value;
    }

    private void OnDestroy()
    {
        // Si el módulo se destruye, quitarlo del registro
        if (isRegisteredInStation && EnergyManager.Instance != null && moduleData != null)
        {
            EnergyManager.Instance.UnregisterModule(moduleData);
        }
    }
}