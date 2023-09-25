using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingController: MonoBehaviour
{
	public const float mxlimit = 0f;
	public float mLimit;
	RectTransform rectTransform;
	Vector3 minLimit;
	Vector3 maxLimit;
	bool litlCotsilForYou;
	private bool isMoveOut = true;

	private void Start()
	{
		rectTransform = GetComponent<ScrollRect>().viewport.GetChild(0).GetComponent<RectTransform>();
		minLimit = new Vector3(mLimit, rectTransform.position.y, 0f);
		maxLimit = new Vector3(mxlimit, rectTransform.position.y, 0f);
	}

	public void OnValueChange()
	{
		///Debug.Log("rtfpos: " + rectTransform.position.x + " minlim: " + mLimit + " maxLim: " + mxlimit);
		if (rectTransform.position.x < mLimit)
		{
			rectTransform.position = minLimit;
		}
		else if (rectTransform.position.x > mxlimit)
		{
			rectTransform.position = maxLimit;
		}
	}
	internal void ChangeState(bool moveOut)
	{
		if (moveOut)
		{
			if (isMoveOut)
			{
				StopCoroutine(MoveY(0));
				StartCoroutine(MoveY(-250f));
				isMoveOut = false;
			}
		}
		else
		{
			if (!isMoveOut)
			{
				StopCoroutine(MoveY(0));
				StartCoroutine(MoveY(250f));
				isMoveOut = true;
			}
		}
	}
	private IEnumerator MoveY(float deltaY)
	{
		float time = 1f;
		RectTransform ret = GetComponent<RectTransform>();
		Vector2 deltaPos = new Vector2(0, deltaY  / time);
		while (time >= 0)
		{
			ret.anchoredPosition += deltaPos * Time.deltaTime;
			time -= Time.deltaTime;
			yield return null;
		}
	}
}
