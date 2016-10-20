using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

public class CityInfoMainController : MonoBehaviour {

	// The position and controlling the movement of the ball
	CityInfoBallController cityInfoBallCtrl;

	// Camera to project a map
	MapCameraController mapCameraCtrl;

	// City information setting
	CityInfoGUIController cityInfoGUICtrl;

	// For the city of mark display
	CityMarkController cityMarkCtrl;

	float distance = 50.0f;

	void Awake() {
		cityInfoBallCtrl = GameObject.Find ("Balls").GetComponent<CityInfoBallController> ();
		mapCameraCtrl = GameObject.Find ("MapCamera").GetComponent<MapCameraController>();
		cityInfoGUICtrl = GameObject.Find ("GameController").GetComponent<CityInfoGUIController> ();


		// クリックイベントを監視
		this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown (0))
			.Select(_ => Input.mousePosition)
			.Subscribe(x => ChangeCityView(x));
	}

	// Use this for initialization
	void Start () {
		// For debugging of this scene
		//GameManager.Instanse.initialize ();
		//GameManager.Instanse.CreateInPlayCity ();

		// For debugging of this scene
		//var list = new List<int>{0,1,2,3,4,5,6,7};//北京-黑龙江省
		//var list = new List<int>{8,9,10,11,12,13,14,15};//上海市-
		//var list = new List<int>{16,17,18,19,20,21,22,23};//湖北省-
		//var list = new List<int>{24,25,26,27,28,29,30,31,32};//云南省-
		//GameManager.Instanse.CreateInPlayCityByIndex(list);


		List<City> cityList = GameManager.Instanse.GetCityListInMaingame ();

		cityInfoBallCtrl.SetCityList(cityList);
		cityInfoBallCtrl.ViewBalls ();
		InfoBall info = cityInfoBallCtrl.GetCenterInfoBall ();
		// The initial display of the camera to project a map
		Vector3 cameraPos = new Vector3(info.City.capitalCameraX, info.City.capitalCameraY, info.City.capitalCameraSize);
		Vector3 cityMarkPos = new Vector3(info.City.capitalPosX, info.City.capitalPosY, info.City.capitalSize);
		mapCameraCtrl.SetInitialPosition(cameraPos, cityMarkPos);
		// Initialization of place names
		cityInfoGUICtrl.SetCity(info.City);
		// The initial display of pinyin
		info.initializeCenter();
	}

	/// <summary>
	/// Return the clicked ball. Returns null if the ball is not clicked.
	/// </summary>
	GameObject GetInfoBallRayHit(Vector3 input) {
		// Conversion, click the screen coordinates ray
		Ray ray = Camera.main.ScreenPointToRay (input);
		// To store the information of the object that hit the Ray
		RaycastHit hit = new RaycastHit ();

		// Calculate the hit determination
		Physics.Raycast (ray, out hit, distance);

		return hit.collider != null ? hit.collider.gameObject : null;
	}

	/// <summary>
	/// To update the screen on the basis of the city information of the ball of the clicked position
	/// </summary>
	/// <param name="input">Input.</param>
	void ChangeCityView(Vector3 input) {
		// Get the information of the clicked ball
		GameObject ballObj = GetInfoBallRayHit(input);
		// When the ray hits the object
		if (ballObj != null) {
			InfoBall movedBall = cityInfoBallCtrl.MoveCityBall (ballObj);
			if (movedBall != null) {
				// Movement of the map camera
				mapCameraCtrl.MoveCamera (new Vector3(movedBall.City.capitalCameraX, movedBall.City.capitalCameraY, movedBall.City.capitalCameraSize)
					, new Vector3(movedBall.City.capitalPosX, movedBall.City.capitalPosY, movedBall.City.capitalSize));
				// Update city name
				cityInfoGUICtrl.SetCity(movedBall.City);
				// Todo : Cool effects, such as an airplane flies
			}
		}
	}
}
