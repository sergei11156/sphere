using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenu: MonoBehaviour
{
	bool activate;

	public void ChangeState()
	{
		activate = !activate;
		gameObject.SetActive(activate);
	}

	public void GoToMenu()
	{
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1;
	}
}
