using System;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(Animator))]
	public class SetAnimatorBool : MonoBehaviour
	{
		[SerializeField] private string parameter;
		private Animator anim;

		private void Awake()
		{
			anim = GetComponent<Animator>();
		}

		public void SetBool(bool value)
		{
			anim.SetBool(parameter, value);
		}
	}
}