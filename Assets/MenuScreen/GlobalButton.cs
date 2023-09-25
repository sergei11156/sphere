using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalButton : MonoBehaviour {
	public LevelManager levelManager;

	public void OnClick()
	{
		levelManager.Click();
	}
}
