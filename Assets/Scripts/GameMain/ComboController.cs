using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace ZhStg.GameMain
{
	public class ComboController : MonoBehaviour
	{
		GUIController guiController;

		void Awake ()
		{
			GameObject gameControllerObj = GameObject.Find ("GameControllers");
			guiController = gameControllerObj.GetComponent<GUIController> ();
		}

		// Use this for initialization
		void Start ()
		{
			// Change the display when the combo count has changed
			GameManager.Instanse.CurrentComboCount.Subscribe (count => {
				// Update the display until the 99
				if (count > 0) {
					// Count up until 99
					if (count <= 99) {
						guiController.SetComboCoount ("连续 " + count.ToString () + "次");
						// TODO:In the case of the combo, to animate
						if (count > 1) {
							// TODO: To animate
						}
					}
				} else {
					// Do not show anything when the 0
					guiController.SetComboCoount ("");
				}
			}).AddTo (gameObject);
		}
	}
}
