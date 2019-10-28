using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	public void NewGame()
	{
		SceneManager.LoadScene("Game");
	}

	public void QuitGame()
	{
		UnityEditor.EditorApplication.isPlaying = false;
		//Application.Quit();
	}
}
