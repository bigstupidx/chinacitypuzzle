using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CityInfoBallController : MonoBehaviour {

	// Parent object of the ball to be displayed
	GameObject balls;

	// Hold the selected ball
	GameObject centerBall;

	public List<City> cityListIngame;

	// Use this for initialization
	void Awake() {
		balls = GameObject.Find ("Balls");
	}

	public void SetCityList(List<City> list) {
		cityListIngame = list;
	}

	// To place the ball of the city in the form of a ring
	public void ViewBalls() {
		int ballCount = cityListIngame.Count;

		for (int i = 0; i < ballCount; i++) {
			float rad = ((360.0f / ballCount * i) - 90 ) * Mathf.Deg2Rad;
			Vector3 pos = new Vector3 (Mathf.Cos (rad) * 2.8f, 5.0f, Mathf.Sin (rad) * 2.8f);
			GameObject go = Instantiate (Resources.Load ("Prefabs/InfoSphere"), pos, Quaternion.identity) as GameObject;
			go.transform.Rotate(30.0f, 0.0f, 0.0f);
			go.transform.SetParent (balls.transform);

			// Set the city information on the ball
			go.GetComponent<InfoBall> ().City = cityListIngame [i];

			// Storing a ball that is displayed on the center
			if (i == 0) {
				go.GetComponent<InfoBall> ().SetCenterColor ();
				centerBall = go;
			} else {
				go.GetComponent<InfoBall> ().SetNonSelectedColor ();
			}
		}

		balls.transform.Rotate (-60.0f, 0.0f, 0.0f);
		balls.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
	}

	/// <summary>
	/// Replace the selected ball screen and the front row of the ball
	/// </summary>
	/// <returns>The city ball.</returns>
	/// <param name="obj">Object.</param>
	public InfoBall MoveCityBall(GameObject obj) {

		InfoBall movedCenterInfoBall = null;

		GameObject hitBall = obj;
		if (hitBall != centerBall) {
			InfoBall centerBallScript = centerBall.GetComponent<InfoBall> ();
			InfoBall hitBallScript = hitBall.GetComponent<InfoBall> ();
			if (!centerBallScript.isMoving && !hitBallScript.isMoving) {
				// Swap the position by moving the ball
				Vector3 centerBallPos = centerBall.transform.position;
				Vector3 hitBallPos = hitBall.transform.position;
				centerBall.GetComponent<InfoBall> ().MoveToOut (hitBallPos);
				hitBallScript.MoveToCenter (centerBallPos);
				// Stores the selected ball
				centerBall = hitBall;

				// Return the InfoBall instance of the ball moved to the center
				movedCenterInfoBall = hitBallScript;
			}
		}

		return movedCenterInfoBall;
	}

	/// <summary>
	/// Get the center of the ball script
	/// </summary>
	/// <returns>The center info ball.</returns>
	public InfoBall GetCenterInfoBall() {
		return centerBall.GetComponent<InfoBall> ();
	}
}
