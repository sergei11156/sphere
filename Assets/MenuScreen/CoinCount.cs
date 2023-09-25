using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCount: MonoBehaviour
{
	internal MainMenuManager mmm;
	void Start()
	{
		mmm = GameObject.FindGameObjectWithTag("MMM").GetComponent<MainMenuManager>();
		mmm.CoinCount = gameObject.GetComponent<Text>();
		mmm.UpdateCoin();
	}

}
