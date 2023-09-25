using System;
using System.Collections;
using UnityEngine;

public class Train: MonoBehaviour
{

	public Rigidbody2D rb;
	public FixedJoint2D fj;
	public TheCenter theCenter;

	internal LevelManager levelManager;
	internal Train lastTrain;
	internal Train nextTrain;
	internal Rope rope;
	internal Vector2 speedAtDeparture;

	internal float power = 0.5f;
	internal bool isFly = false;
	internal bool returning;
	internal bool onHideOrbit;
	internal bool isFirstFly { get; private set; }
	internal int indexOfTrainSprite;

	bool isCollisionWhithEnemy = false;
	//int hideOrbitAngel;

	private Vector2 acceleration;
	private float timeToReturn;
	private Vector2 achorVelosity;
	private bool goToNewOrbit;
	private float gtnoTime;

	private void Start()
	{
		isFirstFly = true;
	}

	private void Update()
	{
		if (returning)
		{
			rb.velocity += acceleration * Time.deltaTime;

			if (timeToReturn <= 0)
			{
				isFirstFly = false;

				acceleration = Vector2.zero;
				rb.velocity = Vector2.zero;

				returning = false;
				rb.angularVelocity = 0f;
				if (onHideOrbit)
				{
					ChangeSprite(levelManager.trainSprite);
					//theCenter = levelManager.theCenter.GetComponentInParent<TheCenter>();
					theCenter.TrainsOnCenter++;
					Destroy(gameObject);                        //WARNING!!
				}
				else
				{
					rope.rj.enabled = true;
					rope.imFree = false;
					rope.trainOnRope = true;
					rope.rj.connectedBody = rb;
					JointMotor2D motor = rope.hj.motor;
					motor.motorSpeed = levelManager.road.speedOfMotor;
					rope.hj.motor = motor;
					rope.hj.enabled = true;

					if (levelManager.train)
					{
						if (levelManager.train.rope.ropeIndex < rope.ropeIndex)
						{
							lastTrain = levelManager.train;
							lastTrain.nextTrain = this;
							lastTrain.ChangeSprite(levelManager.trainSprite);
							levelManager.train = this;
						}
						else
						{
							nextTrain = levelManager.train;
							nextTrain.lastTrain = this;
							ChangeSprite(levelManager.trainSprite);
						}
					}
					else
					{
						levelManager.train = this;
					}
				}
				isFly = false;
			}
			else
			{
				timeToReturn -= Time.deltaTime;
			}
		}
		if (goToNewOrbit)
		{
			if (gtnoTime <= 0)
			{
				goToNewOrbit = false;
			}
			else
			{
				gtnoTime -= Time.deltaTime;
			}
		}
	}

	internal IEnumerator StopReturnAndDie()
	{
		acceleration = -acceleration * 2;
		returning = false;
		while (true)
		{
			if (rb)
				rb.velocity += acceleration * Time.deltaTime;
			else break;
			yield return null;
		}
	}

	public void ChangeSprite(Sprite sprite)
	{
		GetComponent<SpriteRenderer>().sprite = sprite;
	}

	internal void ActivateFly()
	{
		isFly = true;

		if (lastTrain)
		{
			levelManager.train = lastTrain;
			lastTrain.nextTrain = null;
			lastTrain.ChangeSprite(levelManager.trainHighlited);
		}
		else
		{
			levelManager.train = null;
		}

		StartCoroutine(rope.Rescale(0.1f, 0.3f));
		FreePlace();
		rb.angularVelocity = 0;
		if (rb.velocity.magnitude == 0)
		{
			rb.velocity = (Vector2)transform.up * 4;
		}
		speedAtDeparture = rb.velocity;
	}

