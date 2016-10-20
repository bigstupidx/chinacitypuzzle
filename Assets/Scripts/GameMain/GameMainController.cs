using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace ZhStg.GameMain
{
	public class GameMainController : MonoBehaviour
	{

		CityRoulette cityRoulette;
		GUIController guiController;

		string[] startCountName = { "一", "二", "三" };
		string startName = "开始!";

		// Lv per
		float nowPer = 0.0f;

		// ray distance
		float distance = 50f;

		void Awake ()
		{
			GameObject gameControllerObj = GameObject.Find ("GameControllers");
			cityRoulette = gameControllerObj.GetComponent<CityRoulette> ();
			guiController = gameControllerObj.GetComponent<GUIController> ();
		}

		// Use this for initialization
		void Start ()
		{
			StartCoroutine ("StartCount");

			guiController.SetScore (0);
			guiController.SetRank (GameManager.Instanse.GetGradeName ()); // 初めのランク
			GameManager.Instanse.CreateInPlayCity ();
			GameManager.Instanse.CurrentScorePer.Subscribe (per => {
				guiController.UpdateBar (per);
				// Update the percentage display
				guiController.SetScore ((int)(per * 100));
				// It ranks up After more than 100% percent
				if (nowPer > per) {
					guiController.SetRank (GameManager.Instanse.GetGradeName ());
					SoundManager.Instance.PlaySE (3);
				}

				this.nowPer = per;
			}).AddTo (gameObject);
		}
	
		// Update is called once per frame
		void Update ()
		{
			// Tap processing
			if (Input.GetMouseButtonDown (0)) {
				if (GameManager.Instanse.inPlay) {
					// Conversion, tap the screen coordinates ray
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					// To store the information of the object that hit the Ray
					RaycastHit hit = new RaycastHit ();
					// When the ray hits the object
					if (Physics.Raycast (ray, out hit, distance)) {
						// Get the name of the ray hits object
						Transform chineseTextTrf = hit.collider.gameObject.transform.FindChild ("ChineseText");
						if (chineseTextTrf != null) {
							string shortName = chineseTextTrf.gameObject.GetComponent<TextMesh> ().text;

							// Remove the ball you were hit
							GameObject hitObject = hit.collider.gameObject;
							BallState bs = hitObject.GetComponent<BallState> ();
							if (shortName == cityRoulette.nowShortName) {
								// Additional 
								GameManager.Instanse.AddScore ();
								// Sound the music of the correct answer
								SoundManager.Instance.PlaySE (0);
								// To view the Pinyin
								City city = CityGenerator.Instance.GetCityByShortName (shortName);
								Vector3 pinyinPos = hitObject.transform.position;
								// Adjusting the position of pinyin to pop-up
								pinyinPos.x -= hitObject.transform.localScale.x / 4;
								GameObject pinyinPopup = Instantiate (Resources.Load ("Prefabs/PopupTextGen"), pinyinPos, Quaternion.identity) as GameObject;
								var popupScript = pinyinPopup.GetComponent<PopupTextGen> ();
								popupScript.Popup (city.shortNamePinyin);
								// explosion
								Instantiate (Resources.Load ("Prefabs/BlueLineParticle"), hitObject.transform.position, Quaternion.identity);
								// remove ball
								Destroy (hitObject);
								// combo + 1
								GameManager.Instanse.IncrementComboCount ();
							} else {
								// Processing at the time of the incorrect answer
								// Redden the ball
								bs.SetNg ();
								// Play a sound when the wrong answer
								SoundManager.Instance.PlaySE (1);
								// Clear combo count
								GameManager.Instanse.ClearComboCount ();
							}
						}
					}
				}
			}
		}

		IEnumerator StartCount ()
		{
			for (int i = 0; i < startCountName.Length; i++) {
				guiController.SetReadyText (startCountName [i]);
				yield return new WaitForSeconds (1.5f);
			}

			guiController.SetReadyText (startName);
			float closeTime = 0.0f;
			while (closeTime < 2.0f) {
				closeTime += Time.deltaTime;
				yield return null;
			}

			// It performs processing after the countdown
			AfterCountdown ();
		}

		/// <summary>
		/// processing after the countdown
		/// </summary>
		void AfterCountdown ()
		{
			// To disable the countdown panel of
			guiController.SetStartCounterEnabled (false);
			// To be able to tap on the screen
			GameManager.Instanse.SetInPlay (true);
			// Play BGM
			SoundManager.Instance.volume.BGM = 0.6f;
			SoundManager.Instance.PlayBGM ((int)BGM.MainGame);
		}

		/// <summary>
		/// The name of the provincial capital set to the manager to get to the random, return the abbreviation of the acquired saving
		/// </summary>
		/// <returns>Abbreviation of the provincial</returns>
		int GetShortCityIndexRandom ()
		{
			return Random.Range (0, GameManager.Instanse.inPlayCityNums.Count);
		}

		/// <summary>
		/// Create ball
		/// </summary>
		/// <param name="pos">Position.</param>
		public void CreateCityBall (Vector3 pos)
		{
			Vector3 instantPos = pos;
			// Adjustment of the position to fire the ball
			instantPos.x = instantPos.x + Time.deltaTime * 2;
			instantPos.y = instantPos.y - 6.0f;
			// create ball
			GameObject obj = Instantiate (Resources.Load ("Prefabs/Sphere"), instantPos, Quaternion.identity) as GameObject;
			// Set of characters to be displayed on the ball
			int cityIndex = GetShortCityIndexRandom ();
			int playCityindex = GameManager.Instanse.inPlayCityNums [cityIndex];
			obj.transform.FindChild ("ChineseText").gameObject.GetComponent<TextMesh> ().text 
				= CityGenerator.Instance.GetCityByIndex (playCityindex).shortName;

			// Show only beginner mode only by selecting a color
			if (GameManager.Instanse.GameMode == GameModeEnum.Beginner) {
				Color color = obj.GetComponent<BiginnerBallState>().BallColors[cityIndex];
				obj.GetComponent<BallState> ().initialize (color);
			}
		}

		public void PauseOn() {
			Time.timeScale = 0.0f;
			GameManager.Instanse.inPlay = false;
			guiController.DispGameQuitPanel ();
		}

		public void PauseOff() {
			guiController.HideGameQuitPanel ();
			Time.timeScale = 1.0f;
			GameManager.Instanse.inPlay = true;
		}

		public void GoToResult() {
			if (SoundManager.Instance != null) {
				SoundManager.Instance.PlaySE (5);
			}
			// TODO: for fade animation
			Time.timeScale = 1.0f;
			// Changing Scene
			SceneController.Instanse.ChangeResult ();
		}
	}
}
