using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformParameters: MonoBehaviour
{
	public RectTransform rectTransform;

	internal Vector2 leftEdge;
	internal Vector2 rightEdge;
	internal Vector2 pos;

	void Start()
	{
		Camera c = Camera.main;
		Vector2 distanceFromCenterToEdge = new Vector2(rectTransform.rect.width / 2, rectTransform.rect.height / 2);
		float bottomEdge = rectTransform.transform.position.y - distanceFromCenterToEdge.y;
		leftEdge = new Vector2(rectTransform.transform.position.x - distanceFromCenterToEdge.x, bottomEdge);
		rightEdge = new Vector2(rectTransform.transform.position.x + distanceFromCenterToEdge.x, bottomEdge);

		leftEdge = c.ScreenToWorldPoint(leftEdge);
		rightEdge = c.ScreenToWorldPoint(rightEdge);
		pos = c.ScreenToWorldPoint(rectTransform.transform.position);
	}
}
