using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace ZhStg.GameMain
{
	public class ProceedButtonController : MonoBehaviour
	{
		[SerializeField] Button button;

		GameMainController gameMainController;

		void Awake ()
		{
			gameMainController = GameObject.Find ("GameControllers").GetComponent<GameMainController> ();
		}

		// Use this for initialization
		void Start ()
		{
			// Not pressed more than once button
			button.OnClickAsObservable ().First ().Repeat().Subscribe (_ => {
				gameMainController.PauseOff();
			});
		}
	}
}