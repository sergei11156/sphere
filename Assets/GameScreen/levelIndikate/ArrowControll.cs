using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControll : MonoBehaviour {

	internal IEnumerator LevelUp()
	{
		const float defaultMaxPosition = 80;
		float time = 0.5f;
		RectTransform rectTransform = GetComponent<RectTransform>();
		Vector2 deltaPosition = new Vector2 (0, (defaultMaxPosition - rectTransform.anchoredPosition.y)/time);
		while(time >= 0)
		{
			rectTransform.anchoredPosition += deltaPosition * Time.deltaTime;
			time -= Time.deltaTime;
			yield return null;
		}
	}
}
