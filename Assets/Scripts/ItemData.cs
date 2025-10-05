using UnityEngine;

[CreateAssetMenu(fileName = "NewModuleData", menuName = "Space Station/Module Data")]
public class ModuleData : ScriptableObject
{
    [Header("Informaci�n B�sica")]
    public string moduleName = "M�dulo";
    public Sprite icon;

    [Header("Estad�sticas")]
    public int energyConsumption = 0; // Negativo si consume, positivo si genera
    public int capacity = 0; // Capacidad de personas
    public float weight = 0; // Peso en kg

    [Header("Categor�a")]
    public ModuleCategory category;

    [Header("Descripci�n")]
    [TextArea(3, 5)]
    public string description = "Descripci�n del m�dulo";

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