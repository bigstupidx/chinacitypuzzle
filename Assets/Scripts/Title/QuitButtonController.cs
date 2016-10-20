using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class QuitButtonController : MonoBehaviour {

	/// <summary>
	/// Quit Game
	/// </summary>
	public void QuitApplication() {
	#if UNITY_STANDALONE
		// Quit the application
		Application.Quit();
	#endif

	#if UNITY_EDITOR
		// Stop playing the scene
		UnityEditor.EditorApplication.isPlaying = false;
	#endif

	#if UNITY_ANDROID
		Application.Quit ();
	#endif
	}
}
