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

    [Header("Configuraci�n")]
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
    /// Registra un nuevo m�dulo colocado
    /// </summary>
    public void RegisterModule(ModuleData data)
    {
        if (data == null) return;

        if (data.isEnergyGenerator || data.energyValue > 0)
        {
            totalEnergyGenerated += data.energyValue;
            Debug.Log($"Generador a�adido: +{data.energyValue} kW");
        }
        else if (data.energyValue < 0)
        {
            totalEnergyConsumed += Mathf.Abs(data.energyValue);
            Debug.Log($"Consumidor a�adido: -{Mathf.Abs(data.energyValue)} kW");
        }

        availableEnergy = totalEnergyGenerated - totalEnergyConsumed;
        UpdateEnergyUI();
    }

    /// <summary>
    /// Quita un m�dulo cuando se remueve de la estaci�n
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
    /// Verifica si hay suficiente energ�a para colocar un m�dulo
    /// </summary>
    public bool CanPlaceModule(ModuleData data)
    {
        if (data == null) return true;

        // Si es generador, siempre se puede colocar
        if (data.isEnergyGenerator || data.energyValue > 0)
        {
            return true;
        }

        // Si consume energ�a, verificar que haya suficiente
        float consumption = Mathf.Abs(data.energyValue);
        return availableEnergy >= consumption;
    }

    /// <summary>
    /// Actualiza la interfaz visual de energ�a
    /// </summary>
    private void UpdateEnergyUI()
    {
        // Actualizar texto
        energyText.text = $"Energ�a: {availableEnergy:F1} kW\n" +
                         $"<size=70%>Generando: {totalEnergyGenerated:F1} kW | Consumiendo: {totalEnergyConsumed:F1} kW</size>";

        // Si no hay generaci�n, la barra est� vac�a
        if (totalEnergyGenerated <= 0)
        {
            energySlider.value = 0;
            return;
        }

        // Calcular porcentaje de energ�a disponible
        float energyPercentage = Mathf.Clamp01(availableEnergy / totalEnergyGenerated);
        energySlider.value = energyPercentage;

    }

    /// <summary>
    /// Obtiene informaci�n del estado actual
    /// </summary>
    public string GetEnergyStatus()
    {
        if (availableEnergy < 0)
        {
            return "�ENERG�A INSUFICIENTE!";
        }
        else if (availableEnergy == 0 && totalEnergyConsumed > 0)
        {
            return "Energ�a al l�mite";
        }
        else if (totalEnergyGenerated == 0)
        {
            return "Sin generaci�n de energ�a";
        }
        else
        {
            return "Energ�a estable";
        }
    }

    // M�todos p�blicos para obtener valores
    public float GetAvailableEnergy() => availableEnergy;
    public float GetTotalGenerated() => totalEnergyGenerated;
    public float GetTotalConsumed() => totalEnergyConsumed;
}