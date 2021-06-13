using UnityEngine;
using UnityEngine.Events;

public class TurnOnCollision : MonoBehaviour
{
	public UnityEvent OnTurnNeeded = null;
	private void OnCollisionEnter2D(Collision2D other)
	{
		OnTurnNeeded.Invoke();
	}
}
