using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart: MonoBehaviour
{
	bool flyAway = false;
	Vector2 speed;

	internal void FlyAway() //Vector2 speed)
	{
		speed = transform.localPosition * 20;
		flyAway = true;
	}

	private void Update()
	{
		if (flyAway)
		{
			transform.position += (Vector3)speed * Time.deltaTime;
		}
	}
}
