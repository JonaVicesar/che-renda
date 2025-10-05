using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop llamado en DropZone!");

        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggableItem != null)
        {
            draggableItem.SetDroppedOnValidZone(true);
            eventData.pointerDrag.transform.SetParent(transform);
            Debug.Log("Item soltado correctamente");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Puntero entr� en DropZone");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Puntero sali� de DropZone");
    }
}