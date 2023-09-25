using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause: MonoBehaviour
{
	public Animator animator;
	public Sprite play;
	public Sprite pausePic;
	public ToMenu toMenu;

	Image image;
	bool pause;

	void Start()
	{
		image = GetComponent<Image>();
		pause = false;
	}
	public void ChangeState()
	{
		if (!pause)
		{
			pause = true;
			image.sprite = play;
			animator.speed = 0f;
		} else
		{
			pause = false;
			image.sprite = pausePic;
			animator.speed = 1f;
		}
		//toMenu.ChangeState();
	}
}
