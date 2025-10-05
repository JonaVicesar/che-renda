using UnityEngine;

[CreateAssetMenu(fileName = "NewModuleData", menuName = "Módulos/Module Data")]
public class ModuleData : ScriptableObject
{
    [Header("Información Básica")]
    public string moduleName = "Módulo";

    [Header("Características")]
    public bool isEnergyGenerator = false; // Si es un generador de energía

    [Tooltip("Energía que consume (-) o genera (+)")]
    public float energyValue = 0f; // Negativo si consume, positivo si genera

    [Tooltip("Capacidad de personas que pueden habitar")]
    public int capacity = 0;

    [Tooltip("Oxígeno que genera")]
    public float oxygenGeneration = 0f;

    [Header("Descripción")]
    [TextArea(2, 4)]
    public string description = "";
}