	internal void ActivateGoOut()
	{
		isFly = true;
		if (lastTrain)
		{
			levelManager.train = lastTrain;
			lastTrain.nextTrain = null;
			lastTrain.ChangeSprite(levelManager.trainHighlited);
		}
		else
		{
			levelManager.train = null;
		}

		FreePlace();

		rb.angularVelocity = 0;
		if (rb.velocity.magnitude == 0)
		{
			rb.velocity = (Vector2)transform.up * 4;
		}
		speedAtDeparture = Vector2.zero;
	}

	internal void FreePlace()
	{
		if (rope)
		{
			rope.imFree = true;
			rope.rj.enabled = false;
			rope.trainOnRope = false;
			rope.train = null;
			rope = null;
		}

		if (lastTrain)
		{
			if (lastTrain.nextTrain)
				lastTrain.nextTrain = null;
			lastTrain = null;
		}

		if (nextTrain)
		{
			nextTrain.lastTrain = null;
			nextTrain = null;
		}
	}

	public bool IsFly()
	{
		return isFly;
	}

	internal void StartReturnToOrbit()
	{
		returning = true;
		acceleration = -rb.position.normalized * 3;
		rope = levelManager.GetFreeRope();
		if (rope && 2 * (transform.position.magnitude - rope.transform.localScale.x) >= 0)
		{
			rope.imFree = false;
			timeToReturn = Mathf.Sqrt(2 * (transform.position.magnitude - rope.transform.localScale.x) / acceleration.magnitude);
			StartCoroutine(rope.CatchTrain(this, timeToReturn));
			float angle = -(Vector3.Angle(transform.position.normalized, transform.up) - 90);
			rb.angularVelocity = angle / timeToReturn;
		}
		else
		{
			timeToReturn = Mathf.Sqrt(2 * transform.position.magnitude / acceleration.magnitude);
			onHideOrbit = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Enemy enemy;
		if (enemy = collision.GetComponentInParent<Enemy>())
		{
			if (isFly)
			{
				if (enemy is Coin)
				{
					if (!returning)
					{
						rb.velocity -= speedAtDeparture * power;
						if (rb.velocity.magnitude <= 0)
						{
							StartReturnToOrbit();
						}
					}
					StartCoroutine(enemy.gameObject.GetComponent<Coin>().Die());
				}
				else
				{ // if just enemy
					enemy.Die();

					if (!returning)
					{
						rb.velocity -= speedAtDeparture * power;
						if (rb.velocity.magnitude <= 0)
						{
							StartReturnToOrbit();
						}
						else
						{
							bool back = true;
							Vector2 offset = transform.right * levelManager.offset;
							{
								RaycastHit2D[] results = Physics2D.RaycastAll(rb.position + offset, rb.velocity.normalized, 7f);
								for (int i = 0; i < results.Length; i++)
								{
									if (results[i].collider != null)
									{
										Enemy enemy1 = results[i].transform.gameObject.GetComponent<Enemy>();
										if (enemy1 != null)
										{
											back = false;
											enemy1.NoAlpha((transform.position - enemy1.transform.position).magnitude / rb.velocity.magnitude);
											break;
										}
									}
								}
							}
							if (back)
							{
								RaycastHit2D[] results = Physics2D.RaycastAll(rb.position - offset, rb.velocity.normalized, 7f);
								for (int i = 0; i < results.Length; i++)
								{
									if (results[i].collider != null)
									{
										Enemy enemy1 = results[i].transform.gameObject.GetComponent<Enemy>();
										if (enemy1 != null)
										{
											back = false;
											enemy1.NoAlpha(((transform.position - enemy1.transform.position).magnitude - 1f) / rb.velocity.magnitude);
											break;

										}
									}
								}
								if (back)
								{
									rb.velocity = Vector2.zero;
									StartReturnToOrbit();
								}
							}
						}
					}
				}
			}
			else
			{
				StartCoroutine(enemy.Exploid());
				if (!isCollisionWhithEnemy)
				{
					isCollisionWhithEnemy = true;
					levelManager.AddCollisionEvent(this);
				}
			}

		}
	}
}
