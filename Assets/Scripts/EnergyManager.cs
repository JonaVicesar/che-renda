using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;

    [Header("Referencias UI")]
    public Slider energySlider;
    public TextMeshProUGUI energyText;
 
    [Header("Colores")]
    public Color positiveColor = Color.green;
    public Color warningColor = Color.yellow;
    public Color criticalColor = Color.red;

    [Header("Configuración")]
    [Tooltip("Umbral para color amarillo (%)")]
    public float warningThreshold = 0.3f;
    [Tooltip("Umbral para color rojo (%)")]
    public float criticalThreshold = 0.1f;

    // Estado actual
    private float totalEnergyGenerated = 0f;
    private float totalEnergyConsumed = 0f;
    private float availableEnergy = 0f;

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

        UpdateEnergyUI();
    }

    /// <summary>
    /// Registra un nuevo módulo colocado
    /// </summary>
    public void RegisterModule(ModuleData data)
    {
        if (data == null) return;

        if (data.isEnergyGenerator || data.energyValue > 0)
        {
            totalEnergyGenerated += data.energyValue;
            Debug.Log($"Generador añadido: +{data.energyValue} kW");
        }
        else if (data.energyValue < 0)
        {
            totalEnergyConsumed += Mathf.Abs(data.energyValue);
            Debug.Log($"Consumidor añadido: -{Mathf.Abs(data.energyValue)} kW");
        }

        availableEnergy = totalEnergyGenerated - totalEnergyConsumed;
        UpdateEnergyUI();
    }

    /// <summary>
    /// Quita un módulo cuando se remueve de la estación
    /// </summary>
    public void UnregisterModule(ModuleData data)
    {
        if (data == null) return;

        if (data.isEnergyGenerator || data.energyValue > 0)
        {
            totalEnergyGenerated -= data.energyValue;
            Debug.Log($"Generador removido: -{data.energyValue} kW");
        }
        else if (data.energyValue < 0)
        {
            totalEnergyConsumed -= Mathf.Abs(data.energyValue);
            Debug.Log($"Consumidor removido: +{Mathf.Abs(data.energyValue)} kW");
        }

        availableEnergy = totalEnergyGenerated - totalEnergyConsumed;
        UpdateEnergyUI();
    }

    /// <summary>
    /// Verifica si hay suficiente energía para colocar un módulo
    /// </summary>
    public bool CanPlaceModule(ModuleData data)
    {
        if (data == null) return true;

        // Si es generador, siempre se puede colocar
        if (data.isEnergyGenerator || data.energyValue > 0)
        {
            return true;
        }

        // Si consume energía, verificar que haya suficiente
        float consumption = Mathf.Abs(data.energyValue);
        return availableEnergy >= consumption;
    }

    /// <summary>
    /// Actualiza la interfaz visual de energía
    /// </summary>
    private void UpdateEnergyUI()
    {
        // Actualizar texto
        energyText.text = $"Energía: {availableEnergy:F1} kW\n" +
                         $"<size=70%>Generando: {totalEnergyGenerated:F1} kW | Consumiendo: {totalEnergyConsumed:F1} kW</size>";

        // Si no hay generación, la barra está vacía
        if (totalEnergyGenerated <= 0)
        {
            energySlider.value = 0;
            return;
        }

        // Calcular porcentaje de energía disponible
        float energyPercentage = Mathf.Clamp01(availableEnergy / totalEnergyGenerated);
        energySlider.value = energyPercentage;

    }

    /// <summary>
    /// Obtiene información del estado actual
    /// </summary>
    public string GetEnergyStatus()
    {
        if (availableEnergy < 0)
        {
            return "¡ENERGÍA INSUFICIENTE!";
        }
        else if (availableEnergy == 0 && totalEnergyConsumed > 0)
        {
            return "Energía al límite";
        }
        else if (totalEnergyGenerated == 0)
        {
            return "Sin generación de energía";
        }
        else
        {
            return "Energía estable";
        }
    }

    // Métodos públicos para obtener valores
    public float GetAvailableEnergy() => availableEnergy;
    public float GetTotalGenerated() => totalEnergyGenerated;
    public float GetTotalConsumed() => totalEnergyConsumed;
}