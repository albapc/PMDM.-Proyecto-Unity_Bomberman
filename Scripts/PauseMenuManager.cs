using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {

    GameObject[] pauseObjects;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        HidePaused();
	}
	
	// Update is called once per frame
	void Update () {
		
        //boton P para pausar y reanudar el juego
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0;
                ShowPaused();
            } else if(Time.timeScale == 0)
            {
                Debug.Log("High");
                Time.timeScale = 1;
                HidePaused();
            }
        }
	}

    //reinicia el nivel
    public void Reload()
    {
        SceneManager.LoadScene("Game");
    }

    //controla las pausas de la escena
    public void PauseControl()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
            ShowPaused();
        } else if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
            HidePaused();
        }
    }

    //muestra los objetos con la etiqueta ShowOnPause
    public void ShowPaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    //oculta los objetos con la etiqueta ShowOnPause
    public void HidePaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    //carga el nivel indicado, en este caso lo usaremos para volver al menu principal
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}
