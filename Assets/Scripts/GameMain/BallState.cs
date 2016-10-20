using UnityEngine;
using System.Collections;

namespace ZhStg.GameMain
{
	public class BallState : MonoBehaviour
	{

		public float duration = 0.5f;
		public Color ngColor;
		Color oldColor;
		bool IsNg;

		public void initialize(Color color) {
			gameObject.GetComponent<Renderer> ().material.color = color;
		}

		/// <summary>
		/// When you tap the ball was wrong
		/// </summary>
		public void SetNg ()
		{
			// 
			if (IsNg) {
				return;
			}

			// Ng -> true
			IsNg = true;

			// Save the original color, set the color at the time of the incorrect answer
			oldColor = gameObject.GetComponent<Renderer> ().material.color;
			gameObject.GetComponent<Renderer> ().material.color = ngColor;

			// Revert to the original color after the specified seconds
			StartCoroutine (Execute ());
		}

		private IEnumerator Execute ()
		{
			yield return new WaitForSeconds (duration);
			gameObject.GetComponent<Renderer> ().material.color = oldColor;
			// clear ng flag
			IsNg = false;
		}
	}
}
