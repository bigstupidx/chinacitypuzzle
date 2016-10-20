using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class ResultController : MonoBehaviour {

	[SerializeField] Button button;

	// Use this for initialization
	void Start () {
		GameObject.Find ("ModeText").gameObject.GetComponent<Text> ().text = GameManager.Instanse.GetModeName();
		GameObject.Find ("ScoreText").gameObject.GetComponent<Text> ().text = GameManager.Instanse.Score.Value.ToString ();
		GameObject.Find ("GradeText").gameObject.GetComponent<Text> ().text = GameManager.Instanse.GetGradeName ();

		// Not pressed more than once button
		button.OnClickAsObservable ().First ().Subscribe (_ => {
			SceneController.Instanse.ChangeTitle ();
		});
	}
}
