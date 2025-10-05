using UnityEngine;

[CreateAssetMenu(fileName = "NewModuleData", menuName = "Space Station/Module Data")]
public class ModuleData : ScriptableObject
{
    [Header("Información Básica")]
    public string moduleName = "Módulo";
    public Sprite icon;

    [Header("Estadísticas")]
    public int energyConsumption = 0; // Negativo si consume, positivo si genera
    public int capacity = 0; // Capacidad de personas
    public float weight = 0; // Peso en kg

    [Header("Categoría")]
    public ModuleCategory category;

    [Header("Descripción")]
    [TextArea(3, 5)]
    public string description = "Descripción del módulo";

    [Header("Propiedades Adicionales")]
    public bool requiresOxygen = true;
    public bool generatesOxygen = false;
    public int storageCapacity = 0; // Para almacenes
}

public enum ModuleCategory
{
    Habitacion,
    Energia,
    Comunicacion,
    Almacenamiento,
    Investigacion,
    Ocio
}