using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [Header("Referencias UI")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI capacityText;
    public TextMeshProUGUI oxygenText;
    public TextMeshProUGUI descriptionText;

    [Header("Configuración")]
    public Vector2 offset = new Vector2(10, 10);

    private RectTransform tooltipRect;
    private Canvas canvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        canvas = tooltipPanel.GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
        }

        HideTooltip();
    }

    public void ShowTooltip(ModuleData data, Vector2 mousePosition)
    {
        if (data == null) return;

        tooltipPanel.SetActive(true);

        // Configurar textos
        nameText.text = data.moduleName;

        // Energía
        if (data.isEnergyGenerator)
        {
            energyText.text = $"<color=#FFD700>Genera: {data.energyValue} kW</color>";
        }
        else
        {
            energyText.text = $"Consume: {Mathf.Abs(data.energyValue)} kW";
        }

        // Capacidad
        if (data.capacity > 0)
        {
            capacityText.text = $"Capacidad: {data.capacity} personas";
            capacityText.gameObject.SetActive(true);
        }
        else
        {
            capacityText.gameObject.SetActive(false);
        }

        // Oxígeno
        if (data.oxygenGeneration > 0)
        {
            oxygenText.text = $"Oxigeno: +{data.oxygenGeneration} L/min";
            oxygenText.gameObject.SetActive(true);
        }
        else
        {
            oxygenText.gameObject.SetActive(false);
        }

        // Descripción
        if (!string.IsNullOrEmpty(data.description))
        {
            descriptionText.text = data.description;
            descriptionText.gameObject.SetActive(true);
        }
        else
        {
            descriptionText.gameObject.SetActive(false);
        }

        // Posicionar tooltip
        UpdateTooltipPosition(mousePosition);
    }

    public void UpdateTooltipPosition(Vector2 mousePosition)
    {
        if (!tooltipPanel.activeSelf) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePosition,
            canvas.worldCamera,
            out localPoint
        );

        tooltipRect.localPosition = localPoint + offset;

        // Ajustar para que no se salga de la pantalla
        Vector3[] corners = new Vector3[4];
        tooltipRect.GetWorldCorners(corners);

        RectTransform canvasRect = canvas.transform as RectTransform;

        // Ajustar horizontalmente
        if (corners[2].x > Screen.width)
        {
            float diff = corners[2].x - Screen.width;
            tooltipRect.localPosition = new Vector2(tooltipRect.localPosition.x - diff - 10, tooltipRect.localPosition.y);
        }

        // Ajustar verticalmente
        if (corners[1].y > Screen.height)
        {
            float diff = corners[1].y - Screen.height;
            tooltipRect.localPosition = new Vector2(tooltipRect.localPosition.x, tooltipRect.localPosition.y - diff - 10);
        }
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}