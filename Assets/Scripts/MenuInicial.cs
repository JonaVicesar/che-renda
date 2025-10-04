using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    [SerializeField] private GameObject menuInicialUI;
    [SerializeField] private GameObject creditosUI;

    public void CrearEstacion()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Salir()
    {
        Debug.Log("Salir del Juego");
        Application.Quit();
    }
}