using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class Best: MonoBehaviour
{

	public int best;
	public bool newBest;

	void Start()
	{
		MainMenuManager mmm = GameObject.FindGameObjectWithTag("MMM").GetComponent<MainMenuManager>();
		if (mmm.newBest)
		{
			GetComponent<Text>().text = "New Best: " + mmm.parametersSaveLoad.Best;
			mmm.newBest = false;
			newBest = true;
			best = mmm.parametersSaveLoad.Best;
		} else
		{
			GetComponent<Text>().text = "Best: " + mmm.parametersSaveLoad.Best;
		}
	}
}
