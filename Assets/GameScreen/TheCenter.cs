using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TheCenter: MonoBehaviour
{
	public LevelManager levelManager;
	public Image loadingCircle;
	//public Rigidbody2D rigidbody2D;

	internal byte toNextLevel;
	int trainsOnCenter;
	public Text trainsOnCenterText;
	//bool endGame;

	internal int TrainsOnCenter {
		get
		{
			return trainsOnCenter;
		}
		set
		{
			if (levelManager.isGameStart)
			{
				trainsOnCenter = value;
				loadingCircle.fillAmount = (float)trainsOnCenter / (float)toNextLevel;
				if (trainsOnCenter == toNextLevel)
				{
					levelManager.GameStateChange(1);
				}
				trainsOnCenterText.text = "" + trainsOnCenter;
			}
			else
			{
				trainsOnCenter = 0;
				loadingCircle.fillAmount = (float)trainsOnCenter / (float)toNextLevel;
				trainsOnCenterText.text = "" + trainsOnCenter;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Enemy enemy = collision.GetComponentInParent<Enemy>();
		if (enemy)
		{
			if (trainsOnCenter > 5)
			{
				levelManager.LostTrainsFromCentre(5);
				TrainsOnCenter -= 5;
			}
			else if(trainsOnCenter > 0)
			{
				levelManager.LostTrainsFromCentre(trainsOnCenter);
				TrainsOnCenter = 0;
			}
			else
			{
				levelManager.GameStateChange(0);
			}
			StartCoroutine(enemy.Exploid());
		}
	}
	internal void TrainsCountSetZero()
	{
		TrainsOnCenter = 0;
	}
	private void Update()
	{
		if (TrainsOnCenter > 0)
		{
			if (levelManager.TrainsCount - trainsOnCenter < levelManager.road.MaxRoatingTrainsCount)
			{
				TrainsOnCenter--;
				levelManager.AddTrainInCenter();
			}
		}
	}
}