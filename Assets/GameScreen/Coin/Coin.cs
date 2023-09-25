using System.Collections;
using UnityEngine;

public class Coin: Enemy
{
	internal RectTransformParameters targetRect;

	protected override IEnumerator UpScaleAndStartMove()
	{
		yield return StartCoroutine(Rescale(maxSize));
		rb2.velocity = new Vector2(-rb2.position.x, 0).normalized * speed;
	}

	internal new IEnumerator Die()
	{
		GetComponent<CircleCollider2D>().enabled = false;
		rb2.velocity = (targetRect.pos - rb2.position).normalized * 5;
		StartCoroutine(Rescale(minSize));
		bool check = true;
		bool w = true;
		while (w)
		{
			yield return null;
			if (transform.position.y > targetRect.leftEdge.y &&
			transform.position.x < targetRect.rightEdge.x &&
			transform.position.x > targetRect.leftEdge.x)
			{
				w = false;
				levelManager.CoinCount++;
				Destroy(gameObject);
			}
			if (check)
			{
				if (transform.position.y > targetRect.leftEdge.y - 1 &&
					transform.position.x < targetRect.rightEdge.x - 0.25f &&
					transform.position.x > targetRect.leftEdge.x + 0.25f)
				{
					StartCoroutine(Disappear(0.3f));
					check = false;
				}
			}
		}
	}
}
