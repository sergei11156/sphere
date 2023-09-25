using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColor: MonoBehaviour
{

	public Camera myCamera;
	public CircleChangeColor circleChange;

	private MainMenuManager mainMenuManager;

	// Use this for initialization
	void Start()
	{
		mainMenuManager = GameObject.FindGameObjectWithTag("MMM").GetComponent<MainMenuManager>();
		myCamera.backgroundColor = mainMenuManager.colors[mainMenuManager.randomColor];
	}

	internal IEnumerator ChangeColor(Vector2 pos)
	{
		mainMenuManager.NewBackgroundColor();
		yield return StartCoroutine(circleChange.Grow(mainMenuManager.colors[mainMenuManager.randomColor], pos));
		myCamera.backgroundColor = mainMenuManager.colors[mainMenuManager.randomColor];
	}
}
