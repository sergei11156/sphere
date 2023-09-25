using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope: MonoBehaviour
{
	public HingeJoint2D hj;
	public Rigidbody2D rb;
	public RelativeJoint2D rj;
	public SpriteRenderer sprite;
	public Transform ropeScaler;
	public int ropeIndex;

	internal Train train;
	internal bool trainOnRope;
	internal bool imFree = true;
	private float timeToRescale;
	private Vector2 deltaScale;

	internal IEnumerator CatchTrain(Train train, float time)
	{
		this.train = train;
		StopAllCoroutines();
		JointMotor2D motor = hj.motor;
		motor.motorSpeed = Vector2.SignedAngle(train.rb.position.normalized, transform.right);
		if (motor.motorSpeed > 0)
		{
			motor.motorSpeed -= 360;
		}
		float rotationToCatchTime = (time / 4);
		motor.motorSpeed /= rotationToCatchTime;
		hj.motor = motor;
		hj.enabled = true;
		timeToRescale = rotationToCatchTime * 3;
		yield return new WaitForSeconds(rotationToCatchTime);
		StartCoroutine(Rescale(1f, timeToRescale));
	}

	internal IEnumerator Rescale(float newScale, float time)
	{
		hj.enabled = false;
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0f;

		timeToRescale = time;
		deltaScale = new Vector2(newScale - ropeScaler.localScale.x, newScale - ropeScaler.localScale.x);
		deltaScale /= time;
		while (timeToRescale > 0)
		{
			ropeScaler.localScale += (Vector3)deltaScale * Time.deltaTime;
			timeToRescale -= Time.deltaTime;
			yield return null;
		}
	}

}
