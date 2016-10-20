using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class ReturnTitleController : MonoBehaviour {

	[SerializeField] Button button;

	// Use this for initialization
	void Start () {
		// Not pressed more than once button
		button.OnClickAsObservable ().First ().Subscribe (_ => {
			SceneController.Instanse.ChangeTitle();
		});
	}
}
