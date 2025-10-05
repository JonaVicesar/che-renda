using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Datos del M�dulo")]
    public ModuleData moduleData;

    [Header("Configuraci�n")]
    [Tooltip("Si es true, el tooltip solo se muestra cuando el m�dulo est� colocado")]
    public bool onlyShowWhenPlaced = true;

    private bool isShowingTooltip = false;
    private DraggableItem draggableItem;

    private void Awake()
    {
        draggableItem = GetComponent<DraggableItem>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Si est� configurado para mostrar solo cuando est� colocado
        if (onlyShowWhenPlaced && draggableItem != null && draggableItem.isInventoryItem)
        {
            return; // No mostrar tooltip si est� en inventario
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
        // Actualizar posici�n del tooltip mientras el mouse est� sobre el objeto
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