using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject BotonPausa;
    [SerializeField] private GameObject MenuPausaUI;
    public static bool JuegoPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (JuegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }
    public void Pausar()
    {
        Time.timeScale = 0f;
        BotonPausa.SetActive(false);
        MenuPausaUI.SetActive(true);
        JuegoPausado = true;
    }
    public void Reanudar()
    {
        Time.timeScale = 1f;
        BotonPausa.SetActive(true);
        MenuPausaUI.SetActive(false);
        JuegoPausado = false;
    }
    public void Opciones()
    {
        Debug.Log("Cargar Menu Opciones");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    public void MenuInicial()
    {
        Debug.Log("Cargar Menu Principal");
        Time.timeScale = 1f;
        BotonPausa.SetActive(false);
        MenuPausaUI.SetActive(false);
        JuegoPausado = false;
        SceneManager.LoadScene(0);
    }
}