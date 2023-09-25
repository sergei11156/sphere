using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders: MonoBehaviour
{

	public LevelManager levelManager;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log(collision);
		Train train;
		if (train = collision.GetComponentInParent<Train>())
		{
			levelManager.TrainDie(train);
		}
		Destroy(collision.gameObject);
	}

}
