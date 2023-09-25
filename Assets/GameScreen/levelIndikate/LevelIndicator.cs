using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIndicator: MonoBehaviour
{
	MainMenuManager mmm;
	public Text levelCount;
	public ArrowControll arrowControl;

	void Start()
	{
		mmm = GameObject.FindGameObjectWithTag("MMM").GetComponent<MainMenuManager>();
		levelCount.text = "" + mmm.parametersSaveLoad.Level;
	}
	IEnumerator ShowAppaerAnimation(float time)
	{
		Image image = GetComponent<Image>();
		Color deltaColor = new Color(0,0,0, (1 - image.color.a)/time);
		while(time >= 0)
		{
			image.color += deltaColor * Time.deltaTime;
			time -= Time.deltaTime;
			yield return null;
		}
	}

	internal IEnumerator LevelUpAnimation()
	{
		yield return StartCoroutine(arrowControl.LevelUp());
		levelCount.text = "" + mmm.parametersSaveLoad.Level;
	}
}
