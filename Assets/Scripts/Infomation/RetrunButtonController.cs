using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class RetrunButtonController : MonoBehaviour {

	[SerializeField] Button button;

	void Start() {
		// Not pressed more than once button
		button.OnClickAsObservable ().First ().Subscribe (_ => {
			SceneController.Instanse.ReturnResult ();
		});
	}
}
