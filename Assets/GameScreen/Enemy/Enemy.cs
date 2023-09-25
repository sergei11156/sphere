using System.Collections;
using UnityEngine;

public class Enemy: MonoBehaviour
{

	public SpriteRenderer[] parts;
	

	protected const float minSize = 0.2f;
	internal float maxSize;
	internal float speed;

	internal LevelManager levelManager;
	protected Rigidbody2D rb2;
	private bool backing;

	void Start()
	{
		rb2 = GetComponent<Rigidbody2D>();
		StartCoroutine(UpScaleAndStartMove());
	}

	virtual protected IEnumerator UpScaleAndStartMove()
	{
		transform.localScale = Vector3.zero;
		yield return StartCoroutine(Rescale(maxSize));
		rb2.velocity = -rb2.position.normalized * speed;
	}

	protected IEnumerator Rescale(float size)
	{
		Vector3 sizeVector = new Vector3(size - transform.localScale.x, size - transform.localScale.y, 0);
		if (sizeVector.x > 0)
		{
			while (transform.localScale.x < size)
			{
				transform.localScale += sizeVector * Time.deltaTime;
				yield return null;
			}
		} else
		{
			while (transform.localScale.x > size)
			{
				transform.localScale += sizeVector * Time.deltaTime;
				yield return null;
			}
		}
	}

	internal void Die()
	{
		GetComponent<CircleCollider2D>().enabled = false;
		rb2.velocity = Vector2.zero;
		StartCoroutine(Rescale(maxSize / 2));
		StartCoroutine(FallApart());
		levelManager.AddTrain(rb2.position);
	}

	internal IEnumerator NoAlpha(float time)
	{
		Color color = new Color(0f, 0f, 0f, (1 - parts[0].color.a) / time);
		while (time >= 0)
		{
			time -= Time.deltaTime;
			foreach (SpriteRenderer part in parts)
			{
				part.color += color * Time.deltaTime;
			}
			yield return null;
		}
	}

	internal IEnumerator Exploid()
	{
		rb2.velocity = Vector2.zero;
		GetComponent<CircleCollider2D>().enabled = false;
		StartCoroutine(Rescale(maxSize * 2));
		yield return StartCoroutine(Disappear(0.3f));
		Destroy(gameObject);
	}

	private IEnumerator FallApart()
	{
		foreach (SpriteRenderer part in parts)
		{
			part.GetComponent<EnemyPart>().FlyAway();
		}
		yield return StartCoroutine(Disappear(0.2f));
		Destroy(gameObject);
	}

	protected IEnumerator Disappear(float time)
	{
		Color color = new Color(0f, 0f, 0f, parts[0].color.a / time);
		while (parts[0].color.a > 0)
		{
			time -= Time.deltaTime;
			foreach (SpriteRenderer part in parts)
			{
				part.color -= color * Time.deltaTime;
			}
			yield return null;
		}
	}
	private void Update()
	{
		if (!backing)
		{
			if (!levelManager.isGameStart)
			{
				rb2.velocity = rb2.position.normalized * 5;
				backing = false;
			}
		}
	}
}
