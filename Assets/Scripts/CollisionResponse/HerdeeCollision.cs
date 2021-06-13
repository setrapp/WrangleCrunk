using UnityEngine;

public class HerdeeCollision : MonoBehaviour
{
	public HerdSteering Steering;

	private void OnTriggerEnter2D(Collider2D other)
	{
		var otherHerdee = other.GetComponent<HerdeeCollision>();
		if (otherHerdee != null && otherHerdee.Steering != null)
		{
			otherHerdee.Steering.MetHerdee(Steering);
		}
	}
}
