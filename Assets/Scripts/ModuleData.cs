using UnityEngine;

[CreateAssetMenu(fileName = "NewModuleData", menuName = "M�dulos/Module Data")]
public class ModuleData : ScriptableObject
{
    [Header("Informaci�n B�sica")]
    public string moduleName = "M�dulo";

    [Header("Caracter�sticas")]
    public bool isEnergyGenerator = false; // Si es un generador de energ�a

    [Tooltip("Energ�a que consume (-) o genera (+)")]
    public float energyValue = 0f; // Negativo si consume, positivo si genera

    [Tooltip("Capacidad de personas que pueden habitar")]
    public int capacity = 0;

    [Tooltip("Ox�geno que genera")]
    public float oxygenGeneration = 0f;

    [Header("Descripci�n")]
    [TextArea(2, 4)]
    public string description = "";
}