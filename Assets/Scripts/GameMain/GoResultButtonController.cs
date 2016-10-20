using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace ZhStg.GameMain
{
	public class GoResultButtonController : MonoBehaviour
	{

		[SerializeField] Button button;


		GameMainController gameMainController;

		void Awake ()
		{
			gameMainController = GameObject.Find ("GameControllers").GetComponent<GameMainController> ();
		}

		void Start ()
		{
			// Not pressed more than once button
			button.OnClickAsObservable ().First ().Subscribe (_ => {
				gameMainController.GoToResult();
			});
		}
	}
}
