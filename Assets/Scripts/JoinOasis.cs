using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JoinOasis : MonoBehaviour
{
	[Serializable]
	public class UnityEventTransform : UnityEvent<Transform> { }
	public UnityEventTransform OnJoin = null;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (LayerMask.LayerToName(other.gameObject.layer) == "OasisJoin")
		{
			OnJoin.Invoke(other.transform);
		}
	}
}
