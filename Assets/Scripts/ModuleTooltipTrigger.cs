using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Datos del Módulo")]
    public ModuleData moduleData;

    [Header("Configuración")]
    [Tooltip("Si es true, el tooltip solo se muestra cuando el módulo está colocado")]
    public bool onlyShowWhenPlaced = true;

    private bool isShowingTooltip = false;
    private DraggableItem draggableItem;

    private void Awake()
    {
        draggableItem = GetComponent<DraggableItem>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Si está configurado para mostrar solo cuando está colocado
        if (onlyShowWhenPlaced && draggableItem != null && draggableItem.isInventoryItem)
        {
            return; // No mostrar tooltip si está en inventario
        }

        if (moduleData != null && TooltipManager.Instance != null)
        {
            TooltipManager.Instance.ShowTooltip(moduleData, eventData.position);
            isShowingTooltip = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isShowingTooltip && TooltipManager.Instance != null)
        {
            TooltipManager.Instance.HideTooltip();
            isShowingTooltip = false;
        }
    }

    private void Update()
    {
        // Actualizar posición del tooltip mientras el mouse esté sobre el objeto
        if (isShowingTooltip && TooltipManager.Instance != null)
        {
            TooltipManager.Instance.UpdateTooltipPosition(Input.mousePosition);
        }
    }

    private void OnDisable()
    {
        // Ocultar tooltip si el objeto se desactiva
        if (isShowingTooltip && TooltipManager.Instance != null)
        {
            TooltipManager.Instance.HideTooltip();
            isShowingTooltip = false;
        }
    }
}   