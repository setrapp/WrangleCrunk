using UnityEditor;
using UnityEngine;

	public class QuitOnEscape : EscapeResponder
	{
		protected override void HandleEscape()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}