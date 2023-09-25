using UnityEngine;

public class BackBoard: MonoBehaviour
{

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Train train;
		if (train = collision.GetComponentInParent<Train>())
		{
			if (!train.returning)
				if (train.rb.velocity.magnitude < train.speedAtDeparture.magnitude)
				{
					train.rb.velocity = Vector2.zero;
					train.StartReturnToOrbit();
				}
		}
	}
}
