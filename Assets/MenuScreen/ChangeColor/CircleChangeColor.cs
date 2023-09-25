using System.Collections;
using UnityEngine;

public class CircleChangeColor : MonoBehaviour {
	public SpriteRenderer sprite; 

	internal IEnumerator Grow(Color color, Vector2 pos)
	{
		sprite.enabled = true;
		transform.localScale = Vector2.zero;
		sprite.color = new Color(color.r, color.g, color.b, 1f);
		transform.position = new Vector3(pos.x, pos.y, 100f);
		const float newSize = 4;
		Vector3 changeScale = new Vector2(newSize, newSize);
		while(transform.localScale.x < newSize)
		{
			transform.localScale += changeScale * Time.deltaTime;
			yield return null;
		}
	} 
}
