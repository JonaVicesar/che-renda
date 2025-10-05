using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    [SerializeField] private GameObject MenuInicialUI;
    public void CrearEstacion()
    {
        SceneManager.LoadScene(1);
    }
    public void Opciones()
    {
        Debug.Log("Cargar Menu Opciones");
    }
    public void Salir()
    {
        Debug.Log("Salir del Juego");
        Application.Quit();
    }
}