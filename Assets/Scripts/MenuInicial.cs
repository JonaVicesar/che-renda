using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    [SerializeField] private GameObject MenuInicialUI;
    public void CrearEstacion()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Opciones()
    {
        Debug.Log("Cargar Menu Opciones");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    public void Salir()
    {
        Debug.Log("Salir del Juego");
        Application.Quit();
    }
}