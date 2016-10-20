using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class TitleButtonController : MonoBehaviour {

	public GameModeEnum gameMode = GameModeEnum.Easy;

	[SerializeField] Button button;

	// Use this for initialization
	void Start () {
		// Not pressed more than once button
		button.OnClickAsObservable ().First ().Subscribe (_ => {
			SoundManager.Instance.PlaySE (5);
			GameManager.Instanse.SetGameMode(gameMode);
			SceneController.Instanse.ChangeMainGame ();
		});
	}
}
