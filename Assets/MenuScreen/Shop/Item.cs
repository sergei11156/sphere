using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;


public class Item: MonoBehaviour
{
	public bool isThatFirstItem;
	public int itemIndex;
	public Sprite activate;
	public Sprite deActivate;
	public Image image;
	public int price;
	internal MainMenuManager mmm;
	Text textPrice;

	//const string achivStrangeThing = "CgkIscWHzrYREAIQAw";
	//const string achivSputnik = "CgkIscWHzrYREAIQAg";

	void Start()
	{
		textPrice = GetComponentInChildren<Text>();
		if (textPrice)
		{
			textPrice.text = price + "$";
		}

		mmm = GameObject.FindGameObjectWithTag("MMM").GetComponent<MainMenuManager>();
		if (mmm.isFirstStart)
		{
			if (isThatFirstItem)
			{
				mmm.ActiveItem = this;
				PlayerPrefs.SetInt(itemIndex + "ib", 1);
				PlayerPrefs.SetInt("activ", itemIndex);
				image.sprite = activate;
				textPrice.gameObject.SetActive(false);
			}
			else
			{
				PlayerPrefs.SetInt(itemIndex + "ib", 0);
				image.sprite = deActivate;
			}
		}
		else
		{
			if(PlayerPrefs.GetInt(itemIndex + "ib") != 0)
			{
				textPrice.gameObject.SetActive(false);
			}

			if(PlayerPrefs.GetInt("activ") == itemIndex)
			{
				image.sprite = activate;
				mmm.ActiveItem = this;
			}
			else
			{
				image.sprite = deActivate;
			}
		}
	}

	void DeActivate()
	{
		image.sprite = deActivate;
	}

	public void Activate()
	{
		if (mmm.ActiveItem != this)
		{
			if (PlayerPrefs.GetInt(itemIndex + "ib") != 0)
			{
				mmm.ActiveItem.DeActivate();
				mmm.ActiveItem = this;
				PlayerPrefs.SetInt("activ", itemIndex);
				image.sprite = activate;
				mmm.levelManager.ChangeTrains();
			}
			else
			{
				if (mmm.parametersSaveLoad.CoinCount >= price)
				{
					mmm.parametersSaveLoad.CoinCount -= price;
					mmm.UpdateCoin();
					PlayerPrefs.SetInt(itemIndex + "ib", 1);
					
					mmm.ActiveItem.DeActivate();
					mmm.ActiveItem = this;
					PlayerPrefs.SetInt("activ", itemIndex);

					image.sprite = activate;
					textPrice.gameObject.SetActive(false);

					mmm.levelManager.ChangeTrains();
				}
			}
		}
	}
}
