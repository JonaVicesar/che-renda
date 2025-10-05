using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Estaciones : MonoBehaviour
{
    public void Galaxia()
    {
        SceneManager.LoadScene(2);
    }
    public void Luna()
    {
        SceneManager.LoadScene(3);
    }
    public void Marte()
    {
        SceneManager.LoadScene(4);
    }
}
